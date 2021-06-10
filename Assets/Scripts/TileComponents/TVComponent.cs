using System.Collections.Generic;

namespace Tiles.Components
{
    public class TVComponent : LightbulbComponent
    {
        public override TileType TileType => TileType.TV;
        public TVComponent(PlayerTile playerTile, List<string> parameters, int index, bool oneSwipeOnly = false) : base(playerTile, parameters, index, oneSwipeOnly) { }

        public override bool IsSolved() => IsPluggedIn;
    }
}
