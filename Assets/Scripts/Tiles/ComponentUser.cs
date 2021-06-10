using Tiles.Components;
using ExtensionMethods;
using System.Collections.Generic;

public class ComponentHandler
{
    public TileSideHandler Sides { get; set; }
    private List<(TileType, TileComponent)> componentsToAdd;
    private PlayerTile tile;

    public ComponentHandler(PlayerTile tile) => (this.tile, Sides, componentsToAdd) = (tile, new TileSideHandler(tile), new List<(TileType, TileComponent)>());

    // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Methods    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
    public void AddComponent(TileType type, TileComponent component)      => componentsToAdd.Add((type, component));
    public void AddComponents(List<(TileType, TileComponent)> components) => components.ForEach(c => componentsToAdd.Add((c.Item1, c.Item2)));
    public void AddComponents()
    {
        Sides.AddComponents(componentsToAdd);
        componentsToAdd?.Clear();
    }
    public void Activate(bool wasPlayerTriggered)
    {
        if (Sides.HasMoreThanOneSide) Sides.NextSide();
        Sides.CurrentSideComponents.ForEach(t => t.Activate(wasPlayerTriggered));
    }
}