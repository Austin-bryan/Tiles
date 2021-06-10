using UnityEngine;
using Tiles.Components;
using ExtensionMethods;
using static TileType;
using static PathDebugger;
using System.Collections.Generic;
using Components = System.Collections.Generic.List<Tiles.Components.TileComponent>;
using ComponentsDict = System.Collections.Generic.Dictionary<TileType, Tiles.Components.TileComponent>;

// ADD SWIPE MODE
public class TileSideHandler
{
    // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Fields    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
    public int CurrentIndex { get; private set; } = 0;
    public int Count                        => sides.Count;
    public bool HasMoreThanOneSide          => Count > 1;
    public List<TileType> CurrentSideTypes  => CurrentSide.TileComponents.Types;
    public Components CurrentSideComponents => CurrentSide.TileComponents.Components;
    public ComponentsDict TypePropertyDict  => CurrentSide.TileComponents.TypeComponentsDict;
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
    public TileComponents this[int index] => sides[index].TileComponents;

    // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Methods    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
    // ---- Misc ---- //
    public void SetVisibilities(bool isVisible)         => sides.ForEach(s => s.SetVisibilities(isVisible));
    public void SetColor(TileColor color, int side = 0) => sides[side] = new TileSide(color, sides[side].TileComponents, tile, sides[side].IsOneSwipe);
    public bool HasOneSwipeSide()
    {
        foreach (var side in sides)
            if (side.IsOneSwipe) return true;
        return false;
    }
    public void SetSidesToOneSwipe()
    {
        for (int i = 0; i < sides.Count; i++)
            if (isAllComponentsOneSwipe(i))
                sides[i].IsOneSwipe = Count > 1;

        // ---- Local Functions ---- //
        bool isAllComponentsOneSwipe(int side)
        {
            foreach (var component in sides[side].TileComponents.Components)
            {
                if (!component.OneSwipeOnly)
                    return false;
            }

            return true;
        }
    }
    public void UpdateSubscriptions(PlayerTile parentTile)
    {
        foreach (var side in sides)
        {
            foreach (var component in side.TileComponents.Components)
            {
                //if (component.IsSubscribedToAllMovesFinished)
                    
            }
        }
    }
    public void SetTile(PlayerTile tile)
    {
        this.tile = tile;

        for (int i = 0; i < sides.Count; i++)
            foreach (var component in sides[i].TileComponents.Components)
                component.UpdateTile(tile);
    }
    public bool HasOneSwipeComponent()
    {
        foreach (var side in sides)
            foreach (var component in side.TileComponents.Components)
                if (component.OneSwipeOnly) return true;
        return false;
    }
    public bool HasComponent(TileType type)
    {
        if (type != Chess) return CurrentSide.TileComponents.Types.Contains(type);

        return HasComponent(Pawn) || HasComponent(Rook) || HasComponent(Knight) || HasComponent(Bishop) || HasComponent(King) || HasComponent(Queen);
    }
    public TileComponent GetComponent(TileType type)
    {
        foreach (var side in sides)
            if (side.TileComponents.TypeComponentsDict.ContainsKey(type))
                return side.TileComponents.TypeComponentsDict[type];

        if (type == Chess)
        {
            var component = GetComponent(Pawn);
            if (component != null) return component;

            component = GetComponent(Rook);
            if (component != null) return component;

            component = GetComponent(Knight);
            if (component != null) return component;

            component = GetComponent(Bishop);
            if (component != null) return component;

            component = GetComponent(King);
            if (component != null) return component;

            component = GetComponent(Queen);
            if (component != null) return component;
        }

        return null;
    }

    // ---- Adding ---- //
    public void AddSide(TileColor color = TileColor.White, bool isOneSwipe = false)     => sides.Add(new TileSide(color, new TileComponents(), tile, isOneSwipe));
    public void AddComponent(TileType type,   TileComponent component,    int side = 0) => CurrentSide.TileComponents.AddComponent(type, component, tile);
    public void AddComponents(List<(TileType, TileComponent)> components, int side = 0)
    {
        CurrentSide.TileComponents.AddComponents(components, tile);
    }

    // ---- Removing ---- //
    public void RemoveComponents()  => CurrentSide.TileComponents.RemoveComponents();
    public void RemoveComponent(TileType type, int side = 0)
    {
        if (!HasComponent(type)) return;
        if (type != Chess) { CurrentSide.TileComponents.RemoveComponent(type); return; }

        RemoveComponent(Pawn);
        RemoveComponent(Rook);
        RemoveComponent(Knight);
        RemoveComponent(Bishop);
        RemoveComponent(King);
        RemoveComponent(Queen);
    }
    public void RemoveOneSwipeComponents()
    {
        foreach (var side in sides) side.TileComponents.RemoveOneSwipeComponents(this);
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

        sides[oldIndex].TileComponents.Components.ForEach(c => c.Hide());
        sides[CurrentIndex].TileComponents.Components.ForEach(c => c.Show());
    }

    // ==== Tile Side ==== //
    private class TileSide : MonoBehaviour
    {
        public bool IsOneSwipe { get; set; }
        public TileColor Color;
        public TileComponents TileComponents;
        private PlayerTile tile;

        // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Methods    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
        public TileSide(TileColor color, TileComponents components, PlayerTile tile, bool isOneSwipe) =>
            (Color, TileComponents, this.tile, IsOneSwipe) = (color, components, tile, isOneSwipe);

        public void AddComponents(List<(TileType, TileComponent)> components) => TileComponents.AddComponents(components, tile);
        public void SetVisibilities(bool isVisible) => TileComponents.SetVisibilites(isVisible);
    }
}