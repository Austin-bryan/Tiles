using System.Collections.Generic;
using static Tiles.Modules.ModuleConstants;

namespace Tiles.Modules
{
    public class IronModule : FiniteUseModule
    {
        protected override int TextIndex  => 2;
        public override TileType TileType => TileType.Iron;
        public override TileType DeactivatedType => TileType.Nail;

        public IronModule(PlayerTile playerTile, List<string> parameters, int moveCount, bool oneSwipeOnly = false) : base(playerTile, parameters, IronIndex, oneSwipeOnly) { }
        public override void Swipe(Direction direction, bool? wasPlayerTriggered, bool shouldSpawnOpposite, bool wasObstructed) 
        {
            if (wasObstructed) return;
            if (Use(wasPlayerTriggered)) base.Swipe(direction, wasPlayerTriggered, shouldSpawnOpposite, wasObstructed);
        }
    }
}//80, 19 