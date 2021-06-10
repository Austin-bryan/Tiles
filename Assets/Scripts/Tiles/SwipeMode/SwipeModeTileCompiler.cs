using static PathDebugger;
using System.Collections.Generic;

public partial class PlayerTile
{
    public abstract class SwipeModeTileCompiler : SwipeMode
    {
        private PlayerTile tile;
        public SwipeModeTileCompiler(PlayerTile tile) : base(tile) => this.tile = tile;

        protected abstract void AddRange(ref List<PlayerTile> tiles, Direction direction, PlayerTile tile, ref bool foundWall);
        protected override void GetLayerTiles(Direction direction, ref List<PlayerTile> layerTiles)
        {
            layerHasWall = getLayer(out List<PlayerTile> foundTiles, direction.ToLayerType());
            layerTiles.AddRange(foundTiles);
            layerTiles.Remove(tile);

            // ---- Local Functions ---- //
            bool getLayer(out List<PlayerTile> tiles, LayerType layerType)
            {
                bool foundWall = false;

                tiles = new List<PlayerTile>();
                AddRange(ref tiles, direction, tile, ref foundWall);

                return foundWall;
            }
        }
    }
}