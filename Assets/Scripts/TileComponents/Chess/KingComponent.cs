using System.Collections.Generic;

namespace Tiles.Components
{
    public class KingComponent : ChessComponent
    {
        public override TileType TileType => TileType.King;
        public KingComponent(PlayerTile playerTile, List<string> parameters, string chessPiece, string color, bool oneSwipeOnly = false) : base(playerTile, parameters, chessPiece, color, oneSwipeOnly) => playerTile.SetSwipeMode(SwipeStyle.Hybrid, false);
    }
}