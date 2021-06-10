using static MoveType;
using ExtensionMethods;
using System.Collections.Generic;

namespace Tiles.Components
{
    public class LightbulbComponent : OutletUser
    {
        public override bool SubscribeToAllMoves => true;
        public override TileType TileType => TileType.Lightbulb;
        private int spriteIndex;

        public LightbulbComponent(PlayerTile playerTile, List<string> parameters, int index, bool oneSwipeOnly = false) : base(playerTile, parameters, index, oneSwipeOnly) => spriteIndex = index;

        // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Methods    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
        public override void ValidateSwipe(Direction direction, bool wasPlayerTrigered) => UpdatePlugInStatus();
        public override void OnCoordChange()     => UpdatePlugInStatus();
        public override void GlobalSwipeFinish() => UpdatePlugInStatus();

        protected override void PlugIn()  { base.PlugIn(); TurnOn();  }
        protected override void Unplug()  { base.Unplug(); TurnOff(); }
        protected override void TurnOn()  => ShowSprites(true);
        protected override void TurnOff() => ShowSprites(false);

        public override void SetVisibility(bool isVisible)
        {
            base.SetVisibility(isVisible);

            //if (isVisible) SwipeManager.AddToFinishSwipeList(this);
            //else SwipeManager.RemoveFinishSwipeList(this);
        }
        private void ShowSprites(bool turnedOn)
        {
            Tile.ShowSprite(true,  spriteIndex, turnedOn ? 1 : 0);    // turn on  new sprite
            Tile.ShowSprite(false, spriteIndex, turnedOn ? 0 : 1);    // turn off old sprite
            Tile.SetIsSwipeObstructed(!turnedOn);
        }
    }
}
