using UnityEngine.UI;
using ExtensionMethods;
using static PathDebugger;

public class TargetTile : Tile
{
    public Coord D = default;
    public int X = 23;

    private bool idVisible    = true;
    private bool coordVisible = false;

    public override void BeginPlay()
    {
        showCoord();
        hideCoord();
        hideID();
        D = Coord;

        void hideID()    => hideChild(0, idVisible);
        void hideCoord() => hideChild(1, coordVisible);
        void hideChild(int index, bool visible) => this.GetChild(0, index).GetComponent<Text>().enabled = visible;
    }
}
