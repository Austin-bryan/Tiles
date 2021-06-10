using UnityEngine;
using ExtensionMethods;
using UnityEngine.SceneManagement;
using static PathDebugger;

public class LoadManager : MonoBehaviour
{
    public static string LevelName => "Level";

    private void Start() => this.Delay(0.02f, LoadSeed);
    public void LoadSeed()
    {
        LevelManager.SetLevel0(SeedCreator.GetSeed());
        SceneManager.LoadScene(LevelName);
    }
}
