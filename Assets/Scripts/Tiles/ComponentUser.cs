using Tiles.Modules;
using ExtensionMethods;
using System.Collections.Generic;

public class ModuleHandler
{
    public TileSideHandler Sides { get; set; }
    private List<(TileType, TileModule)> componentsToAdd;
    private PlayerTile tile;

    public ModuleHandler(PlayerTile tile) => (this.tile, Sides, componentsToAdd) = (tile, new TileSideHandler(tile), new List<(TileType, TileModule)>());

    // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Methods    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
    public void AddModule(TileType type, TileModule component)      => componentsToAdd.Add((type, component));
    public void AddModules(List<(TileType, TileModule)> components) => components.ForEach(c => componentsToAdd.Add((c.Item1, c.Item2)));
    public void AddModules()
    {
        Sides.AddModules(componentsToAdd);
        componentsToAdd?.Clear();
    }
    public void Activate(bool wasPlayerTriggered)
    {
        if (Sides.HasMoreThanOneSide) Sides.NextSide();
        Sides.CurrentSideModules.ForEach(t => t.Activate(wasPlayerTriggered));
    }
}