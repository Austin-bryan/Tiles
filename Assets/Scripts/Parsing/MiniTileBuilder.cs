using System.Linq;
using UnityEngine;
using Tiles.Factories;
using ExtensionMethods;
using Tiles.Components;
using System.Collections.Generic;
using static PathDebugger;
using static BoardManager;
using static UnityEngine.Object;

namespace Tiles.Parsing
{
    public class MiniTileBuilder
    {
        private static MiniRotateTileBuilder miniRotateBuilder = new MiniRotateTileBuilder();
        private readonly List<RotateComponent> components      = new List<RotateComponent>();
        private readonly List<int> tiles = new List<int>();

        public static MiniTileBuilder GetMiniTileBuilder(TileType type)
        {
            switch (type)
            {
                case TileType.Rotate: return miniRotateBuilder;
                default: return null;
            }
        }
        public static void CheckAll() => miniRotateBuilder.CheckForMatches();
        public void Add(PlayerTile tile, RotateComponent component)
        {
            tiles.Add(tile.ID);
            components.Add(component);
        }

        private void CheckForMatches()
        {
            foreach (var component in components)
            {
                var coord       = component.Tile.Coord;
                var otherTiles  = new List<PlayerTile>();
                var testCoords  = new Coord[component.rotateNumber - 1];
                var finalCoords = new Coord[component.rotateNumber - 1];

                if (testCoords.Count() < 1) break;

                for (int i = 0; i < component.rotateNumber - 1; i++)
                    otherTiles.Add(null);

                int coordIndex = 0;
                for (int j = 0; j < component.rotateNumber - (component.rotateNumber == 4 ? 1 : 0); j++)
                {
                    testCoords[coordIndex] = coord + new Coord(j == 0 ? 0 : 1, j.IsEven() ? 1 : 0);

                    if (AllInFrame())
                    {
                        bool shouldContinue = false;

                        for (int i = 0; i < component.rotateNumber - 1; i++)
                            otherTiles[i] = (PlayerTile)PBoard[testCoords[i]];

                        for (int i = 0; i < component.rotateNumber - 1; i++)
                            if (!otherTiles[i].Sides.HasComponent(TileType.Rotate) || otherTiles[i].Get(TileType.Rotate).To<RotateComponent>().rotateNumber != component.rotateNumber)
                            {
                                shouldContinue = true;
                                break;
                            }

                        if (shouldContinue) continue;

                        finalCoords = (new Coord[] { coord }).Concat(testCoords).ToArray();
                        CreateMini();

                        component.Tile.Delay(0.1f, Remove);
                        coordIndex--;
                    }
                    coordIndex++;
                }

                // ---- Local Functions ---- //
                void Remove()
                {
                    foreach (var removeCoord in finalCoords)
                    {
                        var tile = (PlayerTile)PBoard[removeCoord];
                        tile.Sides.RemoveComponent(component.TileType);
                    }
                }
                bool AllInFrame()
                {
                    foreach (var testCoord in testCoords)
                        if (!testCoord.IsInFrame()) return false;
                    return true;
                }
                void CreateMini()
                {
                    var miniTileObject  = (GameObject)Instantiate(Resources.Load("GameObjects/MiniTile"));
                    var miniTile        = miniTileObject.GetComponent<MiniTile>();
                    Vector3 positionSum = default;

                    otherTiles.Add(component.Tile);
                    foreach (var tile in otherTiles)
                    {
                        tile.transform.parent = null;
                        positionSum          += tile.transform.localPosition;
                        tile.transform.parent = PBoard.transform;
                    }
                    
                    miniTile.transform.parent = PBoard.transform;
                    miniTile.transform.localPosition = positionSum / component.rotateNumber;

                    var factory = TileFactory.GetFactory(TileType.Rotate);
                    var cv = factory.GetComponent(miniTile, new List<string>() { $"{component.rotateNumber}", "cw", "mini" }, false);

                    miniTile.AddComponent(TileType.Rotate, cv);
                    miniTile.AddComponents();
                    miniTile.Get(TileType.Rotate).To<RotateComponent>().SetUpConnectedTiles(finalCoords);
                }
            }
        }
    }
} //151