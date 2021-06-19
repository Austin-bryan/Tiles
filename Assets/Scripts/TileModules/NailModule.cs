using System.Collections.Generic;
using static Tiles.Modules.ModuleConstants;

namespace Tiles.Modules
{
    public class NailModule : ObstructionModule
    {
        public override TileType TileType    => TileType.Nail;
        protected override MoveType moveType => MoveType.Swipe;

        public NailModule(PlayerTile playerTile, List<string> parameters, bool oneSwipeOnly = false) : base(playerTile, parameters, NailsIndex, oneSwipeOnly) { }
    }
}
