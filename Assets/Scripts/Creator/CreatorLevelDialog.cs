using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ExtensionMethods;
using static PathDebugger;

public class CreatorLevelDialog : MonoBehaviour
{
    // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Fields    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //

    public bool DragEnabled { get; set; }
    public Vector3 Offset   { get; private set; }
    public enum DialogType  { Open, Save, Seed }
    public DialogType Type;
    public Text text;

    private Vector3 startPos;

    // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Methods    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //

    private void Start()
    {
        startPos = transform.position;

        if (Type == DialogType.Open)
            ShowText();
    }

    public void SetDragEnabled(bool value)
    {
        DragEnabled = value;

        if (value) Offset = transform.position - Input.mousePosition;
    }
    public void Show(bool value)
    {
        gameObject.SetActive(value);

        if (value) ResetPos();
    }
    public void Cancel()    => Show(false);
    public void ClearText() => text.text = "";
    public void ShowText()  => text.text = $"Enter Level 0 - {SaveManager.GetCreatorLevelCount()}";
    private void ResetPos() => transform.position = new Vector3(500, 385);

}
