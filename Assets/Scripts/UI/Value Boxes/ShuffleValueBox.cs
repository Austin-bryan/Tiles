using UnityEngine;

public class ShuffleValueBox : ValueBox
{
    protected override void ChangeValue(bool increase)
    {
        base.ChangeValue(increase);
        /* Change Shuffle Count */
    }
    public override void Decrease() => base.Decrease();
    public override void Increase() => base.Increase();
}
