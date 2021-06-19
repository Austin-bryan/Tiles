using Tiles;

public enum SwipeStyle { Cardinal, Ordinal, EightWay }

public partial class PlayerTile
{
    public enum ObstructionState { Idle, NeedsToSwipeBack, IsSwipingBack }
}