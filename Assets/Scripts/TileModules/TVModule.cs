using System.Collections.Generic;

namespace Tiles.Modules
{
    public class TVModule : LightbulbModule
    {
        public override TileType TileType => TileType.TV;
        public TVModule(PlayerTile playerTile, List<string> parameters, int index, bool oneSwipeOnly = false) : base(playerTile, parameters, index, oneSwipeOnly) { }

        public override bool IsSolved() => IsPluggedIn;
    }
}
