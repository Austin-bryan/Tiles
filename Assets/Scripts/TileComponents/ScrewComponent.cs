using System.Collections.Generic;
using static Tiles.Components.ComponentConstants;

namespace Tiles.Components
{
    public class ScrewComponent : ObstructComponent
    {
        public override TileType TileType    => TileType.Screw;
        protected override MoveType moveType => MoveType.Rotate;

        public ScrewComponent(PlayerTile playerTile, List<string> parameters, bool oneSwipeOnly = false) : base(playerTile, parameters, ScrewsIndex, oneSwipeOnly) { }
    }
}