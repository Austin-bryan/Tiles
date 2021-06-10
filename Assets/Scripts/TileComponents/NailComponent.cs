using System.Collections.Generic;
using static Tiles.Components.ComponentConstants;

namespace Tiles.Components
{
    public class NailComponent : ObstructComponent
    {
        public override TileType TileType    => TileType.Nail;
        protected override MoveType moveType => MoveType.Swipe;

        public NailComponent(PlayerTile playerTile, List<string> parameters, bool oneSwipeOnly = false) : base(playerTile, parameters, NailsIndex, oneSwipeOnly) { }
    }
}
