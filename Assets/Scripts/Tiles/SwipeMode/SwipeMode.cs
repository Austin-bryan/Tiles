using System.Linq;
using Tiles.Factories;
using Tiles.Components;
using ExtensionMethods;
using System.Collections.Generic;
using static SwipeStyle;
using static PathDebugger;
using static BoardManager;
using static PlayerTile.ObstructionState;
using ObstructState = PlayerTile.ObstructionState;

public enum SwipeStyle { Layer, Diagonal, Hybrid } 

public partial class PlayerTile
{
    public abstract class SwipeMode : MoveMode
    {
        public virtual SwipeStyle Style       { get; }
        public SwipeStyle ModeToRevert        { get; set; }
        public List<int> SwipeQueueIndexes    { get; private set; }

        protected bool isMovingIntoWall, layerHasWall, shouldDestroy;
        protected Direction currentDirection;

        private readonly static Dictionary<PlayerTile, bool> oneSwipeTiles = new Dictionary<PlayerTile, bool>();
        private static List<PlayerTile> layerTiles;

        private bool isTemporarilyObstructing;
        private const float scaleConstant = 38;

        // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Methods    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
        public SwipeMode(PlayerTile tile) : base(tile) => SwipeQueueIndexes = new List<int>();

        protected abstract void GetLayerTiles(Direction direction, ref List<PlayerTile> tiles);

        public static void AddToOneSwipeList(PlayerTile tile)
        {
            if (tile.Sides.HasOneSwipeComponent() && !oneSwipeTiles.ContainsKey(tile)) oneSwipeTiles.Add(tile, true);
        }
        public static SwipeMode GetSwipeMode(SwipeStyle swipeStyle, PlayerTile tile) => swipeStyle switch
        {
            Layer    => new LayerSwipeMode(tile),
            Hybrid   => new HybridSwipeMode(tile),
            Diagonal => new DiagonalSwipeMode(tile),
            _        => default,
        };
     
        // Swiping //
        public virtual void BeginSwipe (Direction direction, bool playerTriggered)
        {
            List<PlayerTile> oldTiles = null;
            
            if (layerTiles != null) oldTiles = layerTiles.Duplicate();

            (layerTiles, currentDirection, hasBeenObstructed) = (new List<PlayerTile>(), direction, false);

            SwipeManager.BeginSwipe(currentDirection);
            tileAudio.PlaySwipe();

            GetLayerTiles(direction, ref layerTiles);

            if (oldTiles != null) oldTiles.ForEach(t =>
                {
                    if (!layerTiles.Contains(t)) t.Sides.CurrentSideComponents.ForEach(c => c.OnFocusLost());
                });
            layerTiles.ForEach(t => t.Sides.CurrentSideComponents.ForEach(c => c.ValidateSwipe(currentDirection, playerTriggered)));

            (layerTiles, currentDirection, hasBeenObstructed) = (new List<PlayerTile>(), direction, false);
            GetLayerTiles(direction, ref layerTiles);

            // Find Obstructing tiles
            for (int i = 0; i < layerTiles.Count; i++)
            {
                var tile = layerTiles[i];
                if (tile == null) continue;

                if (tile.SwipeStyle == Diagonal && SwipeManager.CurrentDirection.IsLayer())
                {
                    if (!tile.IsSwipeObstructed())
                    {
                        tile.SwipingMode.isTemporarilyObstructing = true;
                        tile.SetIsSwipeObstructed(true);
                    }
                }
                if (tile.IsSwipeObstructed()) hasBeenObstructed = true;
            }

            // Swipe tile if it should be
            for (int i = 0; i < layerTiles.Count; i++)
            {
                var tile = layerTiles[i];
                if (tile == null || !tile.IsMovable) continue;
                
                tile.SwipingMode.layerHasWall = layerHasWall;
                tile.SwipingMode.Swipe(direction, playerTriggered, true);
            }
        }
        public          void Swipe      (Direction direction, bool? _playerTriggered, bool shouldSpawnOpposite)
        {
            (currentDirection, playerTriggered) = (direction, _playerTriggered);
            GetNewCoord();

            var x = PBoard[NewCoord];

            tile.Sides.CurrentSideComponents.ForEach(t => t.TrySwipe(direction, playerTriggered, shouldSpawnOpposite, hasBeenObstructed));

            var oldTrans = tile.transform.ToTransform();

            if (hasBeenObstructed && ObstructionState == Idle) ObstructionState = NeedsToSwipeBack;
            else if (!NewCoord.IsInFrame() || isMovingIntoWall)
            {
                SpawnTileInCurDirection(isMovingIntoWall);
                shouldDestroy = true;
            }

            tile.UpdateTargetLocation();
            tile.isBeingSwiped = true;
            tile.tileSpeed.UpdateObstruction(hasBeenObstructed);
        }
        public override void FinishSwipe()
        {
            if (hasBeenObstructed && ObstructionState == NeedsToSwipeBack)
            {
                ObstructionState = IsSwipingBack;
                tile.Coord = NewCoord;
                tile.SwipingMode.Swipe(currentDirection.GetOppositeDirection(), false, false);

                return;
            }
            else if (ObstructionState == IsSwipingBack) 
            {
                ObstructionState = Idle;
                
                if (tile.SwipingMode.isTemporarilyObstructing && tile.IsSwipeObstructed())
                    tile.SetIsSwipeObstructed(false);
                return; 
            }

            if (SwipeManager.ActiveTile == tile) SwipeManager.FinishTurn();
            if (shouldDestroy)     tile.Delay(0.5f, tile.Destroy);
            if (!isMovingIntoWall) tile.SetCoord(NewCoord);

            (tile.isBeingSwiped, isMovingIntoWall, layerHasWall) = (false, false, false);

            if (!hasBeenObstructed) tile.Delay(0f, SendPropertiesToPreviousTile);  // DON'T DO IF NOT PLAYER TRIGGERED
            if (tile.shouldRevertMode) RevertMode();
            if (Temporary)
            {
                tile.SetSwipeMode(ModeToRevert, false);
                Temporary = false;
            }

            tile.Sides.CurrentSideComponents.ForEach(c => c.FinishSwipe(tile.isNewTile)); 
            tile.UpdateSolved();
            tile.isNewTile = false;

            if (tile.queuedStyle != null)
            {
                tile.SetSwipeMode(tile.queuedStyle ?? Layer, false);
                tile.queuedStyle = null;
            }

            // ---- Local Functions ---- //
            void SendPropertiesToPreviousTile()
            {
                if (!(oneSwipeTiles.Keys.Contains(tile) && oneSwipeTiles[tile])) return;

                var types      = new TileType[tile.Sides.Count][];
                var newTile    = GetTile();
                int sideCount  = 0;
                var initalSide = tile.Sides.CurrentIndex;

                // Gets all the types on each side and stores them in 2D array where the outer array represent a side
                for (int i = 0; i < tile.Sides.Count; i++)
                {
                    types[i] = new TileType[tile.Sides[i].Types.Count];

                    for (int j = 0; j < tile.Sides[i].Types.Count; j++)
                        types[i][j] = tile.Sides[i].Types[j];
                }

                // Loops through each side and adds the types to newTile
                foreach (TileType[] side in types)
                {
                    // Switches to next side if there is one
                    if (sideCount > 0)
                    {
                        // Adds a side to newTile if it needs it
                        if (newTile.Sides.Count < sideCount + 1)        
                            newTile.Sides.AddSide(newTile.Color, true);
                        newTile.Sides.NextSide();
                    }

                    // Loops through each type on the current side
                    foreach (TileType type in side)
                    {
                        // Doesn't add the type if its one swipe only
                        if (!tile.Get(type).OneSwipeOnly)
                            continue;

                        var factory = TileFactory.GetFactory(type);

                        if (factory == null || newTile.Sides.HasComponent(type)) continue;

                        newTile.AddComponent(type, factory.GetComponent(newTile, tile.Get(type).Parameters, true));
                        newTile.AddComponents();
                    }
                    sideCount++;
                }
                if (oneSwipeTiles[tile])
                {
                    oneSwipeTiles.Remove(tile);

                    if (!oneSwipeTiles.Keys.Contains(newTile))
                        oneSwipeTiles.Add(newTile, false);

                    tile.Delay(0.2f, bar);
                    void bar() => oneSwipeTiles[newTile] = true;
                }

                tile.Sides.RemoveOneSwipeComponents();
                newTile.Sides.ChangeSide(initalSide);

                tile.Sides.RemoveOneSwipeSides();
            }
            PlayerTile GetTile() => PBoard.Get(currentDirection.IsHorizontal() ? NewCoord + currentDirection : NewCoord - currentDirection);
        }
        
