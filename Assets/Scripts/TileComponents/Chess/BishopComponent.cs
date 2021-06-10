using System.Collections.Generic;

namespace Tiles.Components
{
    public class BishopComponent : RookComponent
    {
        public override TileType TileType => TileType.Bishop;
        public BishopComponent(PlayerTile playerTile, List<string> parameters, string chessPiece, string color, bool oneSwipeOnly = false) : base(playerTile, parameters, chessPiece, color, oneSwipeOnly) => playerTile.SetSwipeMode(SwipeStyle.Diagonal, false);
    }
}