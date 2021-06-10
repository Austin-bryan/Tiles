using static PathDebugger;
using static BoardManager;
using System.Collections.Generic;

public partial class PlayerTile
{
    public class HybridSwipeMode : SwipeModeTileCompiler
    {
        public override SwipeStyle Style => SwipeStyle.Hybrid;

        public HybridSwipeMode(PlayerTile tile) : base(tile) { }
        protected override void AddRange(ref List<PlayerTile> tiles, Direction direction, PlayerTile tile, ref bool foundWall)
        {
            if (direction.IsDiagonal()) tiles.AddRange(PBoard.GetDiagonal(tile.Coord, direction.IsUnsyncedDiag(), ref foundWall));
            else tiles.AddRange(PBoard.GetLayer(tile.Coord, direction.IsRow(), ref foundWall));
        }
    }
}
