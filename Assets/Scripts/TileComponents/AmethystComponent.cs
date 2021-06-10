using System.Collections.Generic;
using static Tiles.Components.ComponentConstants;

namespace Tiles.Components
{
    public class AmethystComponent : FiniteUseComponent
    {
        protected override int TextIndex  => 4;
        public override TileType TileType => TileType.Amethyst;
        public override TileType DeactivatedType => TileType.Bolt;

        public AmethystComponent(PlayerTile playerTile, List<string> parameters, int moveCount, bool oneSwipeOnly = false) : base(playerTile, parameters, AmethystIndex, oneSwipeOnly) { }
        public override void OnWasWarped(bool wasPlayerTriggered)
        {
            base.OnWasWarped(wasPlayerTriggered);
            Use(true);
        }
    }
}