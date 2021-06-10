using ExtensionMethods;
using System.Collections.Generic;
using static PathDebugger;

namespace Tiles.Components
{
    public abstract class TileComponent : IPlayerTile
    {
        // --------  Public Fields  -------- //
        public static List<(int, TileType)> ActivatedComponents { get; set; } = new List<(int, TileType)>();

        public abstract TileType TileType       { get; }
        public bool WasSwipedThisRound          => SwipeManager.ComponentsSweptThisSwipe.Contains(this);
        public virtual bool HasMiniVersion      => false;
        public virtual bool SubscribeToGlobal   => false;
        public virtual bool SubscribeToAllMoves => false;
        public virtual List<string> Parameters     { get; protected set; }
        public PlayerTile Tile                     { get; protected set; }
        public bool IsSubscribedToGlobalSwipe      { get; protected set; }
        public bool IsSubscribedToAllMovesFinished { get; protected set; }

        public readonly bool OneSwipeOnly;

        // --------  Protected Fields -------- //
        protected bool WasActivatedThisRound => ActivatedComponents.Contains((Tile.ID, TileType));

        protected bool IsVisible = true;
        protected int count = 0;

        // --------  Private Fields  -------- //
        private readonly int[] indexes;

        // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Methods    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
        public TileComponent(PlayerTile playerTile, List<string> parameters, int index, bool oneSwipeOnly = false) : this(playerTile, parameters, new[] { index }, oneSwipeOnly) { }
        public TileComponent(PlayerTile playerTile, List<string> parameters, int[] indexes, bool oneSwipeOnly = false)
        {
            (Tile, this.indexes, Parameters, OneSwipeOnly) = (playerTile, indexes, parameters, oneSwipeOnly);
            Show();
        }

        public virtual void Hide() => SetVisibility(false);
        public virtual void Show() => SetVisibility(true);
        public virtual void Remove()   => Hide();
        public virtual bool IsSolved() => true;
        public virtual void UpdateTile(PlayerTile newTile) => Tile = newTile;

        public virtual void OnFocusLost()   { }
        public virtual void OnCoordChange() { }
        public virtual void OnFinishParse() { }
        public virtual void AllMovesFinished()    { }
        public virtual void GlobalSwipeFinish()   { }
        public virtual void OnQueuedSwipeCalled() { }  // This gets called when a tile such as Balloon queues up a swipe to SwipeManager, and then that swipe is actually fired
        public virtual void FinishSwipe (bool isNewTile) { }
        public virtual void Activate    (bool wasPlayerTriggered) { }
        public virtual void OnWasWarped (bool wasPlayerTriggered) { }
        public virtual void OnWasRotated(bool wasPlayerTriggered) { }
        public virtual void BeginSwipe(Direction direction, bool playerTriggered) { }
        public virtual void ValidateSwipe(Direction direction, bool wasPlayerTrigered) { }
        public virtual void Swipe(Direction direction, bool? wasPlayerTriggered, bool shouldSpawnOpposite, bool wasObstructed) { }
        public virtual void SetVisibility(bool isVisible)
        {
            if (indexes[0] > -1) Tile.ShowSprite(isVisible, indexes);
            IsVisible = isVisible;

            if (IsSubscribedToGlobalSwipe) AddToGlobalSwipeNotify(isVisible);
            if (IsSubscribedToAllMovesFinished) AddToAllMovesFinished(isVisible);
        }
        public void TrySwipe(Direction direction, bool? wasPlayerTriggered, bool shouldSpawnOpposite, bool wasObstructed) { if (IsVisible) Swipe(direction, wasPlayerTriggered, shouldSpawnOpposite, wasObstructed); }

        protected void AddToGlobalSwipeNotify(bool shouldAdd)
        {
            if (shouldAdd) SwipeManager.GlobalMoveFinished += GlobalSwipeFinish;
            else SwipeManager.GlobalMoveFinished -= GlobalSwipeFinish;

            IsSubscribedToGlobalSwipe = shouldAdd;
        }
        protected void AddToAllMovesFinished(bool shouldAdd)
        {
            if (shouldAdd) SwipeManager.AllMovesFinished += AllMovesFinished;
            else SwipeManager.AllMovesFinished -= AllMovesFinished;
            
            IsSubscribedToAllMovesFinished = shouldAdd;
        }
        protected bool TrySwipe()
        {
            if (WasSwipedThisRound) return false;

            SwipeManager.ComponentsSweptThisSwipe.Add(this);
            return true;
        }
    }
}