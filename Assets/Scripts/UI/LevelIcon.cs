using UnityEngine;
using ExtensionMethods;
using UnityEngine.UI;
using static PathDebugger;

[ExecuteInEditMode][SelectionBase]
public class LevelIcon : MonoBehaviour
{
    public int Level;
    private const int foreSpriteIndex = 0, levelIndex = 2;

    private void Update()
    {
        var n = name.Substring(12).ToString();
        n     = n.Substring(0, n.Length - 1);
        Level = n.Parse();

        transform.GetChild(levelIndex).Get<Text>().text = n;
    }
    public void OnClick()
    {
        Instantiate(Resources.Load("GameObjects\\Play UI"));
    }
}
