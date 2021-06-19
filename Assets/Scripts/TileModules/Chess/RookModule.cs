using System.Collections.Generic;

namespace Tiles.Modules
{
    public class RookModule : ChessModule
    {
        public override TileType TileType => TileType.Rook;

        protected bool CanCompoundSwipe = true;
        private Direction lastDirection;

        // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Methods    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
        public RookModule(PlayerTile playerTile, List<string> parameters, string chessPiece, string color, bool oneSwipeOnly = false) : base(playerTile, parameters, chessPiece, color, oneSwipeOnly) {  }
        public override void Swipe(Direction direction, bool? wasPlayerTriggered, bool shouldSpawnOpposite, bool wasObstructed)
        {
            base.Swipe(direction, wasPlayerTriggered, shouldSpawnOpposite, wasObstructed);

            if (!CanCompoundSwipe) return;

            if (direction == lastDirection) 
                TurnManager.ShouldSubtractMoves = false;
            lastDirection = direction;
        }
        public override void OnLevelMemberGroup() => lastDirection = Direction.None;
    }
}