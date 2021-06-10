using System.Collections.Generic;

namespace Tiles.Components
{
    public class KnightComponent : ChessComponent
    {
        public override TileType TileType => TileType.Knight;

        public KnightComponent(PlayerTile playerTile, List<string> parameters, string chessPiece, string color, bool oneSwipeOnly = false) : base(playerTile, parameters, chessPiece, color, oneSwipeOnly) { }
        public override void SetVisibility(bool isVisible)
        {
            base.SetVisibility(isVisible);
            Tile.SetSwipeMode(isVisible ? SwipeStyle.Hybrid : SwipeStyle.Layer, true);
        }
    }
}