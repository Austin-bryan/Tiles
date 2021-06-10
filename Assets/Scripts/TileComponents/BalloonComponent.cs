using static TileType;
using ExtensionMethods;
using static Direction;
using static SwipeStyle;
using static PathDebugger;
using System.Collections.Generic;
using static Tiles.Components.ComponentConstants;

namespace Tiles.Components
{
    public class BalloonComponent : TileComponent
    {
        public override bool SubscribeToAllMoves => true;
        public override TileType TileType => Balloon;

        public static bool OverrideDiagonalMode;
        private bool hasBalloonSwiped;
        private bool shouldMakeDiagonal;

        // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Methods    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
        public BalloonComponent(PlayerTile playerTile, List<string> parameters, bool oneSwipeOnly = false) : base(playerTile, parameters, BalloonIndex, oneSwipeOnly) { AddToAllMovesFinished(true); }

        public override void AllMovesFinished()
        {
            if (shouldMakeDiagonal) Tile.SetSwipeMode(SwipeStyle.Diagonal, false);
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
                Tile.SetSwipeMode(Layer, false);
                shouldMakeDiagonal = true;
            }
        }
    }
}