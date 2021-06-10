using static BoardGenerator;

public class SizeValueBox : ValueBox
{
    protected override void ChangeValue(bool increase)
    {
        base.ChangeValue(increase);
        UpdateSize(name == "Size X");
    }
    public override void Decrease() => base.Decrease();
    public override void Increase() => base.Increase();
}