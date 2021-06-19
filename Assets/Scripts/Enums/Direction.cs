using UnityEngine;
using static LayerType;
using static Direction;

public enum Direction { None, Up, Down, Left, Right, UpLeft, UpRight, DownRight, DownLeft }

public static class DirectionUtilities
{
    public static Direction Random() => Random(1, 5);
    public static Direction Random(LayerType type)
    {
        switch (type)
        {
            case Row:    return Random(3, 4);
            case Column: return Random(1, 2);
            default: return default;
        }
    }

    // Extension Methods
    public static bool IsRow(this Direction dir)                => dir.ToLayerType()  == Row;
    public static bool IsCol(this Direction dir)                => dir.ToLayerType()  == Column;
    public static bool IsVertical(this Direction dir)           => dir == Up         || dir == Down;
    public static bool IsHorizontal(this Direction dir)         => dir == Left       || dir == Right;
    public static bool IsLayer(this Direction dir)              => IsHorizontal(dir) || IsVertical(dir);
    public static bool IsOrdinal(this Direction dir)            => IsAscendingDiagonal(dir) || IsDescendingDiagonal(dir);
    public static bool IsAscendingDiagonal(this Direction dir)  => dir == UpLeft     || dir == DownRight;
    public static bool IsDescendingDiagonal(this Direction dir) => dir == UpRight    || dir == DownLeft;
    public static Vector2 ToVector2(this Direction dir)         => dir.ToCoord();
    public static Vector3 ToVector3(this Direction dir)         => dir.ToCoord();
    public static LayerType ToLayerType(this Direction dir)     => dir.IsHorizontal() ? Row : dir.IsVertical() ? Column : Diagonal;
    public static Coord Times  (this Direction dir, int x)      => dir.ToCoord() * x;
    public static Coord Divide (this Direction dir, int x)      => dir.ToCoord() / x;
    public static bool Is(this Direction dir, SwipeStyle style) => style == SwipeStyle.Ordinal ? IsOrdinal(dir) : IsLayer(dir);
    public static Coord ToCoord(this Direction dir) => dir switch
    {
        Up        => new Coord(0, -1),
        Down      => new Coord(0, 1),
        Left      => new Coord(-1, 0),
        Right     => new Coord(1, 0),
        UpRight   => new Coord(1, -1),
        DownRight => new Coord(1, 1),
        UpLeft    => new Coord(-1, -1),
        DownLeft  => new Coord(-1, 1),
        _ => new Coord(0, 0),
    };
    public static Direction GetOppositeDirection(this Direction dir) => dir switch
    {
        Up        => Down,
        Down      => Up,
        Left      => Right,
        Right     => Left,
        UpRight   => DownLeft,
        DownRight => UpLeft,
        UpLeft    => DownRight,
        DownLeft  => UpRight,
        _         => None,
    };

    private static Direction Random(int a, int b) => (Direction)UnityEngine.Random.Range(a, b + 1);
}
