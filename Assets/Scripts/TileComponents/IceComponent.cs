using ExtensionMethods;
using System.Collections.Generic;
using static Tiles.Components.ComponentConstants;

namespace Tiles.Components
{
    public class IceComponent : TileComponent
    {
        public override TileType TileType => TileType.Ice;
        public IceComponent(PlayerTile playerTile, List<string> parameters, bool oneSwipeOnly = false) : base(playerTile, parameters, IceIndex, oneSwipeOnly) { }

        public override void Swipe(Direction direction, bool? wasPlayerTriggered, bool shouldSpawnOpposite, bool wasObstructed)
        {
            if (Tile.IsSwipeObstructed() || !TrySwipe()) return;
            SwipeManager.AddDirection(Tile, direction); 
        }
    }
}
