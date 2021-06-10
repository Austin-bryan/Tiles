using System;
using static PathDebugger;
using System.Collections.Generic;
using static Tiles.Components.ComponentConstants;

namespace Tiles.Components
{
    public class WarpComponent : TileComponent
    {
        public override TileType TileType => TileType.Warp;
        public override List<string> Parameters { get => new List<string>() { warpValue.ToString() }; protected set => base.Parameters = value; }

        private static Dictionary<int, List<PlayerTile>> warpTiles = new Dictionary<int, List<PlayerTile>>();
        private readonly int warpValue;

        // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Methods    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
        public WarpComponent(PlayerTile playerTile, List<string> parameters, int warpValue, bool oneSwipeOnly = false) : base(playerTile, parameters, new[] { WarpIndex, warpValue - 1}, oneSwipeOnly)
        {
            this.warpValue = warpValue;
            AddTileToWarpList(warpValue);
        }
        public override void UpdateTile(PlayerTile newTile)
        {
            warpTiles[warpValue][warpTiles[warpValue].IndexOf(Tile)] = newTile;
            
            base.UpdateTile(newTile);
        }
        public override void Remove()
        {
            base.Remove();
            warpTiles[warpValue].Remove(Tile);
        }

        public override void Activate(bool wasPlayerActivated)
        {
            if (WasActivatedThisRound) return;

            foreach (var tile in warpTiles[warpValue])
            {
                if (tile.IsWarpObstructed() || !tile.Sides.HasComponent(TileType.Warp)) return;
                tile.Sides.CurrentSideComponents.ForEach(c => c.OnWasWarped(wasPlayerActivated));
                if (tile == Tile) continue;

                var coordA = Tile.Coord;
                Tile.TeleportTo(tile.Coord);
                tile.TeleportTo(coordA);
            }

            base.Activate(wasPlayerActivated);
        }
        private void AddTileToWarpList(int warpValue)
        {
            if (!warpTiles.ContainsKey(warpValue))
                warpTiles.Add(warpValue, new List<PlayerTile>());
            if (!warpTiles[warpValue].Contains(Tile))
                warpTiles[warpValue].Add(Tile);
        }
    }
}