        public Coord GetOppositeCoord()
        {
            if (currentDirection.IsDiagonal()) return NewCoord.GetOppositeDiagonal();
            return tile.Coord.GetOppositeCoord(currentDirection);
        }
        protected void GetNewCoord()
        {
            NewCoord = tile.Coord;

            do NewCoord += currentDirection;
            while (PBoard[NewCoord].To<PlayerTile>()?.HasGap() ?? false);

            tileMovingInto   = PBoard[NewCoord].To<PlayerTile>();
            isMovingIntoWall = tileMovingInto?.HasWall() ?? false;
        }
        
        private void RevertMode()
        {
            if (tile.SwipeStyle != tile.originalMode)
            {
                tile.shouldRevertMode = false;
                tile.SetSwipeMode(tile.originalMode, false);
            }
            else tile.SetSwipeMode(tile.nextMode, false);
        }
        private void SpawnTileInCurDirection(bool shouldSpawnOpposite)
        {
            var newTile      = Instantiate(tile);
            var workingCoord = NewCoord;
            workingCoord     = layerHasWall ? WallComponent.GetCoordOfWallToSpawnAt(currentDirection) : (NewCoord - currentDirection).GetOppositeCoord(currentDirection);

            setupTile();

            // ---- Local Functions ---- //
            void setupTile()
            {
                newTile.SetSwipeMode(tile.originalMode, true);
                newTile.SetCoord(workingCoord);
                (newTile.name, newTile.shouldColor, newTile.SwipingMode.SwipeQueueIndexes) = ($"Tile {tile.ID}", false, SwipeQueueIndexes);
                (newTile.Sides, newTile.Directions) = (tile.Sides, tile.Directions.Duplicate());
                newTile.Sides.SetTile(newTile);
                newTile.SetPosition(PBoard.GetSlotTransform(workingCoord, true).Location);
                newTile.BeginPlay();
                newTile.UpdateID(tile.ID);
                newTile.SetParent(PBoard);
                newTile.SwipingMode.Swipe(currentDirection, null, shouldSpawnOpposite);
                newTile.MatchColor(tile);
                newTile.queuedStyle = tile.queuedStyle;
                newTile.isNewTile = true;
                newTile.SetParent(BoardManager.PBoard);
                newTile.SetLocalScale(scaleConstant);

                if (tile.shouldRevertMode) tile.Delay(0.1f, setupMode);

                SwipeManager.Replace(tile, newTile);
            }
            void setupMode()
            {
                if (!tile.shouldRevertMode) return;

                newTile.SetSwipeModeForOneSwipe(tile.nextMode);
                newTile.SetSwipeMode(tile.nextMode, false);
            }
        }
    }
} // 326, 286