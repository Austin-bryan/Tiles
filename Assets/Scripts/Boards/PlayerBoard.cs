using UnityEngine;
using ExtensionMethods;
using System.Collections.Generic;
using static LayerType;
using static BoardManager;
using static PathDebugger;

public class PlayerBoard : Board
{
    public static Vector3 Scale => instance.Scale();
    private static PlayerBoard instance;

    public new void Awake()
    {
        base.Awake();
        PBoard = this;
    }
    public override void Begin()
    {
        base.Begin();
        instance = this;
    }
    public override void CreateBoard() => base.CreateBoard();

    public PlayerTile Get(Coord coord)                      => Tiles[coord.X, coord.Y] as PlayerTile;
    public PlayerTile Get(Coord coord, out PlayerTile tile) => tile = Get(coord);
    public List<PlayerTile> GetCardinalLayer   (Coord coord, bool isRow,    ref bool foundWall) => GetStrip(coord, !isRow,   MakeCoordDelta((0,  1), (0, -1), (1, 0), (-1,  0)), ref foundWall, out List<PlayerTile> foundTiles);
    public List<PlayerTile> GetOrdinalLayer(Coord coord, bool isSynced, ref bool foundWall) => GetStrip(coord, isSynced, MakeCoordDelta((1, -1), (-1, 1), (1, 1), (-1, -1)), ref foundWall, out List<PlayerTile> foundTiles);

    private List<PlayerTile> GetStrip(Coord coord, bool isStripA, Coord[,] coordDeltas, ref bool foundWall, out List<PlayerTile> foundTiles)
    {
        // Coord deltas is the change in the coord value. It depends on which striptype it is (row/col; synced diagonal/unsynched) and which direction along that strip
        foundTiles = new List<PlayerTile>();

        if (isStripA)
        {
            AddTilesToList(coord, coordDeltas[0, 0], ref foundWall, ref foundTiles);
            AddTilesToList(coord, coordDeltas[0, 1], ref foundWall, ref foundTiles);
        }                                           
        else                                        
        {                                           
            AddTilesToList(coord, coordDeltas[1, 0], ref foundWall, ref foundTiles);
            AddTilesToList(coord, coordDeltas[1, 1], ref foundWall, ref foundTiles);
        }

        return foundTiles;
    }
    private void AddTilesToList(Coord coord, Coord coordIncrement, ref bool foundWall, ref List<PlayerTile> foundTiles)
    {
        //$"{coord}, {coordIncrement}".Log();
        for (var workingCoord = coord;; workingCoord += coordIncrement)
        {
            if (!workingCoord.InBoard()) break;
            Get(workingCoord, out PlayerTile tile);

            if (tile?.Sides.HasModule(TileType.Wall) ?? false)
            {
                foundWall = true;
                break;
            }

            foundTiles.Add(tile);
        }
    }
    private Coord[,] MakeCoordDelta((int x, int y) deltaA1, (int x, int y) deltaA2, (int x, int y) deltaB1, (int x, int y) deltaB2) => new Coord[2, 2] { { deltaA1, deltaA2 }, { deltaB1, deltaB2 } };
    private static void GetCoord(int i, int index, LayerType type, out Coord coord) => coord = (type == Row ? new Coord(i, index) : new Coord(index, i));  // If type is row, returns (i, index), else if it's col, returns (index, i)
} // 155, 127, 82, 67