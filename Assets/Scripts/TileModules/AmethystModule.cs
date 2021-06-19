using System.Collections.Generic;
using static Tiles.Modules.ModuleConstants;

namespace Tiles.Modules
{
    public class AmethystModule : FiniteUseModule
    {
        protected override int TextIndex  => 4;
        public override TileType TileType => TileType.Amethyst;
        public override TileType DeactivatedType => TileType.Bolt;

        public AmethystModule(PlayerTile playerTile, List<string> parameters, int moveCount, bool oneSwipeOnly = false) : base(playerTile, parameters, AmethystIndex, oneSwipeOnly) { }
        public override void OnWasWarped(bool wasPlayerTriggered)
        {
            base.OnWasWarped(wasPlayerTriggered);
            Use(true);
        }
    }
}