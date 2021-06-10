using System.Collections.Generic;
using static PathDebugger;
using static BoardManager;

public partial class PlayerTile
{
    public class DiagonalSwipeMode : SwipeModeTileCompiler
    {
        public override SwipeStyle Style => SwipeStyle.Diagonal;

        public DiagonalSwipeMode(PlayerTile tile) : base(tile) { }
        protected override void AddRange(ref List<PlayerTile> tiles, Direction direction, PlayerTile tile, ref bool foundWall) => tiles.AddRange(PBoard.GetDiagonal(tile.Coord, direction.IsUnsyncedDiag(), ref foundWall));
    }
}
