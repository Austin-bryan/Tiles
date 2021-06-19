using System.Collections.Generic;

namespace Tiles.Modules
{
    public class BishopModule : RookModule
    {
        public override TileType TileType => TileType.Bishop;
        public BishopModule(PlayerTile playerTile, List<string> parameters, string chessPiece, string color, bool oneSwipeOnly = false) : base(playerTile, parameters, chessPiece, color, oneSwipeOnly) => playerTile.SetSwipeMode(SwipeStyle.Ordinal, false);
    }
}