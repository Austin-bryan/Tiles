using UnityEngine;
using UnityEngine.UI;
using Tiles.Components;
using ExtensionMethods;
using System.Collections.Generic;
using static TileType;
using static PathDebugger;
using static BoardManager;
using static ExpandedFlowControl.LambdaExt;
using Directions = System.Collections.Generic.List<Direction>;
using UnityEngine.UIElements;

//todo: Refactor functions into Local Functions
public partial class PlayerTile : Tile, IPlayerTile
{
    // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Properties      ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
    public bool IsMovable           { get; set; }
    public SwipeStyle SwipeStyle    { get; private set; }
    public Directions Directions    { get; private set; }
    public TileSideHandler Sides    { get => componentHandler.Sides; set => componentHandler.Sides = value; }
    public override TileColor Color { get => Sides.Color; set => (Sides.Color, this.Get<SpriteRenderer>().color) = (value, value.ToColor()); }
    public ObstructionStates ObstructionStates { get; private set; } = new ObstructionStates();

    // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Fields    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
    public SwipeMode SwipingMode;
    public RotateMode RotatingMode;

    private static int  count = 1;
    private static TileAudio tileAudio;
    private const bool coordVisible = true, idVisible = true;

    private bool isBeingSwiped, shouldRevertMode, isNewTile;
    private SwipeStyle originalMode, nextMode;
    private Vector3    targetLoc;
    private TileSpeed  tileSpeed;
    private ComponentHandler componentHandler;
    private SwipeStyle? queuedStyle;
    private bool isBeingRotated;

    // rotation //
    private float rotatedSpeed = 2f;
    [SerializeField] float rotateRadius = 172.75f;
    private float[] rotationRadi = new float[] { 297.3f, 209.2f, 172.75f, 144f, 123.3f, 107.7f, 95.7f };
    private Vector2 center;
    private float angle;

    // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Methods    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
    // --------  Public Methods  -------- //
    public static TargetTile GetTarget(Coord coord, bool shouldSubtract = true) => (TargetTile)TBoard[coord - (shouldSubtract ? 1 : 0)];

    public override void BeginPlay()
    {
        L(ShowID, showCoord, showID, () => UpdateID(), moveToSpot);
        tag = "PlayerTile";

        RotatingMode = new RotateMode(this, 4);
        SwipeMode.AddToOneSwipeList(this);

        // ---- Local Functions ---- //
        void showID()     => showChild(0, idVisible);
        void showCoord()  => showChild(1, coordVisible);
        void moveToSpot() => this.SetPosition(GetLocation(Coord));
        void showChild(int index, bool visible) => this.GetChild(0, index).SetActive(visible);
    }
    public void Awake()
    {
        (tileAudio, tileSpeed, Directions, componentHandler) = (new TileAudio(), new TileSpeed(), new List<Direction>(), new ComponentHandler(this));
        SetSwipeMode(SwipeStyle.Layer, true);

        IsMovable = true;
    }

    float angOffset;
    float startAngle;
    Vector3 targLocation;

    public void Update()
    {
        // factor out if statements
        if (isBeingSwiped)
        {
            this.SetPosition(Vector3.Lerp(this.Position(), targetLoc, tileSpeed.CurrentSpeed));

            if (targetLoc.ApproximatelyEqual(this.Position(), 2))
                SwipingMode.FinishSwipe();
        }
        if (isBeingRotated)
        {
            angle += rotatedSpeed * Time.deltaTime;

            var coordDiff = Coord - RotatingMode.rotatingTile.Coord;
            if (startAngle == 0)
            {
                float angleDiff = 3;

                     if (coordDiff.Y == -1)  angleDiff = 0;
                else if (coordDiff.X == -1)  angleDiff = 1.5f;
                else if (coordDiff.Y == 1) angleDiff = 3;
                else if (coordDiff.X == 1) angleDiff = 4.5f;

                var c= new Coord(coordDiff.X == 0 ? coordDiff.Y : coordDiff.X, coordDiff.Y == 0 ? -coordDiff.X : coordDiff.Y);
                var newCoord = Coord - c;

                targLocation = PBoard[newCoord.X, newCoord.Y].Position();
                Pair(Coord, coordDiff, c, newCoord).Log();
                newCoord.Log();

                startAngle = angle -= angleDiff;
            }

            var m = new Vector2(angle.Sin(), angle.Cos());
            var offset = m * rotationRadi[Board.LargestSize - 3];

            //if (ID == 2)
            //{
            //    (this.Position(), PBoard.TileTransforms[2, 1].Location).Log();
            //}
            Pair(this.Position(), targLocation).Log();

            var diff = this.Position() - targLocation;
            const int dd = 2;
            if ((diff.x.Abs() < dd) && diff.y.Abs() < dd) 
            {
                Path2();
                (isBeingRotated, startAngle) = (false, 0);
                this.SetPosition(targLocation);
            }

            //if (angle >= startAngle + 1.5f) (isBeingRotated, startAngle) = (false, 0);

            /// Save this for swap mode, this function swaps tiles on opposite sides of the tile with the swap component
            //var offset = (new Vector2(angle.Sin(), angle.Cos()) * coordDifference) * rotationRadi[Board.LargestSize - 3];

            this.SetPosition(center + offset);
        }
    }
    public void SetRotationCenter(Vector2 position) => center = position;

