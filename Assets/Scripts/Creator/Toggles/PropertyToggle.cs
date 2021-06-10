using System;
using static PathDebugger;

public class PropertyToggle : ToggleButton
{
    public bool WasCalled { get; set; }
    public TileType TileProperty;

    public PropertyToggle() => MustBeActive = false;
    protected override void UpdateTiles() => (WasCalled, LastButton, SelectedType) = (true, this, TileProperty);
}
