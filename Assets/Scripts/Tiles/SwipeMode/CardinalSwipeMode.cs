using System.Collections.Generic;
using static PathDebugger;
using static BoardManager;

public partial class PlayerTile
{
    public class CardinalSwipeMode : SwipeMode
    {
        public override SwipeStyle Style => SwipeStyle.Cardinal;

        public CardinalSwipeMode(PlayerTile tile) : base(tile) { }

        protected override void AddRange(ref List<PlayerTile> tiles, Direction direction, PlayerTile tile, ref bool foundWall) =>
            tiles.AddRange(PBoard.GetCardinalLayer(tile.Coord, direction.IsRow(), ref foundWall));
    }
} // 48, 32, 13