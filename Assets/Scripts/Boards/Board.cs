using UnityEngine;
using ExtensionMethods;
using System.Collections.Generic;
using static PathDebugger;
using static LevelManager;

public class Board : MonoBehaviour, IBoard
{
    #region < Fields >

    public static int       X  => Size.X;
    public static int       Y  => Size.Y;
    public static int MarginX  => Size.X + 1;
    public static int MarginY  => Size.Y + 1;
    public static Coord Margin => Size + new Coord(1, 1);
    public static Coord Size { get; set; }

    // ---- Size Getters ---- //
    public static int SmallestSize => Size.X > Size.Y ? Size.Y : Size.X;
    public static int LargestSize  => Size.X > Size.Y ? Size.X : Size.Y;

    public static int SizeByLayerType(LayerType type) => type == LayerType.Row ? Size.X : Size.Y;

    public GridSlot[] GridSlots { get; set; }

    // --------  Public Fields  -------- //
    public static float[] scales = new float[9] { 4.5f, 2f, 1.35f, 0.95f, 0.785f, 0.655f, 0.56f, 0.49f, 0.435f };
    public const int TileLength = 220;

    public Transform[,] TileTransforms { get; private set; }
    public GameObject TileSpawnClass { get; private set; }
    public Tile[,] Tiles { get; protected set; }

    // --------  Protected Fields -------- //
    protected float scaleFactor = 1;
    protected bool hasMargins   = true, isTargetBoard;
    protected Coord currentCoord;

    // --------  Protected Fields -------- //
    private const  float tileScale    = 38;
    private static float CurrentScale => scales[LargestSize - 1];
    private static int firstColOffset = getOffset(Size.X);
    private static int firstRowOffset = getOffset(Size.Y);
    protected const int startLevel = 15;

    private int playerIndex = 0;
    private List<PlayerTile> playerTiles;

    #endregion < Fields >

    // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Methods    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
    public Tile this[Coord coord]
    {
        get => CoordInRange(coord) ? Tiles[coord.X, coord.Y] : null;
        set
        {
            if (CoordInRange(coord))
                Tiles[coord.X, coord.Y] = value;
        }
    }
    public Tile this[int x, int y] => Tiles[x, y];

    public void Awake() => playerTiles = new List<PlayerTile>();
    public void ScaleBoard() => this.SetLocalScale((CurrentScale * scaleFactor).ToVector3());
    public void Restart()
    {
        foreach (var t in Tiles)
            if (t != null)
                t.Destroy();

        this.SetLocalScale(new Vector3(1, 1, 1));
        GridSlots      = default;
        currentCoord   = default;
        Tile.LastColor = 1;
        Tile.PrevColor = default;
        Begin();
        MaskManager.Restart();
    }
    public virtual void Begin()
    {
        playerTiles    = new List<PlayerTile>();
        Size           = ParseManager.BoardSize;
        TileTransforms = new Transform[Size.X, Size.Y];
        Tiles          = new Tile[Size.X + marginBuff(), Size.Y + marginBuff()];

        CreateBoard();
        ScaleBoard();

        int marginBuff() => hasMargins ? 2 : 0;
    }
    public virtual void CreateBoard()
    {
        int endX  = Size.X;
        int endY  = Size.Y;

        if (!isTargetBoard) playerTiles = ParseManager.ParseTiles(Levels[CurrentLevel]);

        firstColOffset = getOffset(Size.X);
        firstRowOffset = getOffset(Size.Y);

        for (int i = 1; i <= endY; i++)
        for (int j = 1; j <= endX; j++)
        {
            currentCoord = new Coord(j, i);
            SpawnEmptySlot();
        }
    }

    private bool CoordInRange(in Coord coord) => coord.InRange(0, Tiles.GetLength(0) - 1, Tiles.GetLength(1) - 1);

    // ---- Get Transform ---- //
    public  Transform GetSlotTransform(Coord coord)                      => GetSlotTransform(coord, transform.ToTransform());
    public  GridSlot  GetSlotByCoord  (Coord coord)                      => GridSlots?[coord.GetPlayerIndex()];
    public  GridSlot  GetSlotByCoord  (Coord coord, out GridSlot slot)   => slot = GetSlotByCoord(coord);
    public  Transform GetSlotTransform(Coord coord, bool shouldAddScale) => GetSlotTransform(coord) * (shouldAddScale ? CurrentScale : 1);
    private Transform GetSlotTransform(Coord coord, Transform defaultTrans)
    {
        var trans = new Transform();

        trans = GetY(defaultTrans, coord);
        trans = GetX(trans, coord);
        trans.Scale = new Vector3(tileScale, tileScale);

        return trans;
    }
    private Transform GetSlotTransform() => GetSlotTransform(currentCoord, transform.ToTransform());

    // ---- Spawning ---- //
    private void SpawnEmptySlot() => SpawnSlot(TileSpawnClass);
    private void SpawnSlot(GameObject tileClass)
    {
        GameObject v;

        if (isTargetBoard) v = Tile.CreateTile<TargetTile>();
        else if (playerIndex < playerTiles.Count)
        {
            if (playerTiles[playerIndex] == null) return;
            v = playerTiles[playerIndex]?.gameObject;
        }
        else return;

        if (v == null && !isTargetBoard)
        {
            playerIndex++;
            return;
        }
        GridSlot slot = v.GetComponent<GridSlot>();
        slot.SetCoord(currentCoord);

        if (slot is TargetTile targetTile) targetTile.D = currentCoord;
        else if (slot is PlayerTile playerTile)
        {
            playerTile.MatchTarget();
            playerIndex++;
        }

        slot.SetParent(this);
        slot.BeginPlay();
        slot.SetTransform(GetSlotTransform(currentCoord));
        TileTransforms[currentCoord.X - 1, currentCoord.Y - 1] = slot.GetTransform();
        slot.SetTransform(TileTransforms[currentCoord.X - 1, currentCoord.Y - 1]);

        Tiles[currentCoord.X + marginBuff(), currentCoord.Y + marginBuff()] = (Tile)slot;

        // ---- Local Functions ---- //
        int marginBuff() => hasMargins ? 0 : -1;
    }

    // ---- Get Coord Components ---- //
    private Transform GetX(Transform transform)              => GetX(transform, currentCoord);
    private Transform GetY(Transform transform)              => GetY(transform, currentCoord);
    private Transform GetX(Transform transform, Coord coord) => transform.SubLocX(getLayerOffset(coord.X, useCompression: false) + firstColOffset);
    private Transform GetY(Transform transform, Coord coord) => transform.SubLocY(getLayerOffset(coord.Y, useCompression: true)  + firstRowOffset * (coord.Y - 2 + coord.Y - 1));
    
    // ---- Get Offsets ---- //
    protected static int getOffset(int boardSize) => (int)(TileLength * ((boardSize - 1) / 2f));
    protected static int getLayerOffset(int layer, bool useCompression) => (-TileLength - (useCompression ? (Size.Y - 3) * TileLength: 0)) * (layer - 1);
}