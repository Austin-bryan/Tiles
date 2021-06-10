using ExtensionMethods;
using static PathDebugger;
using System.Collections.Generic;

namespace Tiles.Components
{
    public abstract class OutletUser : TileComponent
    {
        public override TileType TileType => TileType.Normal;
        protected bool IsPluggedIn { get; set; }

        public OutletUser(PlayerTile playerTile, List<string> parameters, int index, bool oneSwipeOnly = false) : base(playerTile, parameters, new[] { index, 1 }, oneSwipeOnly) => 
            Tile.Delay(0.05f, UpdatePlugInStatus);

        // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Methods    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
        public override void ValidateSwipe(Direction direction, bool wasPlayerTrigered) => UpdatePlugInStatus();
        public override void OnCoordChange()     => UpdatePlugInStatus();
        public override void GlobalSwipeFinish() => UpdatePlugInStatus();

        protected virtual void PlugIn() => IsPluggedIn = true;
        protected virtual void Unplug()  { IsPluggedIn = false; Tile.UpdateSolved(); }
        protected virtual void TurnOn()  { }
        protected virtual void TurnOff() { }

        protected void UpdatePlugInStatus()
        {
            if (NearOutlet()) PlugIn();
            else Unplug();

            // ---- Local Functions ---- //
            bool NearOutlet()
            {
                for (int i = 1; i < 9; i++)
                    if (IsOutlet(i)) return true;
                return false;
            }
            bool IsOutlet(int dir) => BoardManager.PBoard[Tile.Coord + (Direction)dir]?.To<PlayerTile>().Sides.HasComponent(TileType.Outlet) ?? false;
        }
    }
}
