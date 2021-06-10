using static BoardGenerator;

public class ColorValueBox : ValueBox
{
    protected override void ChangeValue(bool increase)
    {
        base.ChangeValue(increase);
        UpdateColor();
    }
}
