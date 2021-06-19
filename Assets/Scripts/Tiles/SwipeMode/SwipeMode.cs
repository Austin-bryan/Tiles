using System.Linq;
using Tiles.Factories;
using Tiles.Modules;
using ExtensionMethods;
using System.Collections.Generic;
using static SwipeStyle;
using static PathDebugger;
using static BoardManager;
using static PlayerTile.ObstructionState;
using ObstructState = PlayerTile.ObstructionState;

public partial class PlayerTile
{
    public abstract class SwipeMode : MoveMode
    {
        public virtual SwipeStyle Style { get; }
        public SwipeStyle OriginalStyle { get; set; }
        public List<int> SwipeQueueIndexes { get; private set; }    // See if this line of code is even needed

        protected bool IsMovingIntoWall, MovementGroupHasWrapTile, ShouldDestroy;
        protected Direction CurrentDirection;

        private readonly static Dictionary<PlayerTile, bool> oneSwipeTiles = new Dictionary<PlayerTile, bool>();
        private static List<PlayerTile> movementGroup;

        private bool isTemporarilyObstructing;
        private const float scaleConstant = 38;

        // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Methods    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
        public SwipeMode(PlayerTile tile) : base(tile) => SwipeQueueIndexes = new List<int>();

        protected abstract void AddRange(ref List<PlayerTile> tiles, Direction direction, PlayerTile tile, ref bool foundWall);
        protected void GetMovementGroup(Direction direction, ref List<PlayerTile> layerTiles)
        {
            MovementGroupHasWrapTile = getLayer(out List<PlayerTile> foundTiles, direction.ToLayerType());
            layerTiles.AddRange(foundTiles);
            layerTiles.Remove(tile);

            // ---- Local Functions ---- //
            bool getLayer(out List<PlayerTile> tiles, LayerType layerType)
            {
                bool foundWall = false;

                tiles = new List<PlayerTile>();
                AddRange(ref tiles, direction, tile, ref foundWall);

                return foundWall;
            }
        }

        public static void AddToOneSwipeList(PlayerTile tile)
        {
            if (tile.Sides.HasOneSwipeModule() && !oneSwipeTiles.ContainsKey(tile)) oneSwipeTiles.Add(tile, true);
        }
        public static SwipeMode CreateNewSwipeMode(SwipeStyle swipeStyle, PlayerTile tile) => swipeStyle switch
        {
            Ordinal  => new OrdinalSwipeMode(tile),
            Cardinal => new CardinalSwipeMode(tile),
            EightWay => new EightWaySwipeMode(tile),
            _ => default,
        };

