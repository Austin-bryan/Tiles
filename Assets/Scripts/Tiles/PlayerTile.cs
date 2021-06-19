using UnityEngine;
using UnityEngine.UI;
using Tiles.Modules;
using ExtensionMethods;
using System.Collections.Generic;
using static TileType;
using static PathDebugger;
using static BoardManager;
using Directions = System.Collections.Generic.List<Direction>;

/// Player Tiles are the tiles that the player interacts with 
public partial class PlayerTile : Tile, IPlayerTile
{
    // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Properties      ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
    public bool IsMovable           { get; set; }
    public SwipeStyle SwipeStyle    { get; private set; }
    public Directions Directions    { get; private set; }
    public TileSideHandler Sides    { get => componentHandler.Sides; set => componentHandler.Sides = value; }
    public override TileColor Color { get => Sides.Color; set => (Sides.Color, this.Get<SpriteRenderer>().color) = (value, value.ToColor()); }
    public List<TileModule> Modules => Sides.CurrentSideModules;
    public ObstructionStates ObstructionStates { get; private set; } = new ObstructionStates();

    // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Fields    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
    private SwipeMode SwipingMode;

    private static int count = 1;
    private static TileAudio tileAudio;
    private const bool coordVisible = false, idVisible = false;     // Used only for debugging

    private bool isBeingSwiped, shouldRevertMode, isNewTile;
    private SwipeStyle originalMode;
    private Vector3    targetLoc;
    private TileSpeed  tileSpeed;
    private ModuleHandler componentHandler;
    private SwipeStyle? queuedStyle;

    // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Methods    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
    // --------  Public Methods  -------- //
    public static TargetTile GetTarget(Coord coord, bool shouldSubtract = true) => (TargetTile)TBoard[coord - (shouldSubtract ? 1 : 0)];

    // ---- Unity Methods ---- //
    public override void BeginPlay()
    {
        ShowID();
        showCoord();
        UpdateID();
        moveToSpot();

        name = $"PlayerTile {ID}";
        SwipeMode.AddToOneSwipeList(this);

        // ---- Local Functions ---- //
        void showCoord()  => showChild(1, coordVisible);
        void moveToSpot() => this.SetPosition(GetLocation(Coord));
        void showChild(int index, bool visible) => this.GetChild(0, index).SetActive(visible);
    }
    public void Awake()
    {
        (tileAudio, tileSpeed, Directions, componentHandler) = (new TileAudio(), new TileSpeed(), new List<Direction>(), new ModuleHandler(this));
        SetSwipeMode(SwipeStyle.Cardinal, true);

        IsMovable = true;
    }
    public void Update()
    {
        if (isBeingSwiped)
        {
            this.SetPosition(Vector3.Lerp(this.Position(), targetLoc, tileSpeed.CurrentSpeed));

            if (targetLoc.ApproximatelyEqual(this.Position(), 2))
                SwipingMode.FinishMovement();
        }
    }
    public virtual void Start()
    {
        if (ID == 0) this.Destroy();
        this.SetPosition(GetLocation(Coord));
    }
    public virtual void OnMouseDown() => SwipeManager.ActiveTile = this;

    // ----  Public Virtual Methods  ---- //
    public virtual void AddModule(TileType type, TileModule component)      => componentHandler.AddModule(type, component);
    public virtual void AddModules(List<(TileType, TileModule)> components) => componentHandler.AddModules(components);

    // ----  Public Expression Methods  ---- //
    public void PlayIron()    => tileAudio.PlayIron();
    public void MatchTarget() => GetTarget(Coord)?.MatchColor(this);
    public void AddModules()  => componentHandler.AddModules();
    public void FinishSwipe(bool isNewTile) => SwipingMode.FinishMovement();

