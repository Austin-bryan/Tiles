using static PathDebugger;

public class MiniTile : PlayerTile
{
    private ComponentHandler componentHandler;
    public MiniTile() => componentHandler = new ComponentHandler(this);

    public override void Start()     { }
    public override void BeginPlay() { }
    public override void OnMouseDown()
    {
        if (Sides.HasMoreThanOneSide) Sides.NextSide();
        Sides.CurrentSideComponents.ForEach(t => t.Activate(true));
    }
}//65