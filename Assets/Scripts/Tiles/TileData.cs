using static ColorExt;
using System.Collections.Generic;

public struct TileData
{
    public int Counter;
    public TileType Type;
    public TileColor Color;
    public List<Direction> Directions;

    public TileData(TileColor color, TileType type, int counter, List<Direction> directions) => (Color, Type, Counter, Directions) = (color, type, counter, directions);
}
