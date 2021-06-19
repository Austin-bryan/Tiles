using System.Linq;
using ExtensionMethods;
using Tiles.Modules;
using static LayerType;
using static TileType;

public enum GameMode   { Moves, Timed, Fire, Bricks }
public enum TileType   { Normal, Gap, Wall, Camo, Iron, Brick, Steel, Nail, Screw, Ice, Balloon, Cement, Magnet, Amethyst, Bolt, Diagonal, Link, Warp, Rotate, ThisSideUp, Chess, Pawn, Rook, Knight, Bishop, King, Queen, Hybrid, Outlet, Lightbulb, TV, Tablet };
public enum MoveType   { Swipe, Rotate, Warp }
public enum SwipeType  { None, Bronze, Silver, Gold };
public enum LayerType  { Row, Column, Diagonal }
public enum MarginType { NoMargin, Margin, OnlyMargin }
public enum TileProperty
{
    None = 0b_0, Camo = 0b_0001
}
// ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Methods    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
public static class LayerTypeExt
{
    public static LayerType GetOppositeType(this LayerType layerType) => layerType == Row ? Column : Row;
}
public static class TileTypeExt
{
    public static TileType ToTileType(this TileProperty tileProperty)
    {
        switch (tileProperty)
        {
            case TileProperty.Camo: return Camo;
            default: return default;
        }
    }
    public static TileProperty ToTileProperty(this TileType tileType)
    {
        switch (tileType)
        {
            case Camo: return TileProperty.Camo;
            default: return default;
        }
    }
    public static bool HasCounter(this TileType tileType)            => tileType.RemoveProperties().IsType(Iron, Cement, Brick);
    public static bool UsesForeSprite(this TileType tileType)        => tileType.RemoveProperties().IsType(Iron, Cement, Ice, Balloon);
    public static bool UsesBackgroundColor(this TileType tileType)   => tileType == Wall || tileType == Gap;
    public static TileType RemoveProperties(this TileType tileType)  => tileType.RemoveFlag(Camo);
    public static bool IsType(this TileType tileType, TileType type) => tileType.HasFlag(type);

    public static bool IsType(this TileType tileType, params TileType[] types)        => types.ToList().Contains(tileType);
    public static bool IsType(this TileType tileType, TileType type1, TileType type2) => tileType.HasFlag(type1) || tileType.HasFlag(type2);
    public static bool IsType(this TileType tileType, TileType type1, TileType type2, TileType type3) => tileType.HasFlag(type1) || tileType.HasFlag(type2)|| tileType.HasFlag(type3);
    public static TileType GetProperties(this TileType tileType)
    {
        TileType properties = Normal;

        AddIfHasFlag(Camo);
        return properties.RemoveFlag(Normal);

        // ---- Local Functions ---- //
        void AddIfHasFlag(TileType type) { if (tileType.HasFlag(Camo)) properties.AddFlag(Camo); }
    }
    //public static TileModule ToTileModule(this TileType type) =>  (TileModule)System.Type.GetType($"Tiles.Modules.{type}Module");
    public static bool SameProperties(this TileType a, TileType b)  => a.GetProperties() == b.GetProperties();
}
