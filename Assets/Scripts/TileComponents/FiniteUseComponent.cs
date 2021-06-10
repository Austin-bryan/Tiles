using System;
using UnityEngine.UI;
using Tiles.Factories;
using static TileType;
using ExtensionMethods;
using System.Collections.Generic;

namespace Tiles.Components
{
    public class FiniteUseComponent : TileComponent
    {
        // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Properties      ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
        public int UseCount { get; }
        public override List<string> Parameters
        {
            get => new List<string>() { UsesRemaining.ToString() };
            protected set => base.Parameters = value;
        }

        private int _usesRemaining;
        protected int UsesRemaining
        {
            get => _usesRemaining;
            set
            {
                _usesRemaining = value;
                if (text != null) text.text = value.ToString();
            }
        }

        public override TileType TileType       => Iron;
        public virtual TileType DeactivatedType => Nail;
        protected virtual int TextIndex         => ironTextIndex;

        // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Fields    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
        private const int ironTextIndex = 2;
        private Text text;
        
        // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Methods    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
        public FiniteUseComponent(PlayerTile playerTile, List<string> parameters, int index, bool oneSwipeOnly = false) : base(playerTile, parameters, index, oneSwipeOnly) => UseCount = UsesRemaining = parameters[0].Parse();
        public override void SetVisibility(bool isVisible)
        {
            if (text == null) UpdateText();

            base.SetVisibility(isVisible);
            text.gameObject.SetActive(isVisible && UsesRemaining > 0);

            if (isVisible) text.text = UsesRemaining.ToString();
        }
        public override void UpdateTile(PlayerTile newTile)
        {
            base.UpdateTile(newTile);
            UpdateText();
        }

        protected virtual bool Use(bool? wasPlayerTriggered)
        {
            if (wasPlayerTriggered == null) return false;
            if (!ShuffleManager.IsShuffling)
                UsesRemaining--;

            if (UsesRemaining > 0) return true;
            else if (UsesRemaining == 0) Deactivate(); 

            return false;
        }
        
        private void Deactivate()
        {
            text.gameObject.SetActive(false);

            var factory = TileFactory.GetFactory(DeactivatedType);
            Tile.AddComponent(DeactivatedType, factory.GetComponent(Tile, new List<string>()));
            Tile.AddComponents();

            Tile.PlayIron();  // Generalize
        }
        private void UpdateText() => text = Tile.GetChild(0, TextIndex).GetComponent<Text>();
    }
}