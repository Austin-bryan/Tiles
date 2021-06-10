using UnityEngine;
using ExtensionMethods;
using Tiles.Components;
using static Direction;
using static SwipeStyle;
using static PathDebugger;
using static SwipeManager;
using static BoardManager;

public class SwipeComponent : MonoBehaviour 		
{
    public static Direction SwipeDir { get; set; }
    public static bool IsSynced  => instance.IsSyncedDiagonal;
    public static bool IsSwiping => SwipeDir != None;
    private static SwipeComponent instance;

    private bool IsHorizontal       => xInBuffer  && !yInBuffer;
    private bool IsVertical         => !xInBuffer && yInBuffer;
    private bool IsSyncedDiagonal   => (xInBuffer && xIncreased) && (yInBuffer && yIncreased)  || (xInBuffer && !xIncreased) && (yInBuffer && !yIncreased);
    private bool IsUnsyncedDiagonal => (xInBuffer && xIncreased) && (yInBuffer && !yIncreased) || (xInBuffer && !xIncreased) && (yInBuffer &&  yIncreased);
    private bool IsDiagonal         => IsSyncedDiagonal || IsUnsyncedDiagonal;

    private bool xInBuffer  => swipeDelta.x.Abs() > buffer;
    private bool yInBuffer  => swipeDelta.y.Abs() > buffer;
    private bool xIncreased => swipeDelta.x > 0;
    private bool yIncreased => swipeDelta.y > 0;

    private const float buffer = 10;      // If the delta of the x or y value is smaller than this buffer it will not register
    private bool isHolding = false;
    private bool wasReleased;
    private float lastTime;
    private Vector3 startTouch, swipeDelta;

    private void Start() => instance = this;

    public void Update()
    {
        if (ActiveTile == null) return;

        if (SwipeDir != 0) SwipeDir = None;
        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonDown(0))
            TileComponent.ActivatedComponents.Clear();

        if (Input.GetMouseButtonUp(0))
        {
            GetSwipeDelta();

            if (isHolding && !IsValidSwipe())
            {
                LinkComponent.Reset();
                ActiveTile.Activate(true);
                ActiveTile = null;
                isHolding  = false;
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            isHolding  = true;
            startTouch = Input.mousePosition;
        }

        if (!isHolding) return;
        
        GetSwipeDelta();
        if (!IsValidSwipe()) return;

        isHolding = false;
        Direction swipeDir = default;

        if      (IsHorizontal)       swipeDir |= swipeDelta.x > 0 ? Right     : Left;
        else if (IsVertical)         swipeDir |= swipeDelta.y > 0 ? Up        : Down;
        else if (IsSyncedDiagonal)   swipeDir |= swipeDelta.x > 0 ? UpRight   : DownLeft;
        else if (IsUnsyncedDiagonal) swipeDir |= swipeDelta.x > 0 ? DownRight : UpLeft;

        if (swipeDir.Is(Diagonal))
        {
            if (SwipeDirectionContainsSwipeStyle(Diagonal) || ActiveTile.CanSwipe(Diagonal))
            {
                if (SwipeDirectionContainsSwipeStyle(Diagonal))
                    ActiveTile.SetSwipeMode(Diagonal, false, temporary: true);
                else if (!BalloonComponent.OverrideDiagonalMode) return;
            }
            else return;
        }

        SwipeDir = swipeDir;

        // ---- Local Functions ---- //
        bool SwipeDirectionContainsSwipeStyle(SwipeStyle style)
        {
            if (ActiveTile.CanSwipe(style)) return true;
            return search(true, style) || search(false, style);
        }
        bool search(bool shouldAdd, SwipeStyle style)
        {
            var coord = ActiveTile.Coord;

            while (coord.IsInFrame())
            {
                var s = swipeDir.ToCoord();
                coord += new Coord(s.X * (shouldAdd ? 1 : -1), s.Y * (shouldAdd ? 1 : -1));

                if (PBoard[coord] == null) return false;
                if (PBoard[coord].To<PlayerTile>().CanSwipe(style)) return true;
            }
            return false;
        }
    }

    private bool IsValidSwipe()  => swipeDelta.magnitude > 20;
    private void GetSwipeDelta() => swipeDelta = Input.mousePosition - startTouch;
}