using System.Collections.Generic;

namespace Tiles.Modules
{
    public class QueenModule : RookModule
    {
        public override TileType TileType => TileType.Queen;

        public QueenModule(PlayerTile playerTile, List<string> parameters, string chessPiece, string color, bool oneSwipeOnly = false) : base(playerTile, parameters, chessPiece, color, oneSwipeOnly) { }
        public override void SetVisibility(bool isVisible)
        {
            base.SetVisibility(isVisible);
            Tile.SetSwipeMode(isVisible ? SwipeStyle.EightWay : SwipeStyle.Cardinal, true);
        }
    }
}