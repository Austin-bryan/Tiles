using UnityEngine;

public class TargetBoard : Board
{
    public void Awake() => BoardManager.TBoard = this;

    public new void Begin()
    {
        (scaleFactor, hasMargins, isTargetBoard) = (0.32f, false, true);
        base.Begin();
    }
}