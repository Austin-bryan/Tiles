using System.Collections.Generic;
using static Tiles.Components.ComponentConstants;

namespace Tiles.Components
{
    public class ObstructComponent : TileComponent
    {
        public override TileType TileType   => TileType.Nail;
        protected virtual MoveType moveType => MoveType.Swipe;

        public ObstructComponent(PlayerTile playerTile, List<string> parameters, int index, bool oneSwipeOnly = false) : base(playerTile, parameters, index, oneSwipeOnly) { }
        public override void SetVisibility(bool isVisible)
        {
            base.SetVisibility(isVisible);

            if (isVisible) Tile.ObstructionStates.ObstructMove(moveType);
            else Tile.ObstructionStates.UnobstructMove(moveType);
        }
    }
}
