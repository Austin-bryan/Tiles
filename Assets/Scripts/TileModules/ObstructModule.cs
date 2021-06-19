using System.Collections.Generic;
using static Tiles.Modules.ModuleConstants;

namespace Tiles.Modules
{
    public class ObstructionModule : TileModule
    {
        public override TileType TileType   => TileType.Nail;
        protected virtual MoveType moveType => MoveType.Swipe;

        public ObstructionModule(PlayerTile playerTile, List<string> parameters, int index, bool oneSwipeOnly = false) : base(playerTile, parameters, index, oneSwipeOnly) { }
        public override void SetVisibility(bool isVisible)
        {
            base.SetVisibility(isVisible);

            if (isVisible) Tile.ObstructionStates.ObstructMove(moveType);
            else Tile.ObstructionStates.UnobstructMove(moveType);
        }
    }
}
