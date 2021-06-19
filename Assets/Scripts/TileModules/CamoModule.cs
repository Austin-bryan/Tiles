using ExtensionMethods;
using System.Collections.Generic;
using static Tiles.Modules.ModuleConstants;

namespace Tiles.Modules
{
    public class CamoModule : TileModule
    {
        public override TileType TileType => TileType.Camo;

        public CamoModule(PlayerTile player, List<string> parameters, bool oneSwipeOnly = false) : base(player, parameters, CamoIndex, oneSwipeOnly) { }

        public override void FinishSwipe(bool isNewTile) => MatchColor();
        public override void SetVisibility(bool isVisible)
        {
            base.SetVisibility(isVisible);
            if (isVisible) MatchColor();
        }
        private void MatchColor()
        {
            if (PlayerTile.GetTarget(Tile.Coord) != null) 
                Tile.Color = PlayerTile.GetTarget(Tile.Coord).Color;
        }
    }
}