    public void SetSwipeModeForOneSwipe(SwipeStyle style)               => SetSwipeMode(style, true, true);
    public void ShowSprite(bool isVisible, params int[] indexes)        => this.GetChild(indexes).SetActive(isVisible);
    public void IntiateSwipe(Direction direction, bool playerTriggered) => SwipingMode.InitiateTilesInMovementGroup(direction, playerTriggered);
    public override void OnCoordChange(Coord oldCord, Coord newCoord)   => Sides.CurrentSideModules.ForEach(c => c.OnCoordChange());

    // ----  Public Block Methods  ---- //
    public void UpdateSolved()
    {
        if (!Coord.IsInFrame()) return;

        bool isSolved = Color == TBoard[Coord - new Coord(1, 1)].Color;

        if (isSolved)
        foreach (var componentSolved in GetSolveStatesFromModules())
            isSolved = isSolved && componentSolved;

        SolveManager.Instance[Coord] = isSolved;

        // ---- Local Functions ---- //
        IEnumerable<bool> GetSolveStatesFromModules()
        {
            foreach (var component in Sides.CurrentSideModules)
                yield return component.IsSolved();
        }
    }
    public void AllMovesFinished() { }
    public void TeleportTo(Coord coord)
    {
        SetCoord(coord);
        transform.position = GetLocation(coord);
    }
    public bool CanSwipe(SwipeStyle style)
    {
             if (style == SwipeStyle.Ordinal)  return ExclusiveEightWay() ||  ExclusiveDiagonal();
        else if (style == SwipeStyle.Cardinal) return ExclusiveEightWay() || !ExclusiveDiagonal();
        else if (style == SwipeStyle.EightWay) return ExclusiveEightWay();

        return false;

        // ---- Local Functions ---- //
        bool ExclusiveEightWay() => Sides.HasModule(Hybrid)   || Sides.HasModule(King) || Sides.HasModule(Queen);
        bool ExclusiveDiagonal() => Sides.HasModule(Diagonal) || Sides.HasModule(Bishop);
    }
    public void Activate(bool wasPlayerTriggered)
    {
        componentHandler.Activate(wasPlayerTriggered);
        UpdateSolved();
    }
    public void SetDirections(List<Direction> directions)
    {
        Directions = directions;

        if (directions.Count == 4) return;
        directions.ForEach(d => this.GetChild(11, (int)d - 1).SetActive(true));
    }
    public void SetSwipeMode(SwipeStyle style, bool cacheMode, bool isTemporary = false)
    {
        var originalStyle = SwipeStyle;

        if (cacheMode) originalMode = style;
        (SwipeStyle, SwipingMode) = (style, SwipeMode.CreateSwipeMode(style, this));

        if (isTemporary) (SwipingMode.IsTemporary, SwipingMode.OriginalStyle) = (true, originalStyle);
    }
    public void QueueSwipeMode(SwipeStyle style) => queuedStyle = style;
    public void CreateAndAddModule(TileType type)
    {
        var factory = Tiles.Factories.TileFactory.GetFactory(type);

        if (factory == null || Sides.HasModule(type)) return;
        var component = factory.GetModule(this, null, false);

        AddModule(type, component);
        AddModules();
    }

    // --------  Private Methods  -------- //
    private void ShowID()                    => this.GetChild<Text>(0, 0).text = ID.ToString();
    private Vector3 GetLocation(Coord coord) => PBoard?.GetSlotTransform(coord, shouldAddScale: true).Location ?? new Vector3(-1000, -1000, -1000);

    private void UpdateTargetLocation()
    {
        targetLoc = GetLocation(SwipingMode.NewCoord);

        if (SwipingMode.ObstructionState == ObstructionState.NeedsToSwipeBack) 
            targetLoc = (this.Position() * 3 + targetLoc) / 4;
    }
    private void UpdateID(int id = -1)
    {
        ID = (id < 0) ? count++ : id;
        this.Get<Text>(0, 0).text = ID.ToString();
    }

    public void Swipe(Direction direction, bool? playerTriggered, bool shouldSpawnOpposite, bool wasObstructed) { }

}// 335, 314, 320, 370, 272, 320, 208, 236, 145