using ExtensionMethods;
using static PathDebugger;
using System.Collections.Generic;
using static Tiles.Modules.ModuleConstants;
using LinkedTiles = System.Collections.Generic.Dictionary<System.Int32, System.Collections.Generic.List<PlayerTile>>;

namespace Tiles.Modules
{
    public class LinkModule : TileModule
    {
        public override TileType TileType => TileType.Link;
        public override List<string> Parameters { get => links.ConvertAll(x => x.ToString()); protected set => base.Parameters = value; }

        private static readonly LinkedTiles linkedTiles  = new Dictionary<int, List<PlayerTile>>();
        private static readonly List<int> tilesActivated = new List<int>();
        private static readonly List<int> linksActivated = new List<int>();
        private readonly List<int> links = new List<int>();

        public LinkModule(PlayerTile playerTile, List<string> parameters, bool oneSwipeOnly = false) : base(playerTile, parameters, -1, oneSwipeOnly)
        {
            parameters.ForEach(p => links.Add(p.Parse()));

            links.ForEach(l =>
            {
                ShowSprite(l, IsVisible);
                AddTileToLinkList(l);
            });
        }

        // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Methods    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
        public static void Reset()
        {
            tilesActivated.Clear();
            linksActivated.Clear();
        }

        public override void Activate(bool wasPlayerActivated)
        {
            tilesActivated.Add(Tile.ID);
            base.Activate(wasPlayerActivated);

            foreach (var link in links)
            {
                linksActivated.Count.Log("count");

                if (linksActivated.Contains(link)) continue;
                linksActivated.Add(link);

                foreach (var t in linkedTiles[link])
                {
                    if (t == Tile || tilesActivated.Contains(t.ID)) continue;

                    t.Activate(false);
                    tilesActivated.Add(t.ID);
                }
            }
        }
        public override void SetVisibility(bool isVisible) => links.ForEach(l => ShowSprite(l, isVisible));
        public override void UpdateTile(PlayerTile newTile)
        {
            links.ForEach(link =>
            {
                if (linkedTiles[link].IndexOf(Tile) > -1)
                    linkedTiles[link][linkedTiles[link].IndexOf(Tile)] = newTile;
            });
            base.UpdateTile(newTile); 
        }
        public override void Remove()
        {
            base.Remove();

            foreach (var link in links)
                linkedTiles[link].Remove(Tile);
        }

        private void ShowSprite(int link, bool isVisible) => Tile.ShowSprite(isVisible, new[] { LinkIndex, link - 1 });
        private void AddTileToLinkList(int link)
        {
            if (!linkedTiles.ContainsKey(link))
                linkedTiles.Add(link, new List<PlayerTile>());
            if (!linkedTiles[link].Contains(Tile))
                linkedTiles[link].Add(Tile);
        }
    }
}