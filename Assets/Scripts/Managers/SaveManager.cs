using UnityEngine;
using ExtensionMethods;
using static PathDebugger;
using static UnityEngine.PlayerPrefs;
using static CreatorManager;
using static SeedCreator;

public class SaveManager : MonoBehaviour
{
    private const string creatorLevelCountName = "CreatorLevelCount";
    private const string creatorLevelName      = "Creator Level";
    private const string selectorPositionNameX = "SelectorPositionX", selectorPositionNameY = "SelectorPositionY";

    public static Vector3 SelectorPosition => new Vector3(GetFloat(selectorPositionNameX), GetFloat(selectorPositionNameY));

    public void SaveCreatorLevel()
    {
        SetInt(creatorLevelCountName, CurrentCreatorLevel);
        SetString($"{creatorLevelName} {CurrentCreatorLevel}", GetSeed());
    }
    public void SaveCreatorLevelAs()
    {
        int count = GetInt("CreatorLevelCount", 0) + 1;
        SetInt(creatorLevelCountName, count);
        SetString($"{creatorLevelName} {count}", GetSeed());
    }
    public void SaveCreatorLevelNewVersion()
    {
        int count = GetInt("CreatorLevelCount", 0) + 1;

        SetInt(creatorLevelCountName, count);
        SetString($"{creatorLevelName} {count}", GetSeed());
    }
    public static void SaveSelectorPosition(Vector3 vector)
    {
        SetFloat(selectorPositionNameX, vector.x);
        SetFloat(selectorPositionNameY, vector.y);
    }
    //public void LoadCreatorLevel(Text dialog)
    //{
    //    int level = dialog.text.ParseInt();

    //    Levels[0] = GetString($"Creator Level {level}");

    //    CurrentCreatorLevel = level;
    //    Instance.Start();
    //    BoardManager.BoardSize = GetBoardSize(0);
    //    CreatorBoard.Instance.Refresh();
    //}
    public static int GetCreatorLevelCount() => GetInt(creatorLevelCountName);
}
