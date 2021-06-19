using System;
using ExtensionMethods;
using System.Collections.Generic;
using static Tiles.Modules.ModuleConstants;

namespace Tiles.Modules
{
    public class ThisSideUpModule : TileModule
    {
        public override TileType TileType => TileType.ThisSideUp;
        public ThisSideUpModule(PlayerTile player, List<string> parameters, bool oneSwipeOnly = false) : base(player, parameters, ThisSideUpIndex, oneSwipeOnly) { }
        public override bool IsSolved() => Math.Floor(Tile.transform.eulerAngles.z) == 0;
    }
}
