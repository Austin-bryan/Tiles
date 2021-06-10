using System.Collections.Generic;
using static Tiles.Components.ComponentConstants;

namespace Tiles.Components
{
    public class BoltComponent : ObstructComponent
    {
        public override TileType TileType    => TileType.Bolt;
        protected override MoveType moveType => MoveType.Warp;

        public BoltComponent(PlayerTile playerTile, List<string> parameters, bool oneSwipeOnly = false) : base(playerTile, parameters, BoltIndex, oneSwipeOnly) { }

    }
}