    // ----  Public Virtual Methods  ---- //
    public virtual void Start()
    {
        if (ID == 0) this.Destroy();
        this.SetPosition(GetLocation(Coord));
        center = this.Position();
    }
    public virtual void OnMouseDown() => SwipeManager.ActiveTile = this;
    public virtual void AddComponent(TileType type, TileComponent component)      => componentHandler.AddComponent(type, component);
    public virtual void AddComponents(List<(TileType, TileComponent)> components) => componentHandler.AddComponents(components);

    // ----  Public Expression Methods  ---- //
    public void PlayIron()      => tileAudio.PlayIron();
    public void MatchTarget()   => GetTarget(Coord)?.MatchColor(this);
    public void AddComponents() => componentHandler.AddComponents();
    public void FinishSwipe(bool isNewTile) => SwipingMode.FinishSwipe();

    public void SetSwipeModeForOneSwipe(SwipeStyle style)             => SetSwipeMode(style, true, true);
    public void ShowSprite(bool isVisible, params int[] indexes)      => this.GetChild(indexes).SetActive(isVisible);
    public void BeginSwipe(Direction direction, bool playerTriggered) => SwipingMode.BeginSwipe(direction, playerTriggered);
    public override void OnCoordChange(Coord oldCord, Coord newCoord) => Sides.CurrentSideComponents.ForEach(c => c.OnCoordChange());

    // ----  Public Block Methods  ---- //
    public void UpdateSolved()
    {
        if (!Coord.IsInFrame()) return;

        bool isSolved = Color == TBoard[Coord - new Coord(1, 1)].Color;

        if (isSolved)
        foreach (var componentSolved in GetSolveStatesFromComponents())
            isSolved = isSolved && componentSolved;

        SolveManager.Instance[Coord] = isSolved;

        // ---- Local Functions ---- //
        IEnumerable<bool> GetSolveStatesFromComponents()
        {
            foreach (var component in Sides.CurrentSideComponents)
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
        if (style == SwipeStyle.Layer)         return ExclusiveHybrid() || !ExclusiveDiagonal();
        else if (style == SwipeStyle.Diagonal) return ExclusiveHybrid() ||  ExclusiveDiagonal();
        else if (style == SwipeStyle.Hybrid)   return ExclusiveHybrid();

        return false;

        // ---- Local Functions ---- //
        bool ExclusiveHybrid()   => Sides.HasComponent(Hybrid)   || Sides.HasComponent(King) || Sides.HasComponent(Queen);
        bool ExclusiveDiagonal() => Sides.HasComponent(Diagonal) || Sides.HasComponent(Bishop);
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
    public void SetSwipeMode(SwipeStyle style, bool cacheMode, bool temporary = false)
    {
        var originalStyle = SwipeStyle;

        if (cacheMode) originalMode = style;
        (SwipeStyle, SwipingMode) = (style, SwipeMode.GetSwipeMode(style, this));

        if (temporary) (SwipingMode.Temporary, SwipingMode.ModeToRevert) = (true, originalStyle);
    }
    public void QueueSwipeMode(SwipeStyle style) => queuedStyle = style;
    public void CreateAndAddComponent(TileType type)
    {
        var factory = Tiles.Factories.TileFactory.GetFactory(type);

        if (factory == null || Sides.HasComponent(type)) return;
        var component = factory.GetComponent(this, null, false);

        AddComponent(type, component);
        AddComponents();
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