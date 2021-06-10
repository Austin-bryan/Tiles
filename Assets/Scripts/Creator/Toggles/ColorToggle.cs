using System;
using static ColorExt;
using ExtensionMethods;
using static PathDebugger;
using PT = CreatorManager.PropertyType;

//todo: Have whtie toggle hidden that gets selected when selecting a white tile
//todo: Add a normal shade
public class ColorToggle : ToggleButton
{
    public BaseTileColor Color;
    protected override void UpdateTiles() => SelectedColor = Color.AddShade(SelectedShade);
}
public class ColorToggleManager : ToggleManager
{
    private static readonly Lazy<ColorToggleManager> lazy = new Lazy<ColorToggleManager>(() => new ColorToggleManager(PT.TileColor));
    public static ColorToggleManager Instance => lazy.Value;

    public ColorToggleManager(PT propertyType) : base(propertyType) { }
    protected override bool PropertiesMatch(CreatorTile tile, int? property)        => tile.Color == (TileColor)property;
    protected override void SetCurrentProperty(CreatorTile tile, out int? property) => property = (int?)tile?.Color;
}