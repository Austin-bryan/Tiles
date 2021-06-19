using static PathDebugger;
using static BoardManager;
using System.Collections.Generic;

public partial class PlayerTile
{
    public class EightWaySwipeMode : SwipeMode
    {
        public override SwipeStyle Style => SwipeStyle.EightWay;

        public EightWaySwipeMode(PlayerTile tile) : base(tile) { }
        protected override void AddRange(ref List<PlayerTile> tiles, Direction direction, PlayerTile tile, ref bool foundWall)
        {
            if (direction.IsOrdinal()) tiles.AddRange(PBoard.GetOrdinalLayer(tile.Coord, direction.IsDescendingDiagonal(), ref foundWall));
            else tiles.AddRange(PBoard.GetCardinalLayer(tile.Coord, direction.IsRow(), ref foundWall));
        }
    }
}
