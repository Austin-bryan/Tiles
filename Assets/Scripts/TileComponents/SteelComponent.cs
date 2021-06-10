using ExtensionMethods;
using System.Collections.Generic;
using static Tiles.Components.ComponentConstants;

namespace Tiles.Components
{
    public class SteelComponent : FiniteUseComponent
    {
        protected override int TextIndex  => 3;
        public override TileType TileType => TileType.Steel;
        public override TileType DeactivatedType => TileType.Screw;

        public SteelComponent(PlayerTile playerTile, List<string> parameters, int moveCount, bool oneSwipeOnly = false) : base(playerTile, parameters, SteelIndex, oneSwipeOnly) { }
        public override void OnWasRotated(bool wasPlayerTriggered)
        {
            7.Log();
            base.OnWasRotated(wasPlayerTriggered);
            Use(true);
        }
    }
}