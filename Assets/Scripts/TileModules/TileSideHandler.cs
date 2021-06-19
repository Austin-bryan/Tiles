using UnityEngine;
using Tiles.Modules;
using ExtensionMethods;
using System.Collections.Generic;
using static TileType;
using static PathDebugger;
using Modules = System.Collections.Generic.List<Tiles.Modules.TileModule>;
using ModulesDict = System.Collections.Generic.Dictionary<TileType, Tiles.Modules.TileModule>;

// ADD SWIPE MODE
public class TileSideHandler
{
    // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Fields    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
    public int CurrentIndex { get; private set; } = 0;
    public int Count                        => sides.Count;
    public bool HasMoreThanOneSide          => Count > 1;
    public List<TileType> CurrentSideTypes  => CurrentSide.TileModules.Types;
    public Modules CurrentSideModules       => CurrentSide.TileModules.Modules;
    public ModulesDict TypePropertyDict     => CurrentSide.TileModules.TypeModulesDict;
    public TileColor Color
    {
        get => CurrentSide.Color;
        set => CurrentSide.Color = value;
    }

    private List<TileSide> sides = new List<TileSide>();
    private TileSide CurrentSide => sides[CurrentIndex];
    private PlayerTile tile;

    // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Class    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
    public TileSideHandler(PlayerTile tile)
    {
        AddSide();
        SetTile(tile);
    }
    public TileModules this[int index] => sides[index].TileModules;

    // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Methods    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
    // ---- Misc ---- //
    public void SetVisibilities(bool isVisible)         => sides.ForEach(s => s.SetVisibilities(isVisible));
    public void SetColor(TileColor color, int side = 0) => sides[side] = new TileSide(color, sides[side].TileModules, tile, sides[side].IsOneSwipe);
    public bool HasOneSwipeSide()
    {
        foreach (var side in sides)
            if (side.IsOneSwipe) return true;
        return false;
    }
    public void SetSidesToOneSwipe()
    {
        for (int i = 0; i < sides.Count; i++)
            if (isAllModulesOneSwipe(i))
                sides[i].IsOneSwipe = Count > 1;

        // ---- Local Functions ---- //
        bool isAllModulesOneSwipe(int side)
        {
            foreach (var component in sides[side].TileModules.Modules)
                if (!component.OneSwipeOnly)
                    return false;
            return true;
        }
    }
    public void SetTile(PlayerTile tile)
    {
        this.tile = tile;

        for (int i = 0; i < sides.Count; i++)
            foreach (var component in sides[i].TileModules.Modules)
                component.UpdateTile(tile);
    }
    public bool HasOneSwipeModule()
    {
        foreach (var side in sides)
            foreach (var component in side.TileModules.Modules)
                if (component.OneSwipeOnly) return true;
        return false;
    }
    public bool HasModule(TileType type)
    {
        if (type != Chess) return CurrentSide.TileModules.Types.Contains(type);

        return HasModule(Pawn) || HasModule(Rook) || HasModule(Knight) || HasModule(Bishop) || HasModule(King) || HasModule(Queen);
    }
    public TileModule GetModule(TileType type)
    {
        foreach (var side in sides)
            if (side.TileModules.TypeModulesDict.ContainsKey(type))
                return side.TileModules.TypeModulesDict[type];

        if (type == Chess)
        {
            var component = GetModule(Pawn);
            if (component != null) return component;

            component = GetModule(Rook);
            if (component != null) return component;

            component = GetModule(Knight);
            if (component != null) return component;

            component = GetModule(Bishop);
            if (component != null) return component;

            component = GetModule(King);
            if (component != null) return component;

            component = GetModule(Queen);
            if (component != null) return component;
        }

        return null;
    }

    // ---- Adding ---- //
    public void AddSide(TileColor color = TileColor.White, bool isOneSwipe = false)     => sides.Add(new TileSide(color, new TileModules(), tile, isOneSwipe));
    public void AddModule(TileType type,   TileModule component,    int side = 0) => CurrentSide.TileModules.AddModule(type, component, tile);
    public void AddModules(List<(TileType, TileModule)> components, int side = 0)
    {
        CurrentSide.TileModules.AddModules(components, tile);
    }

    // ---- Removing ---- //
    public void RemoveModules()  => CurrentSide.TileModules.RemoveModules();
    public void RemoveModule(TileType type, int side = 0)
    {
        if (!HasModule(type)) return;
        if (type != Chess) { CurrentSide.TileModules.RemoveModule(type); return; }

        RemoveModule(Pawn);
        RemoveModule(Rook);
        RemoveModule(Knight);
        RemoveModule(Bishop);
        RemoveModule(King);
        RemoveModule(Queen);
    }
    public void RemoveOneSwipeModules()
    {
        foreach (var side in sides) side.TileModules.RemoveOneSwipeModules(this);
    }
    public void RemoveSide(int index = -1)
    {
        index = (index == - 1) ? sides.Count - 1 : index;
        sides.RemoveAt(index);

        if (CurrentIndex >= index /*&& CurrentIndex > 0*/)
        {
            CurrentIndex--;
            if (CurrentIndex < 0)
                CurrentIndex = 0;
        }
    }
    public void RemoveOneSwipeSides()
    {
        var _sides = sides.Duplicate();

        foreach (var side in _sides)
            if (side.IsOneSwipe && sides.Count > 1)
                RemoveSide(sides.IndexOf(side));
        SetSidesToOneSwipe();
    }

    // ---- Changing Sides ---- //
    public void SetToDefault() => ChangeSide(0);
    public void NextSide()     => ChangeSide(CurrentIndex + 1);
    public void PrevSide()     => ChangeSide(CurrentIndex - 1);
    public void ChangeSide(int newSide)
    {
        int oldIndex = CurrentIndex;
        CurrentIndex = newSide;

        if (CurrentIndex >= Count) CurrentIndex = 0;
        else if (CurrentIndex < 0) CurrentIndex = Count - 1;

        Switch(oldIndex);
    }

    private void Switch(int oldIndex)
    {
        tile.Color = Color;

        sides[oldIndex].TileModules.Modules.ForEach(c => c.Hide());
        sides[CurrentIndex].TileModules.Modules.ForEach(c => c.Show());
    }

    // ==== Tile Side ==== //
    private class TileSide : MonoBehaviour
    {
        public bool IsOneSwipe { get; set; }
        public TileColor Color;
        public TileModules TileModules;
        private PlayerTile tile;

        // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Methods    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
        public TileSide(TileColor color, TileModules components, PlayerTile tile, bool isOneSwipe) =>
            (Color, TileModules, this.tile, IsOneSwipe) = (color, components, tile, isOneSwipe);

        public void AddModules(List<(TileType, TileModule)> components) => TileModules.AddModules(components, tile);
        public void SetVisibilities(bool isVisible) => TileModules.SetVisibilites(isVisible);
    }
}