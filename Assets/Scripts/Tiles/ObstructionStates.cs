using static MoveType;
using ExtensionMethods;
using System.Collections.Generic;

public class ObstructionStates
{
    private readonly List<MoveType> obstructedMoves = new List<MoveType>();

    public bool IsWarpObstructed   => obstructedMoves.Contains(Warp);
    public bool IsSwipeObstructed  => obstructedMoves.Contains(Swipe);
    public bool IsRotateObstructed => obstructedMoves.Contains(Rotate);

    public void ObstructMove(MoveType moveType)   => obstructedMoves.AddUnique(moveType);
    public void UnobstructMove(MoveType moveType) => obstructedMoves.Remove(moveType);
}
