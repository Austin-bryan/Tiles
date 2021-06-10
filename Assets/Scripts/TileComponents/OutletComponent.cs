using ExtensionMethods;
using System.Collections.Generic;
using static Tiles.Components.ComponentConstants;

namespace Tiles.Components
{
    public class OutletComponent : TileComponent
    {
        public override TileType TileType => TileType.Outlet;
        public OutletComponent(PlayerTile playerTile, List<string> parameters, bool oneSwipeOnly = false) : base(playerTile, parameters, OutletIndex, oneSwipeOnly) { }
    }
}
