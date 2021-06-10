using UnityEngine;
using UnityEngine.UI;
using ExtensionMethods;
using static PathDebugger;

public class MenuBar : MonoBehaviour
{
    public CreatorLevelDialog OpenLevel, SaveLevel, Seed;
    public InputField SeedField;

    public void FileOpen()
    {
        OpenLevel?.Show(true);
        SaveLevel?.Show(false);
    }
    public void ShowSeed()
    {
        Seed.Show(true);
        SeedField.text = SeedCreator.GetSeed();
    }
}
