using UnityEngine;
using UnityEngine.UI;
using static PathDebugger;

public class CheckBox : MonoBehaviour
{
    public Sprite Locked, Unlocked;
    public Button Button;
    public bool IsChecked { get; set; } = false;

    public void OnClicked()
    {
        BoardGenerator.ForceSquare = IsChecked = !IsChecked;
        BoardGenerator.UpdateSize(true);
        Button.image.sprite = IsChecked ? Locked : Unlocked;
    }
}
