using UnityEngine;
using UnityEngine.UI;
using ExtensionMethods;
using static BoardManager;
using static RelativeMotion;

public abstract class GridSlot : MonoBehaviour
{
    private Coord _coord;
    public Coord Coord
    {
        get => _coord;
        set
        {
            OnCoordChange(_coord, value);
            _coord = value;
        }
    }
    public int ID;

    public GridSlot Child, Parent;

    // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Methods    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
    public abstract void BeginPlay();
    public virtual void OnCoordChange(Coord oldCord, Coord newCoord) { }

    public bool IsEdge()    => Coord.IsEdge();
    public bool IsCorner()  => Coord.IsCorner();
    public bool IsRowEdge() => Coord.IsRowEdge();
    public bool IsColEdge() => Coord.IsColEdge();
    public bool IsSameCol(GridSlot b)   => Coord.IsSameCol(b.Coord);
    public bool IsSameRow(GridSlot b)   => Coord.IsSameRow(b.Coord);
    public bool IsSameLayer(GridSlot b) => Coord.IsSameLayer(b.Coord);

    public void SetCoord(Coord coord)
    {
        Coord = coord;

        if (PBoard != null && this is PlayerTile p)
        {
            PBoard[coord] = p;
            showCoord();
        }
    }
    public RelativeMotion GetRelativeMotion(GridSlot other, Direction direction)
    {
        if (other == null) return None;
        if (!IsSameLayer(other)) return None;

        int coordDifference, difference;
        bool useX = !IsSameRow(other);

        coordDifference = other.Coord.GetCoord(useX) - Coord.GetCoord(useX);
        difference = coordDifference - direction.ToCoord().GetCoord(useX);

        return coordDifference.HasSameSign(difference) ? Away : Towards;
    }

    protected void showCoord() => this.GetChild<Text>(0, 1).text = Coord.ToString();
}
