using static PathDebugger;
using System.Collections.Generic;

namespace Tiles.Components
{
    public class PawnComponent : RookComponent
    {
        public override TileType TileType => TileType.Pawn;

        private bool hasSwiped = false;

        // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Methods    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
        public PawnComponent(PlayerTile playerTile, List<string> parameters, string chessPiece, string color, bool oneSwipeOnly = false) : base(playerTile, parameters, chessPiece, color, oneSwipeOnly) { }

        public override void Swipe(Direction direction, bool? wasPlayerTriggered, bool shouldSpawnOpposite, bool wasObstructed)
        {
            base.Swipe(direction, wasPlayerTriggered, shouldSpawnOpposite, wasObstructed);

            if (hasSwiped) CanCompoundSwipe = false;
            hasSwiped = true;
        }
    }
}