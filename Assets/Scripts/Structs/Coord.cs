using UnityEngine;
using ExtensionMethods;
using static Board;
using static PathDebugger;
using Dir = Direction;

#pragma warning disable CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
[System.Serializable]
public struct Coord
{
    public int X, Y;

    public Coord((int x, int y) values) => (X, Y) = (values.x, values.y);
    public Coord(int x, int y) => (X, Y) = (x, y);
    public Coord(int n) => X = Y = n;

    public static Coord Random() => new Coord(RandomIndex(Size.X), RandomIndex(Size.Y));

    /// <summary> Returns the X coordinate if true, Y if false</summary>
    public int GetCoord(bool getX) => getX ? X : Y;

    public override string ToString() => $"{X}, {Y}";
    public Coord Wrap() => new Coord(X.Wrap(1, Size.X), Y.Wrap(1, Size.Y));

    public bool InBoard() => X > 0 && X < MarginX && Y > 0 && Y < Size.Y + 1;
    public bool InRange(int minX, int minY, int maxX, int maxY) => X.InRange(minX, maxX) && Y.InRange(minY, maxY);
    public bool InRange(int min, int maxX, int maxY) => InRange(min, min, maxX, maxY);
    public void Deconstruct(out int x, out int y)    => (x, y) = (X, Y);

    public static bool operator ==(Coord a, Coord b) => (a.X == b.X) && (a.Y == b.Y);
    public static bool operator !=(Coord a, Coord b) => (a.X != b.X) || (a.Y != b.Y);
    public static Coord operator +(Coord a, Coord b) => new Coord(a.X + b.X, a.Y + b.Y);
    public static Coord operator -(Coord a, Coord b) => new Coord(a.X - b.X, a.Y - b.Y);
    public static Coord operator +(Coord a, int b)   => new Coord(a.X + b, a.Y + b);
    public static Coord operator -(Coord a, int b)   => new Coord(a.X - b, a.Y - b);
    public static Coord operator *(Coord c, int a)   => new Coord(c.X * a, c.Y * a);
    public static Coord operator /(Coord c, int a)   => new Coord(c.X / a, c.Y / a);
    public static Coord operator +(Coord c, Dir d)   => c + d.ToCoord();
    public static Coord operator -(Coord c, Dir d)   => c - d.ToCoord();

    public static implicit operator Vector2(Coord c)        => new Vector2(c.X, c.Y);
    public static implicit operator Vector3(Coord c)        => new Vector3(c.X, c.Y, 0);
    public static implicit operator Coord((int x, int y) c) => new Coord(c.x, c.y);

    public bool IsEdge()             => IsRowEdge() || IsColEdge();
    public bool IsCorner()           => IsRowEdge() && IsColEdge();
    public bool IsRowEdge()          => CoordIsEdge(X, Size.X);
    public bool IsColEdge()          => CoordIsEdge(Y, Size.Y);
    public bool IsInFrame()          => X.InRange(1, Size.X) && Y.InRange(1, Size.Y);
    public bool IsInMargin()         => X.InRange(0, MarginX) && Y.InRange(0, MarginY);
    public bool IsMargin()           => X == 0 || X >= MarginX || Y == 0 || Y >= MarginY;
    public bool IsSameRow(Coord b)   => Y == b.Y;
    public bool IsSameCol(Coord b)   => X == b.X;
    public bool IsAdjecent(Coord b)  => ((X - b.X).Abs() == 1) || ((Y - b.Y).Abs() == 1) || ((Y - b.X).Abs() == 1) || ((Y - b.X).Abs() == 1);
    public bool IsSameLayer(Coord b) => IsSameRow(b) || IsSameCol(b);
    public int GetPlayerIndex()      => X + ((Y - 1) * (Size.X));
    public Coord GetOppositeRow()    => new Coord(GetOppositeCoordComponent(X, Size.X), Y);
    public Coord GetOppositeCol()    => new Coord(X, GetOppositeCoordComponent(Y, Size.Y));

