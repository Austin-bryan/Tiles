using System.Linq;
using UnityEngine;
using Tiles.Modules;
using Tiles.Creator;
using UnityEngine.UI;
using ExtensionMethods;
using System.Collections.Generic;
using static TileType;
using static ColorExt;
using static BoardManager;
using static TileSelector;
using static PathDebugger;

public class CreatorTile : PlayerTile
{
    #region { { Properties } }
    // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Properties      ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
    public TileType Type         { get; set; }
    public ColorShade ColorShade { get; set; }
    public IEnumerable<TileType> Properties { get {
            foreach (var component in Sides.CurrentSideModules)
                yield return component.TileType;
        }
    }

    #region { Full Properties }
    private SpriteRenderer _mainSprite;
    private SpriteRenderer _foreSprite;
    private SpriteRenderer _camoSprite;
    private SpriteMask _imageMask;
    private SpriteMask _foreMask;
    private SpriteMask _mainMask;
    private SpriteRenderer MainSprite => _mainSprite ?? (_mainSprite = this.Get<SpriteRenderer>(mainSpriteIndex));
    private SpriteRenderer ForeSprite => _foreSprite ?? (_foreSprite = this.Get<SpriteRenderer>(foreSpriteIndex));
    private SpriteRenderer CamoSprite => _camoSprite ?? (_camoSprite = this.Get<SpriteRenderer>(camoIndex)); 
    private SpriteMask ImageMask      => _imageMask  ?? (_imageMask  = this.Get<SpriteMask>(mainSpriteIndex)); 
    private SpriteMask ForeMask       => _foreMask   ?? (_foreMask   = this.Get<SpriteMask>(foreSpriteIndex)); 
    private SpriteMask MainMask       => _mainMask   ?? (_mainMask   = this.Get<SpriteMask>(mainMaskIndex));

    public override TileColor Color
    {
        get => _color;
        set
        {
            _color = value;
            this.GetChild(0, 5).Get<SpriteRenderer>().color = Color.ToColor();
        }
    }
    #endregion
    #endregion { Properties }

    // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Fields    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
    public bool IsSelected = false;
    public Text CoordText;

    private static Input input = new Input();
    private static readonly int mainSpriteIndex = 1, foreSpriteIndex = 3, mainMaskIndex = 4, camoIndex = 6;
    private static SeedCreator seedCreator = new SeedCreator();
    private static bool needsToSetupSeedCreator = true;
    private static int counter = 0;
    private bool needsToRefreshColor;

    public new void Awake()
    {
        base.Awake();
        ID = counter++;
    }

    // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Methods    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
    // --------  Public Methods  -------- //
    public static void ColorTiles(bool hasChanged, TileColor color) => ColorTiles(hasChanged, (int)color);
    public override void Start()
    {
        if (!needsToSetupSeedCreator) return;

        needsToSetupSeedCreator = false;
        SeedCreator.GetSeedText(GameObject.FindGameObjectWithTag("Level Seed"));
    }
    public override void OnMouseDown() => ToggleSelection();

    public void Select()
    {
        Select(true);
        SelectedTiles.Add(this);

        if (SelectedTiles.Count == 1) FirstSelectedTile = this;
        MostRecentlySelectedTile = this;
        UpdateToggleButtons();
    }
    public void SetColor(TileColor color)
    {
        if (!TileSelector.ShouldUpateTileProperties) return;

        Color = color;

        if (!color.IsNone()) SetColor(color.ToColor());
    }
    public void SetType(TileType? type, bool visibility)
    {
        Type = type ?? Normal;
        bool usesForeSprite = Type.UsesForeSprite();

        UpdateSprites();
        UpdateColor();

        // ---- Local Functions ---- //
        void UpdateSprites() => CreatorSpriteShower.ShowSprite(this, type ?? Normal, visibility);
        void UpdateColor()
        {
            if (needsToRefreshColor) RefreshColor();
        }
    }
    public override void AddModule(TileType type, TileModule component)
    {
        if (!Properties.ToList().Contains(component.TileType))
            Sides.AddModule(type, component); 
    }
    public void RemoveModule(TileType property)
    {
        var components = Sides.CurrentSideModules.ToArray();

        foreach (var component in components)
        {
            if (component.TileType != property) continue;

            component.Show();
            //Sides.RemoveModule(component);
        }
    }
    public void Deselect()
    {
        Select(false);
        SelectedTiles.Remove(this);
    }

    // --------  Private Methods  -------- //
    private static void ColorTiles(bool hasChanged, int index)
    {
        SelectedTiles.ForEach(t => t?.SetColor((TileColor)index));
        SeedCreator.UpdateSeed();
    }
    public static void SetTileType(bool hasChanged, int index, bool visibility)
    {
        SelectedTiles.ForEach(t => t?.SetType((TileType)index, visibility));
        SeedCreator.UpdateSeed();
    }
    private void SetColor(Color color) => MainSprite.color = color;
    private void RefreshColor()
    {
        MainSprite.color = Color.ToColor();
        needsToRefreshColor = false;
    }
    public void ToggleSelection()
    {
        if (!input.Ctrl() && !input.Shift())
        {
            DeselectAll(this);
            FirstSelectedTile = this;
        }
        if (!IsSelected) Select();
        if (input.Shift()) CBoard.ShiftSelectTiles(FirstSelectedTile);
    }
    private void Select(bool isSelected)
    {
        if (IsSelected && isSelected) return;

        IsSelected = isSelected;
        this.GetChild(0, 4).Get<Animation>().Play(IsSelected ? "Select" : "Deselect");
    }
}
