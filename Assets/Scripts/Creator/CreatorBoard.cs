using System;
using System.Linq;
using UnityEngine;
using static Board;
using UnityEngine.UI;
using ExtensionMethods;
using System.Collections.Generic;
using static PathDebugger;
using static BoardManager;
using static TileSelector;
using static InequalityDelegate;
using static ExpandedFlowControl.LambdaExt;

public class CreatorBoard : MonoBehaviour, IBoard 
{
    // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Fields    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
    public GameObject Grid;
    public CreatorTile CreatorTile;

    private static CreatorBoard instance;
    public static CreatorBoard Instance => instance ?? (instance = new CreatorBoard());

    private int LargestSide => Size.X > Size.Y ? Size.X : Size.Y;
    private static float[] scales  = new float[9] { 4.5f, 2.25f, 1.475f, 1.11f, 0.875f, 0.725f, 0.625f, 0.545f, 0.485f};
    private static CreatorTile tile;
    public static CreatorTile[,] Tiles;

    // ----------------   Constructors    ---------------- //
    private CreatorBoard() {}

    // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Methods    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
    private void Awake() => Size = new Coord(3);
    private void Start()
    {
        CBoard = instance = this;
        scales = new float[9] { 4.5f, 2.25f, 1.475f, 1.11f, 0.875f, 0.725f, 0.625f, 0.545f, 0.485f };
        ResizeTilesArray();
        FillGrid();
        UpdateTiles();
    }
    public void Refresh() => L(ClearGrid, FillGrid, UpdateTiles);
    public void Restart() => L(ClearGrid, Start);
    public List<CreatorTile> ShiftSelectTiles(CreatorTile ignoreTile)
    {
        //todo: If multiple tiles are already selected, selecting one of them will deslect all except for the current one
        //todo: Deselecting from here won't play the deslect anim

        if (FirstSelectedTile is null || MostRecentlySelectedTile is null) return null;

        Coord firstCoord = FirstSelectedTile.Coord;
        Coord lastCoord  = MostRecentlySelectedTile.Coord;
        Coord minCoord, maxCoord;

        DeselectAll(ignoreTile);

        minCoord = new Coord(getIndex(false, firstCoord.X, lastCoord.X), getIndex(false, firstCoord.Y, lastCoord.Y));
        maxCoord = new Coord(getIndex(true,  firstCoord.X, lastCoord.X), getIndex(true,  firstCoord.Y, lastCoord.Y));

        for (int i = minCoord.X; i <= maxCoord.X; i++)
            for (int j = minCoord.Y; j <= maxCoord.Y; j++)
                if (!Tiles[i, j].RefEquals(ignoreTile))
                    Tiles[i, j].Select();

        return default;

        int getIndex(bool useLargest, int firstIndex, int lastIndex)
        {
            Inequal inequal = GreaterThan;
            if (!useLargest) inequal = LessThan;

            return inequal(firstIndex, lastIndex) ? firstIndex : lastIndex;
        }
    }
    public void ShowColors() /*=> ShowProperties((t, i) => t?.SetColor(GetTile(i).Color));*/  { }
    public void ShowType()   /*=> ShowProperties((t, i) => t?.SetType(GetTile(i).Type));*/    { }
    public void UpdateTiles() => L(ShowColors, ShowType);

    private void ShowProperties(Action<CreatorTile, int> action)
    {
        var tiles = Tiles.ToList().Where(x => x != null);

        for (int i = 0; i < Size.X * Size.Y; i++)
            action(tiles.ElementAt(i), i);
    }
    private static void ResizeTilesArray() => Tiles = new CreatorTile[Size.Y + 2, Size.X + 2];
    private static void ClearGrid()
    {
        foreach (var t in Tiles)
            if (t != null)
                t.Destroy();

        ResizeTilesArray();
    }
    private void FillGrid()
    {
        float scale = scales[LargestSide - 1];

        Grid.Get<GridLayoutGroup>().constraint      = GridLayoutGroup.Constraint.FixedColumnCount;
        Grid.Get<GridLayoutGroup>().constraintCount = Size.X;

        for (int i = 1; i <= Size.Y; i++)
            for (int j = 1; j <= Size.X; j++)
            {
                tile = (Instantiate(Resources.Load("CreatorTile") as GameObject)).Get<CreatorTile>();
                tile.transform.SetParent(Grid.transform);
                tile.SetLocalScale(1);
                tile.Coord = new Coord(i, j);

                tile.CoordText.text = tile.Coord.ToString();
                Tiles[i, j] = tile;
            }

        this.SetLocalScale((scale * 1.4f).ToVector3());
    }
}
