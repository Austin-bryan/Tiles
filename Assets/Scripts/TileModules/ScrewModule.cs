using System.Collections.Generic;
using static Tiles.Modules.ModuleConstants;

namespace Tiles.Modules
{
    public class ScrewModule : ObstructionModule
    {
        public override TileType TileType    => TileType.Screw;
        protected override MoveType moveType => MoveType.Rotate;

        public ScrewModule(PlayerTile playerTile, List<string> parameters, bool oneSwipeOnly = false) : base(playerTile, parameters, ScrewsIndex, oneSwipeOnly) { }
    }
}