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
    public static bool IsDiagonal(this Direction dir)           => IsSyncedDiag(dir) || IsUnsyncedDiag(dir);
    public static bool IsSyncedDiag(this Direction dir)         => dir == DownRight  || dir == UpLeft;
    public static bool IsUnsyncedDiag(this Direction dir)       => dir == UpRight    || dir == DownLeft;
    public static Vector2 ToVector2(this Direction dir)         => dir.ToCoord();
    public static Vector3 ToVector3(this Direction dir)         => dir.ToCoord();
    public static LayerType ToLayerType(this Direction dir)     => dir.IsHorizontal() ? Row : dir.IsVertical() ? Column : Diagonal;
    public static Coord Times  (this Direction dir, int x)      => dir.ToCoord() * x;
    public static Coord Divide (this Direction dir, int x)      => dir.ToCoord() / x;
    public static bool Is(this Direction dir, SwipeStyle style) => style == SwipeStyle.Diagonal ? IsDiagonal(dir) : IsLayer(dir);
    public static Coord ToCoord(this Direction dir)
    {
        switch (dir)
        {
            case None:       return new Coord( 0,  0);
            case Up:         return new Coord( 0, -1);
            case Down:       return new Coord( 0,  1);
            case Left:       return new Coord(-1,  0);
            case Right:      return new Coord( 1,  0);
            case UpRight:    return new Coord( 1, -1);
            case DownRight:  return new Coord( 1,  1);
            case UpLeft:     return new Coord(-1, -1);
            case DownLeft:   return new Coord(-1,  1);
        }

        return new Coord(0, 0);
    }
    public static Direction GetOppositeDirection(this Direction dir)
    {
        switch (dir)
        {
            case Up:        return Down;
            case Down:      return Up;
            case Left:      return Right;
            case Right:     return Left;
            case UpRight:   return DownLeft;
            case DownRight: return UpLeft;
            case UpLeft:    return DownRight;
            case DownLeft:  return UpRight;

            default:        return None;
        }
    }

    private static Direction Random(int a, int b) => (Direction)UnityEngine.Random.Range(a, b + 1);
}
