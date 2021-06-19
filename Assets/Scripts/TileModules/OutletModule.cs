using ExtensionMethods;
using System.Collections.Generic;
using static Tiles.Modules.ModuleConstants;

namespace Tiles.Modules
{
    public class OutletModule : TileModule
    {
        public override TileType TileType => TileType.Outlet;
        public OutletModule(PlayerTile playerTile, List<string> parameters, bool oneSwipeOnly = false) : base(playerTile, parameters, OutletIndex, oneSwipeOnly) { }
    }
}
