using UnityEngine;
using ExtensionMethods;
using System.Collections.Generic;
using static Tiles.Modules.ModuleConstants;
using static PathDebugger;

namespace Tiles.Modules
{
    public class TabletModule : LightbulbModule
    {
        public override TileType TileType => TileType.Tablet;

        private SpriteRenderer batterySprite;
        private int battery = 5;
        private bool wasSwiped;

        // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Methods    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
        public TabletModule(PlayerTile playerTile, List<string> parameters, int index, bool oneSwipeOnly = false) : base(playerTile, parameters, index, oneSwipeOnly)
        {
            Tile.GetChild(TabletIndex, 2).SetActive(true);
            batterySprite = Tile.Get<SpriteRenderer>(TabletIndex, 2);
            UpdateBatterySprite(battery);
        }
        public override bool IsSolved()  => battery > 0;
        protected override void PlugIn() => IsPluggedIn = true;
        protected override void Unplug()  { IsPluggedIn = false; Tile.UpdateSolved(); }

        public override void Swipe(Direction direction, bool? wasPlayerTriggered, bool shouldSpawnOpposite, bool wasObstructed)
        {
            base.Swipe(direction, wasPlayerTriggered, shouldSpawnOpposite, wasObstructed);
            wasSwiped = true;
        }
        public override void GlobalSwipeFinish()
        {
            base.GlobalSwipeFinish();
            UpdateBattery();
        }
        public void UpdateBattery()
        {
            if (battery > 0 && !IsPluggedIn) UpdateBatterySprite(battery--);
            if (battery < 5 &&  IsPluggedIn) UpdateBatterySprite(battery++);

            if (battery > 0)  TurnOn();
            if (battery <= 0) TurnOff();

            Tile.UpdateSolved();
        }
        private void UpdateBatterySprite(int newBattery)
        {
            if (battery > 5) battery = 5;
            if (battery < 0) battery = 0;

            if (battery > 0)
            {
                batterySprite.sprite = TileSprites.GetBatterySprite(battery);
                batterySprite.enabled = true;
            }
            else batterySprite.enabled = false;
        }
    }
}
