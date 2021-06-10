using UnityEngine;
using ExtensionMethods;
using System.Collections.Generic;
using static Tiles.Components.ComponentConstants;

namespace Tiles.Components
{
    public class HybridComponent : TileComponent
    {
        public override TileType TileType => TileType.Hybrid;
        private Vector3 originalScale;

        public HybridComponent(PlayerTile playerTile, List<string> parameters, bool oneSwipeOnly = false) : base(playerTile, parameters, DiagonalIndex, oneSwipeOnly) { }
        public override void SetVisibility(bool isVisible)
        {
            base.SetVisibility(isVisible);
            Tile.Get<SpriteRenderer>().sprite = TileSprites.Hybrid;
        }
    }
}
