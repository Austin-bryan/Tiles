using System.Linq;
using Tiles.Components;
using ExtensionMethods;
using System.Collections.Generic;
using static PathDebugger;
using Components     = System.Collections.Generic.List<Tiles.Components.TileComponent>;
using ComponentsDict = System.Collections.Generic.Dictionary<TileType, Tiles.Components.TileComponent>;

public class TileComponents
{
    public List<TileType> Types              => TypeComponentsDict.Keys.ToList();
    public Components Components             => TypeComponentsDict.Values.ToList();
    public ComponentsDict TypeComponentsDict =  new Dictionary<TileType, TileComponent>();

    // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Methods    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
    public TileComponent GetComponent(TileType type) => HasType(type) ? TypeComponentsDict[type] : null;

    public bool HasType(TileType type) => TypeComponentsDict.Keys.Contains(type);
    public void RemoveComponents()
    {
        var types = TypeComponentsDict.Keys.ToArray();
        types.ForEach(t => RemoveComponent(t));
    }
    public void RemoveComponent(TileType type)
    {
        if (!HasType(type)) return;

        TypeComponentsDict[type].Remove();
        TypeComponentsDict.Remove(type);       
    }
    public void AddComponent(TileType type, TileComponent component, PlayerTile tile)
    {
        if (HasType(type)) return;

        component.UpdateTile(tile);
        component.SetVisibility(true);
        TypeComponentsDict.Add(type, component);
    }
    public void AddComponents(List<(TileType, TileComponent)> newComponents, PlayerTile tile) => newComponents.ForEach(c => AddComponent(c.Item1, c.Item2, tile));

    public void SetVisibilites(bool isVisible) => Components.ForEach(c => c.SetVisibility(isVisible));
    public void RemoveOneSwipeComponents(TileSideHandler sides = null)
    {
        var types = TypeComponentsDict.Keys.ToArray();

        types.ForEach(t =>
        {
            if (TypeComponentsDict[t].OneSwipeOnly)
                RemoveComponent(t);
        });
    }
}