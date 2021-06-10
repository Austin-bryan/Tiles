using System.Collections.Generic;
using static Tiles.Components.ComponentConstants;

namespace Tiles.Components
{
    public class IronComponent : FiniteUseComponent
    {
        protected override int TextIndex  => 2;
        public override TileType TileType => TileType.Iron;
        public override TileType DeactivatedType => TileType.Nail;

        public IronComponent(PlayerTile playerTile, List<string> parameters, int moveCount, bool oneSwipeOnly = false) : base(playerTile, parameters, IronIndex, oneSwipeOnly) { }
        public override void Swipe(Direction direction, bool? wasPlayerTriggered, bool shouldSpawnOpposite, bool wasObstructed) 
        {
            if (wasObstructed) return;
            if (Use(wasPlayerTriggered)) base.Swipe(direction, wasPlayerTriggered, shouldSpawnOpposite, wasObstructed);
        }
    }
}//80, 19 