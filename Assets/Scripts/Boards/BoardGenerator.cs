using UnityEngine;
using static Board;
using UnityEngine.UI;
using static PathDebugger;
using static BoardManager;

public class BoardGenerator : MonoBehaviour
{
    public ValueBox ValueBoxX, ValueBoxY, ValueColor, PatternDifficulty;
    public Button ShuffleButton, RestartButton;
    public static bool ForceSquare;

    private static BoardGenerator instance;

    public void Start()   => instance = this;
    public void Shuffle() => ShuffleManager.BeginShuffle();

    public static void UpdateSize(bool calledFromX)
    {
        if (ForceSquare) instance.ValueBoxY.Value = ParseField(calledFromX ? instance.ValueBoxX : instance.ValueBoxY);

        Size = new Coord(ParseField(instance.ValueBoxX), ParseField(instance.ValueBoxY));
        RestartBoards();
    }
    public static void UpdateColor()
    {
        Tile.MaxColorCount   = ParseField(instance.ValueColor);
        Tile.ColorDifficulty = ParseField(instance.PatternDifficulty);
        RestartBoards();
    }
    private static void RestartBoards()
    {
        PBoard?.Restart();
        TBoard?.Restart();
        CBoard?.Restart();
    }
    private static int ParseField(ValueBox field) => field.Value;
}
//56