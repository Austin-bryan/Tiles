using System;
using System.Linq;
using ExtensionMethods;
using static TileSelector;
using static PathDebugger;
using System.Collections.Generic;
using PT = CreatorManager.PropertyType;

public class TypeToggle : ToggleButton
{
    public TileType TileType;

    public TypeToggle(PT propertyType) : base(propertyType) { }
    protected override void UpdateTiles()
    {
        var properties = new List<TileType>();

        //foreach (var property in SelectedTileProperties)
        //    properties.Add((TileType)property);

        LastButton = this;
        SelectedType = TileType;
    }
}
public class TypeToggleManager : ToggleManager
{
    private static readonly Lazy<TypeToggleManager> lazy = new Lazy<TypeToggleManager>(() => new TypeToggleManager(PT.TileColor));
    public static TypeToggleManager Instance => lazy.Value;

    public TypeToggleManager(PT propertyType) : base(propertyType) { }
    public override void UpdateToggleButtons<TKey>(Dictionary<TKey, ToggleButton> toggleButtons)
    {
        foreach (var buttonPair in toggleButtons)
        {
            var button = buttonPair.Value.To<TypeToggle>();
            button.IsOn = SelectedTiles[0].Properties.Contains(button.TileType);
        }
    }

    protected override bool PropertiesMatch(CreatorTile tile, int? property) => tile.Type == (TileType)property;
    protected override void SetCurrentProperty(CreatorTile tile, out int? property) => property = 0;
    private T RemoveProperties<T>(ref T t) => t = t.To<TileType>().RemoveProperties().To<T>();
    private bool PropertiesMatch(CreatorTile tile, List<TileType> properties) => tile.Properties.SequenceEqual(properties);
}