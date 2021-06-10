using ExtensionMethods;
using static PathDebugger;
using System.Collections.Generic;
using static Tiles.Components.ComponentConstants;

namespace Tiles.Components
{
    public class GapComponent : TileComponent
    {
        public override TileType TileType => TileType.Gap;
        public GapComponent(PlayerTile playerTile, List<string> parameters, bool oneSwipeOnly = false) : base(playerTile, parameters, GapIndex, oneSwipeOnly) { }

        public override void SetVisibility(bool isVisible)
        {
            base.SetVisibility(isVisible);
            Tile.IsMovable = !isVisible;
        }
    }
}
