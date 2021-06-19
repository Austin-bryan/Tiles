using System.Collections.Generic;
using static Tiles.Modules.ModuleConstants;

namespace Tiles.Modules
{
    public class BoltModule : ObstructionModule
    {
        public override TileType TileType    => TileType.Bolt;
        protected override MoveType moveType => MoveType.Warp;

        public BoltModule(PlayerTile playerTile, List<string> parameters, bool oneSwipeOnly = false) : base(playerTile, parameters, BoltIndex, oneSwipeOnly) { }

    }
}