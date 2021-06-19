using System.Collections.Generic;

namespace Tiles.Modules
{
    public class KingModule : ChessModule
    {
        public override TileType TileType => TileType.King;
        public KingModule(PlayerTile playerTile, List<string> parameters, string chessPiece, string color, bool oneSwipeOnly = false) : base(playerTile, parameters, chessPiece, color, oneSwipeOnly) => playerTile.SetSwipeMode(SwipeStyle.EightWay, false);
    }
}