using static TileType;
using static Direction;
using static SwipeStyle;
using static PathDebugger;
using static Tiles.Modules.ModuleConstants;
using System.Collections.Generic;

namespace Tiles.Modules
{
    // After each swipe is finished, swipes the tile upwards
    public class BalloonModule : TileModule
    {
        public override bool SubscribeToAllMoves => true;
        public override TileType TileType => Balloon;

        public static bool OverrideDiagonalMode;
        private bool hasBalloonSwiped;
        private bool shouldMakeDiagonal;

        // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Methods    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
        public BalloonModule(PlayerTile playerTile, List<string> parameters, bool oneSwipeOnly = false) : base(playerTile, parameters, BalloonIndex, oneSwipeOnly) { AddToAllMovesFinished(true); }

        public override void AllMovesFinished()
        {
            if (shouldMakeDiagonal) Tile.SetSwipeMode(SwipeStyle.Ordinal, false);
            hasBalloonSwiped = shouldMakeDiagonal = false;
        }

        public override void Swipe(Direction direction, bool? wasPlayerTriggered, bool shouldSpawnOpposite, bool wasObstructed)
        {
            base.Swipe(direction, wasPlayerTriggered, shouldSpawnOpposite, wasObstructed);

            if ((wasPlayerTriggered == false && hasBalloonSwiped) || Tile.IsSwipeObstructed()) return;
            if (!TrySwipe()) return;

            hasBalloonSwiped = true;
            SwipeManager.AddDirection(Tile, Up, this);
        }
        public override void OnQueuedSwipeCalled()
        {
            base.OnQueuedSwipeCalled();

            if (!shouldMakeDiagonal && Tile.Has(TileType.Diagonal))
            {
                Tile.SetSwipeMode(Cardinal, false);
                shouldMakeDiagonal = true;
            }
        }
    }
}