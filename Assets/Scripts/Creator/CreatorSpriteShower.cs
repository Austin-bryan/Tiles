﻿using UnityEngine;
using Tiles.Factories;
using static TileType;
using ExtensionMethods;
using System.Collections.Generic;
using static Tiles.Components.ComponentConstants;

namespace Tiles.Creator
{
    public static class CreatorSpriteShower
    {
        public static void ShowSprite(CreatorTile tile, TileType property, bool visibility)
        {
            switch (property)
            {
                case Brick:    ShowSprite(BrickIndex);    break;
                case Camo:     ShowSprite(CamoIndex);     break;
                case Gap:      ShowSprite(GapIndex);      break;
                case Wall:     ShowSprite(WallIndex);     break;
                case Iron:     ShowSprite(IronIndex);     break;
                case Balloon:  ShowSprite(BalloonIndex);  break;
                case Ice:      ShowSprite(IceIndex);      break;
                case Nail:     ShowSprite(NailsIndex);    break;
                case Screw:    ShowSprite(ScrewsIndex);   break;
                case Steel:    ShowSprite(SteelIndex);    break;
                case Bolt:     ShowSprite(BoltIndex);     break;
                case Amethyst: ShowSprite(AmethystIndex); break;
                case Diagonal: ShowSprite(DiagonalIndex); break; 
            }
            void ShowSprite(int index)
            {
                if (visibility)
                {
                    var factory = TileFactory.GetFactory(property);
                    tile.AddComponent(property, factory.GetComponent(tile, ParameterGetter.GetParameters(property)));
                }
                else tile.RemoveComponent(property);
            }
        }
    }
}
