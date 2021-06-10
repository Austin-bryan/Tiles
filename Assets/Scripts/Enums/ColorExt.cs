using UnityEngine;
using ExtensionMethods;
using static PathDebugger;
using static TileColor;

public enum TileColor
{
    White, Red, Blue, Green, Pink, Orange, Yellow, Cyan, Purple, Magenta,
    Grey, LightRed, LightBlue, LightGreen, LightPink, LightOrange, LightYellow, LightCyan, LightPurple, LightMagenta,
    Black, DarkRed, DarkBlue, DarkGreen, DarkPink, DarkOrange, DarkYellow, DarkCyan, DarkPurple, DarkMagenta,
    WallColor, GapColor
};


public static class ColorExt
{
    private static readonly int colorSize = 10;

    public enum ColorShade    { None, Light, Dark };
    public enum BaseTileColor { White, Red, Blue, Green, Pink, Orange, Yellow, Cyan, Purple, Magenta, Black }

    /// <summary>
    /// Returns true if the color is effictively none, like WallColor or GapColor
    /// </summary>
    public static bool IsNone(this TileColor color) => color == WallColor || color == GapColor;

    public static Color32 ToColor(this TileColor color)
    {
        switch (color)
        {
            case White:         return new Color32(255, 255, 255, 255);
            case LightRed:      return new Color32(255, 100, 100, 255);
            case LightBlue:     return new Color32(150, 150, 255, 255);
            case LightGreen:    return new Color32(80,  200, 80,  255);
            case LightPink:     return new Color32(255, 105, 180, 255);
            case LightOrange:   return new Color32(255, 180, 50,  255);
            case LightYellow:   return new Color32(255, 255, 100, 255);
            case LightCyan:     return new Color32(100, 255, 255, 255);
            case LightPurple:   return new Color32(180, 40,  180, 255);
            case LightMagenta:  return new Color32(255, 5,   128, 255);
            case Grey:          return new Color32(100, 100, 100, 255);
            case Red:           return new Color32(255, 40,  40,  255);
            case Blue:          return new Color32(90,  90,  255, 255);
            case Green:         return new Color32(17,  170, 17,  255);
            case Pink:          return new Color32(221, 57,  115, 255);
            case Orange:        return new Color32(200, 120, 19,  255);
            case Yellow:        return new Color32(255, 255, 40,  255);
            case Cyan:          return new Color32(0,   200, 200, 255);
            case Purple:        return new Color32(128, 0,   128, 255);
            case Magenta:       return new Color32(255, 0,   255, 255);
            case Black:         return new Color32(50,  50,  50,  255);
            case DarkRed:       return new Color32(139, 0,   0,   255);
            case DarkBlue:      return new Color32(20,  20,  150, 255);
            case DarkGreen:     return new Color32(0,   100, 0,   255);
            case DarkPink:      return new Color32(255, 20,  147, 255);
            case DarkOrange:    return new Color32(150, 50,  0,   255);
            case DarkYellow:    return new Color32(150, 150, 0,   255);
            case DarkCyan:      return new Color32(20,  100, 100, 255);
            case DarkPurple:    return new Color32(80,  30,  80,  255);
            case DarkMagenta:   return new Color32(180, 30,  180, 255);

            default: return default;
        }
    }
    public static TileColor GetDark  (this TileColor color)
    {
        if (color == Black) return color;

        if (color.IsLight())  return GetShade(color, 1);
        if (color.IsNormal()) return GetShade(color, 2);

        return color;
    }
    public static TileColor GetLight (this TileColor color)
    {
        if (color == Black) return color;

        if (color.IsDark())   return GetShade(color, -1);
        if (color.IsNormal()) return GetShade(color, 1);

        return color;
    }
    public static TileColor GetNormal(this TileColor color)
    {
        if (color == Black) return color;

        if (color.IsLight()) return GetShade(color, -1);
        if (color.IsDark())  return GetShade(color, -2);

        return color;
    }
    public static TileColor AddShade (this TileColor color, ColorShade shade)
    {
        switch (shade)
        {
            case ColorShade.Dark: return color.GetDark();
            case ColorShade.Light: return color.GetLight();
            case ColorShade.None: return color.GetNormal();
        }

        return default;
    }
    public static TileColor AddShade (this BaseTileColor color, ColorShade shade) => color.ToTileColor().AddShade(shade);
    public static TileColor ToTileColor(this BaseTileColor color)     => color == BaseTileColor.Black ? Black : (TileColor)color;
    public static BaseTileColor ToBaseTileColor(this TileColor color) => color == Black ? BaseTileColor.Black : (BaseTileColor)color.GetNormal();

    private static bool IsNormal(this TileColor color) => ((int)color).InRange(0, 9);
    private static bool IsLight(this TileColor color)  => ((int)color).InRange(10, 19);
    private static bool IsDark(this TileColor color)   => ((int)color).InRange(20, 29);
    private static TileColor GetShade(TileColor color, int n) => (TileColor)(((int)color) + n * colorSize);
}
