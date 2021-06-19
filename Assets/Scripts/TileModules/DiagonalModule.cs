using UnityEngine;
using ExtensionMethods;
using System.Collections.Generic;
using static Tiles.Modules.ModuleConstants;

namespace Tiles.Modules
{
    public class DiagonalModule : TileModule
    {
        public override TileType TileType => TileType.Diagonal;
        private Vector3 originalScale;

        public DiagonalModule(PlayerTile playerTile, List<string> parameters, bool oneSwipeOnly = false) : base(playerTile, parameters, DiagonalIndex, oneSwipeOnly)
        {
            playerTile.Delay(0.01f, Shape);
            AddToAllMovesFinished(true);

            // ---- Local Functions ---- //
            void Shape()
            {
                originalScale = playerTile.transform.localScale;
                SetVisibility(true);
            }
        }
        public override void AllMovesFinished()
        {
            base.AllMovesFinished();
            Tile.SetIsSwipeObstructed(false);
        }
        public override void SetVisibility(bool isVisible)
        {
            if (originalScale == Vector3.zero) return;

            base.SetVisibility(isVisible);
            Tile.SetRotation(isVisible  ? new Vector3(0, 0, 45)  : Vector3.zero);
            Tile.SetSwipeMode(isVisible ? SwipeStyle.Ordinal    : SwipeStyle.Cardinal, true);
            Tile.SetLocalScale(originalScale * (isVisible ? 0.8f : 1));
        }
    }
}
