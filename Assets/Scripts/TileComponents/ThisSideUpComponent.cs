using System;
using ExtensionMethods;
using System.Collections.Generic;
using static Tiles.Components.ComponentConstants;

namespace Tiles.Components
{
    public class ThisSideUpComponent : TileComponent
    {
        public override TileType TileType => TileType.ThisSideUp;
        public ThisSideUpComponent(PlayerTile player, List<string> parameters, bool oneSwipeOnly = false) : base(player, parameters, ThisSideUpIndex, oneSwipeOnly) { }
        public override bool IsSolved() => Math.Floor(Tile.transform.eulerAngles.z) == 0;
    }
}