        // ---- Swiping ---- //
        public override void InitiateTilesInMovementGroup(Direction direction, bool playerTriggered)
        {
            Path4();
            List<PlayerTile> oldMovementGroup = null;

            if (movementGroup != null) oldMovementGroup = movementGroup.Duplicate();

            (movementGroup, CurrentDirection, hasBeenObstructed) = (new List<PlayerTile>(), direction, false);

            SwipeManager.BeginSwipe(CurrentDirection);
            tileAudio.PlaySwipe();

            GetMovementGroup(direction, ref movementGroup);

            // Checks the old movement group for tiles that are not in the current group, then notifies them to preform certain actions
            if (oldMovementGroup != null)
                foreach (var tile in oldMovementGroup)
                    if (!movementGroup.Contains(tile)) 
                        tile.Modules.ForEach(component => component.OnLevelMemberGroup());
            // If there is one 
            foreach (var tile in movementGroup)
                foreach (var component in tile.Modules)
                    component.ValidateSwipe(CurrentDirection, playerTriggered);

            (movementGroup, CurrentDirection, hasBeenObstructed) = (new List<PlayerTile>(), direction, false);
            GetMovementGroup(direction, ref movementGroup);

            // Find Obstructing tiles
            for (int i = 0; i < movementGroup.Count; i++)
            {
                var tile = movementGroup[i];
                if (tile == null) continue;

                if (tile.SwipeStyle == Ordinal && SwipeManager.CurrentDirection.IsLayer())
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
            for (int i = 0; i < movementGroup.Count; i++)
            {
                var tile = movementGroup[i];
                if (tile == null || !tile.IsMovable) continue;

                tile.SwipingMode.MovementGroupHasWrapTile = MovementGroupHasWrapTile;
                tile.SwipingMode.StartMovement(direction, playerTriggered, true);
            }
        }
        public override void StartMovement(Direction direction, bool? _playerTriggered, bool shouldSpawnOpposite)
        {
            (CurrentDirection, playerTriggered) = (direction, _playerTriggered);
            GetNewCoord();

            var x = PBoard[NewCoord];

            tile.Modules.ForEach(t => t.TrySwipe(direction, playerTriggered, shouldSpawnOpposite, hasBeenObstructed));

            var oldTrans = tile.transform.ToTransform();

            if (hasBeenObstructed && ObstructionState == Idle) ObstructionState = NeedsToSwipeBack;
            else if (!NewCoord.IsInFrame() || IsMovingIntoWall)
            {
                SpawnTileInCurDirection(IsMovingIntoWall);
                ShouldDestroy = true;
            }

            tile.UpdateTargetLocation();
            tile.isBeingSwiped = true;
            tile.tileSpeed.UpdateObstruction(hasBeenObstructed);
        }
        public override void FinishMovement()
        {
            if (hasBeenObstructed && ObstructionState == NeedsToSwipeBack)
            {
                ObstructionState = IsSwipingBack;
                tile.Coord = NewCoord;
                tile.SwipingMode.StartMovement(CurrentDirection.GetOppositeDirection(), false, false);

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
            if (ShouldDestroy) tile.Delay(0.5f, tile.Destroy);
            if (!IsMovingIntoWall) tile.SetCoord(NewCoord);

            (tile.isBeingSwiped, IsMovingIntoWall, MovementGroupHasWrapTile) = (false, false, false);

            if (!hasBeenObstructed) tile.Delay(0f, SendPropertiesToPreviousTile);  // DON'T DO IF NOT PLAYER TRIGGERED
            if (tile.shouldRevertMode) RevertMode();
            if (IsTemporary)
            {
                tile.SetSwipeMode(OriginalStyle, false);
                IsTemporary = false;
            }

            tile.Modules.ForEach(c => c.FinishSwipe(tile.isNewTile));
            tile.UpdateSolved();
            tile.isNewTile = false;

            if (tile.queuedStyle != null)
            {
                tile.SetSwipeMode(tile.queuedStyle ?? Cardinal, false);
                tile.queuedStyle = null;
            }

            // ---- Local Functions ---- //
            void SendPropertiesToPreviousTile()
            {
                if (!(oneSwipeTiles.Keys.Contains(tile) && oneSwipeTiles[tile])) return;

                var types = new TileType[tile.Sides.Count][];
                var newTile = GetTile();
                int sideCount = 0;
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

                        if (factory == null || newTile.Sides.HasModule(type)) continue;

                        newTile.AddModule(type, factory.GetModule(newTile, tile.Get(type).Parameters, true));
                        newTile.AddModules();
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

                tile.Sides.RemoveOneSwipeModules();
                newTile.Sides.ChangeSide(initalSide);

                tile.Sides.RemoveOneSwipeSides();
            }
            PlayerTile GetTile() => PBoard.Get(CurrentDirection.IsHorizontal() ? NewCoord + CurrentDirection : NewCoord - CurrentDirection);
        }

        public static SwipeMode CreateSwipeMode(SwipeStyle style, PlayerTile tile) => style switch
        {
            SwipeStyle.Cardinal => new CardinalSwipeMode(tile),
            SwipeStyle.Ordinal  => new OrdinalSwipeMode(tile),
            SwipeStyle.EightWay => new EightWaySwipeMode(tile),
            _ => null
        };

        public Coord GetOppositeCoord()
        {
            if (CurrentDirection.IsOrdinal()) return NewCoord.GetOppositeDiagonal();
            return tile.Coord.GetOppositeCoord(CurrentDirection);
        }
        protected void GetNewCoord()
        {
            NewCoord = tile.Coord;

            do NewCoord += CurrentDirection;
            while (PBoard[NewCoord].To<PlayerTile>()?.HasGap() ?? false);

            tileMovingInto = PBoard[NewCoord].To<PlayerTile>();
            IsMovingIntoWall = tileMovingInto?.HasWall() ?? false;
        }

        private void SpawnTileInCurDirection(bool shouldSpawnOpposite)
        {
            var newTile      = Instantiate(tile);
            var workingCoord = NewCoord;
            workingCoord     = MovementGroupHasWrapTile ? WrapModule.GetCoordOfWallToSpawnAt(CurrentDirection) : (NewCoord - CurrentDirection).GetOppositeCoord(CurrentDirection);

            setupTile();

            // ---- Local Functions ---- //
            void setupTile()
            {
                newTile.SetSwipeMode(tile.originalMode, true);
                newTile.SetCoord(workingCoord);
                (newTile.name, newTile.shouldColor, newTile.SwipingMode.SwipeQueueIndexes) = ($"Tile {tile.ID}", false, SwipeQueueIndexes);
                newTile.Sides = tile.Sides;
                newTile.Sides.SetTile(newTile);
                newTile.SetPosition(PBoard.GetSlotTransform(workingCoord, true).Location);
                newTile.BeginPlay();
                newTile.UpdateID(tile.ID);
                newTile.SetParent(PBoard);
                newTile.SwipingMode.StartMovement(CurrentDirection, null, shouldSpawnOpposite);
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