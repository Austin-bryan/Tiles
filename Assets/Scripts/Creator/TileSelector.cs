using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using ExtensionMethods;
using static PathDebugger;

public static class TileSelector
{
    public static List<CreatorTile> SelectedTiles { get; private set; } = new List<CreatorTile>();
    public static bool ShouldUpateTileProperties  { get; set; }
    public static CreatorTile FirstSelectedTile, MostRecentlySelectedTile;

    public static void DeselectAll(CreatorTile ignoreTile)
    {
        var selectedTiles = SelectedTiles.ToArray();

        selectedTiles.Where(t => !t.RefEquals(ignoreTile))
                     .ToList()
                     .ForEach(t => t.Deselect());
    }
    public static void UpdateToggleButtons()
    {
        ShouldUpateTileProperties = false;          // Must be set off to prevent triggering the styling events of the buttons
        ToggleButtonManager.UpdateToggleButtons();  
        ShouldUpateTileProperties = true;           // Can be set back on now that we're done
    }
}