    public Coord GetOppositeDiagonal()
    {
        var foundCoord = new Coord(X, Y);

        do foundCoord += SwipeManager.CurrentDirection.GetOppositeDirection();
        while (!foundCoord.IsMargin());

        return foundCoord;

        #region
            //int x, y;

            //if (SwipeComponent.SwipeDir.IsUnsyncedDiag())
            //{
            //    if (BoardSize.X <= BoardSize.Y)
            //    {
            //        if (Y == 0)
            //             (x, y) = (Y, X);
            //        else if (X <= MarginX)
            //             (x, y) = (0, MarginX + Y);
            //        else (x, y) = (MarginX, MarginX + X);
            //    }
            //    else
            //    {
            //        if (Y <= MarginX)  
            //             (x, y) = (0, MarginX + Y);
            //        else if (Y == 0)
            //             (x, y) = (Y, X);
            //        else (x, y) = (MarginX - Y, MarginY);
            //    }
            //}
            //else
            //{
            //    if (BoardSize.X <= BoardSize.Y)
            //    {
            //        if (X == 0 || Y == 0)
            //        {
            //            if (Y == 0) 
            //                 (x, y) = (MarginX, MarginX- X);
            //            else if (Y + MarginX >= MarginY) 
            //                 (x, y) = (MarginY - Y, MarginY);
            //            else (x, y) = (MarginX, MarginX + Y);
            //        }
            //        else
            //        {
            //            if (Y < MarginX) 
            //                 (x, y) = (MarginX - Y, 0);
            //            else if (Y < MarginY) 
            //                 (x, y) = (0, Y - BoardSize.X - 1 );
            //            else (x, y) = (0, MarginY - X);
            //        }
            //    }
            //    else
            //    {
            //        if (X == 0 || Y == 0)
            //        {
            //            if (X > (MarginX) - (MarginY)) (x, y) = (MarginX, MarginX - X);
            //            else if (Y < 1) (x, y) = (X + MarginY, MarginY);
            //            else (x, y) = (MarginY - Y, MarginY);
            //        }
            //        else
            //        {
            //            if (X == MarginX) 
            //                 (x, y) = (MarginX - Y, 0);
            //            else if (X * 2 > BoardSize.X) 
            //                 (x, y) = (X - (MarginY), 0);
            //            else (x, y) = (0, (MarginY) - X);
            //        }
            //    }

            //
            //if (X == 0) y = BoardSize.Y + 1;
            //else if (X == MarginX) y = 0;
            //else y = MarginX - X;

            //if (Y == 0) x = MarginX;
            //else if (Y == BoardSize.Y + 1) x = 0;
            //else x = BoardSize.Y + 1 - Y;
            //}

            //((X, Y), (x, y)).Log();
            //return new Coord(x, y);
            #endregion
    }
    public Coord GetOppositeCoord(Dir direction)
    {
             if (direction.IsRow()) return GetOppositeRow();
        else if (direction.IsCol()) return GetOppositeCol();
        else return GetOppositeDiagonal();
    }
    public bool? IsCloserTo(Coord coordA, Coord coordB, Dir direction)
    {
        var distanceA = GetDistanceToCoord(coordA, direction);
        var distanceB = GetDistanceToCoord(coordB, direction);

        if (distanceA == distanceB) return null;
        else return distanceA < distanceB;
    }

    public int?  GetDistanceToCoord(Coord coordA, Dir direction)
    {
        var coord = this;
        int steps = 0;

        while (true)
        {
            if (coord == coordA)  return steps;

            coord += direction;
            if (!coord.IsInFrame()) break;

            steps++;
        }

        coord = this;
        steps = 0;
        while (true)
        {
            if (coord == coordA)  return steps;

            coord -= direction;
            if (!coord.IsInFrame()) return null;

            steps++;
        }
    }

    private int  GetOppositeCoordComponent(int a, int size)
    {
        if (a <= 0)        return size;
        if (a == 1)        return size + 1;
        if (a == size)     return 0;
        if (a >= size + 1) return 1;
        return a;
    }
    private bool CoordIsEdge(int coord, int size) => coord == 1 || coord == size;
    private static int RandomIndex(int index) => UnityEngine.Random.Range(1, index);
}
// 160