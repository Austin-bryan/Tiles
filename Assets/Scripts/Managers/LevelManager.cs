using UnityEngine;
using ExtensionMethods;
using UnityEngine.UI;
using serial = UnityEngine.SerializeField;

public partial class LevelManager : MonoBehaviour
{
    [serial] Text levelText;

    public static  int CurrentLevel     = 17;
    public static  string CreatorName   => "Creator";
    public static  bool LevelZeroIsMade = true;
    private static bool hasBeenCreated;
    
    public static void SetLevel0(string data) => (Levels[0], LevelZeroIsMade) = (data, true);
    private void OnLevelWasLoaded()
    {
        if (levelText != null)
            levelText.text = CurrentLevel.ToString();
        this.MakePersistentSingleton(ref hasBeenCreated);
    }
    private void Start() { if (levelText != null) levelText.text = $"~{CurrentLevel}~"; }
}