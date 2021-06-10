using System;
using static ColorExt;
using ExtensionMethods;
using PT = CreatorManager.PropertyType;

public class ShadeToggle : ToggleButton
{
    public ColorShade Shade;

    protected override void OnChange()    => UpdateShade();
    protected override void UpdateTiles() => SelectedColor = SelectedColor.AddShade(SelectedShade);
    private void UpdateShade()            => SelectedShade = IsActive ? Shade : ColorShade.None;
}

public class ShadeToggleManager : ToggleManager
{
    private static readonly Lazy<ShadeToggleManager> lazy = new Lazy<ShadeToggleManager>(() => new ShadeToggleManager(PT.TileColor));
    public static ShadeToggleManager Instance => lazy.Value;

    protected ShadeToggleManager(PT propertyType) : base(propertyType) { }

    protected override bool PropertiesMatch(CreatorTile tile, int? property)        => tile.ColorShade == (ColorShade)property;
    protected override void SetCurrentProperty(CreatorTile tile, out int? property) => property = (int?)tile?.ColorShade;
}