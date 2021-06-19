using System.Linq;
using Tiles.Modules;
using ExtensionMethods;
using System.Collections.Generic;
using static PathDebugger;
using Modules     = System.Collections.Generic.List<Tiles.Modules.TileModule>;
using ModulesDict = System.Collections.Generic.Dictionary<TileType, Tiles.Modules.TileModule>;

public class TileModules
{
    public List<TileType> Types              => TypeModulesDict.Keys.ToList();
    public Modules Modules             => TypeModulesDict.Values.ToList();
    public ModulesDict TypeModulesDict =  new Dictionary<TileType, TileModule>();

    // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Methods    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
    public TileModule GetModule(TileType type) => HasType(type) ? TypeModulesDict[type] : null;

    public bool HasType(TileType type) => TypeModulesDict.Keys.Contains(type);
    public void RemoveModules()
    {
        var types = TypeModulesDict.Keys.ToArray();
        types.ForEach(t => RemoveModule(t));
    }
    public void RemoveModule(TileType type)
    {
        if (!HasType(type)) return;
        
        TypeModulesDict[type].Remove();
        TypeModulesDict.Remove(type);       
    }
    public void AddModule(TileType type, TileModule component, PlayerTile tile)
    {
        if (HasType(type)) return;

        component.UpdateTile(tile);
        component.SetVisibility(true);
        TypeModulesDict.Add(type, component);
    }
    public void AddModules(List<(TileType, TileModule)> newModules, PlayerTile tile) => newModules.ForEach(c => AddModule(c.Item1, c.Item2, tile));

    public void SetVisibilites(bool isVisible) => Modules.ForEach(c => c.SetVisibility(isVisible));
    public void RemoveOneSwipeModules(TileSideHandler sides = null)
    {
        var types = TypeModulesDict.Keys.ToArray();

        types.ForEach(t =>
        {
            if (TypeModulesDict[t].OneSwipeOnly)
                RemoveModule(t);
        });
    }
}