using System.Collections.Generic;
using static PathDebugger;
using static BoardManager;

public partial class PlayerTile
{
    public class OrdinalSwipeMode : SwipeMode
    {
        public override SwipeStyle Style => SwipeStyle.Ordinal;
        
        public OrdinalSwipeMode(PlayerTile tile) : base(tile) { }

        protected override void AddRange(ref List<PlayerTile> tiles, Direction direction, PlayerTile tile, ref bool foundWall) => 
            tiles.AddRange(PBoard.GetOrdinalLayer(tile.Coord, direction.IsDescendingDiagonal(), ref foundWall));
    }
}
