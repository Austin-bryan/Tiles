using UnityEngine;
using UnityEngine.UI;
using ExtensionMethods;
using static TileSelector;
using static PathDebugger;
using static ColorExt;
using System.Collections.Generic;
using PT = CreatorManager.PropertyType;

public abstract class ToggleButton : MonoBehaviour
{
    public PT PropertyType { get; set; }

    // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Properties      ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
    public bool IsOn
    {
        get => this.Get<Toggle>().isOn;
        set => this.Get<Toggle>().isOn = value;
    }

    // ---- Color ---- //
    private static TileColor _selectedColor;
    protected static TileColor SelectedColor
    {
        get => _selectedColor;
        set
        {
            _selectedColor = value;
            CreatorTile.ColorTiles(true, value);
        }
    }
    protected static ColorShade SelectedShade { get; set; }
    protected static ToggleButton LastButton;

    // ---- Type ---- //
    private static TileType _selectedType;
    protected static TileType SelectedType
    {
        get => _selectedType;
        set
        {
            _selectedType = value;
            CreatorTile.SetTileType(true, (int)_selectedType, LastButton.IsOn);
        }
    }
    protected static List<TileProperty> SelectedTileProperties { get; } = new List<TileProperty>();

    // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Fields    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
    protected bool IsActive, MustBeActive = true;

    // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Constructors    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
    public ToggleButton(PT propertyType) => PropertyType = propertyType;
    public ToggleButton() { }

    // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Methods    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
    public void OnChange(bool isActive)
    {
        OnChange();
        UpdateTiles();
    }
    protected abstract void UpdateTiles();
    protected virtual  void OnChange() { }
}
public abstract class ToggleManager
{
    public PT PropertyType { get; set; }
    protected static bool bb = false;

    // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Constructors    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
    protected ToggleManager(PT propertyType) => PropertyType = propertyType;
    protected ToggleManager() { }

    // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Methods    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
    public virtual void UpdateToggleButtons<TKey>(Dictionary<TKey, ToggleButton> toggleButtons)
    {
        var (sameProperties, property) = SelectedTilesHaveSameProperty();
        //if (!sameProperties) return;

        TKey key = GetKey<TKey>(property);
        TurnOnToggle(toggleButtons, key);
    }
    public (bool Success, int? property) SelectedTilesHaveSameProperty()
    {
        int? property = null;

        foreach (var tile in SelectedTiles)
        {
            if (property == null) SetCurrentProperty(tile, out property);
            if (PropertiesMatch(tile, property)) continue;
            else return (false, null);
        }

        return (true, property);
    }
    protected abstract bool PropertiesMatch(CreatorTile tile, int? property);
    protected abstract void SetCurrentProperty(CreatorTile tile, out int? property);
    protected virtual  TKey GetKey<TKey>(int? property) => (property ?? 0).To<TKey>();
    protected virtual  void TurnOnToggle<TKey>(Dictionary<TKey, ToggleButton> toggleButtons, TKey key)
    {
        if (toggleButtons.ContainsKey(key))
            toggleButtons[key].IsOn = true;
    }
}
