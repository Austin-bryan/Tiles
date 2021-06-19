using ExtensionMethods;
using static PathDebugger;
using System.Collections.Generic;
using static Tiles.Modules.ModuleConstants;

namespace Tiles.Modules
{
    public class GapModule : TileModule
    {
        public override TileType TileType => TileType.Gap;
        public GapModule(PlayerTile playerTile, List<string> parameters, bool oneSwipeOnly = false) : base(playerTile, parameters, GapIndex, oneSwipeOnly) { }

        public override void SetVisibility(bool isVisible)
        {
            base.SetVisibility(isVisible);
            Tile.IsMovable = !isVisible;
        }
    }
}
