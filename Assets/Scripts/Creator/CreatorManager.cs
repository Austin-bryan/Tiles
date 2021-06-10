using System;
using UnityEngine;
using UnityEngine.UI;
using ExtensionMethods;
using static PathDebugger;
using static ParseManager;
using static LevelManager;
using System.Collections.Generic;
using serial = UnityEngine.SerializeField;

public class CreatorManager : MonoBehaviour
{
    public enum PropertyType { TileColor, TileType, ShadeType, TileProperty }

    [Serializable]
    public struct TileImage
    {
        public TileType Type;
        public Sprite Image;
    }
    public static ValueBox[] ValueBoxes => instance.valueBoxes;

    // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Fields    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
    // --------  Public Static Fields  -------- //
    public static int  CurrentCreatorLevel = 0;
    public static Dictionary<TileType, Sprite> TileSprites = new Dictionary<TileType, Sprite>();

    // --------  Private Fields  -------- //
    [serial] TileImage[] tileImages;
    [serial] Dropdown    tileColorMenu, tileTypeMenu;
    [serial] ValueBox[]  valueBoxes;
    [serial] Toggle[]    arrows;
    [serial] InputField  seedText;
    private readonly bool startBoardManager = false;
    private static CreatorManager instance;

    // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Methods    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
    public void Awake() => tileImages.ForEach(t => TileSprites.Add(t.Type, t.Image));
    public void Start()
    {
        instance = this;
        seedText.text = Levels[0];
        UpdateValueBoxValues();
    }
    private void UpdateValueBoxValues() => (valueBoxes[0].Value, valueBoxes[1].Value, valueBoxes[2].Value, valueBoxes[3].Value) = (GetBoardSize(0).X, GetBoardSize(0).Y, GetLimiter(0), GetShuffleCount(0));
}
//174