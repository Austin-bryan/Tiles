using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;
using UnityEngine.UI;
using static PathDebugger;
using static ExpandedFlowControl.IfExt;

[ExecuteInEditMode]
public class PageTab : MonoBehaviour
{
    private bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (value && selectedTab != null) selectedTab.IsSelected = false;

            _isSelected = value;
            image.color = (value ? selectedColor : deselectedColor);
            image.SetLocalScale(value ? selectedScale : deselectedScale);

            if (value) selectedTab = this;
        }
    }

    // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Fields    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
    [SerializeField] Image image; 
    [SerializeField] bool _isSelected;
    [SerializeField] Color selectedColor;
    private static PageTab selectedTab;
    private const float scale = 0.9f;
    private const byte color  = 53;
    private readonly Color deselectedColor   = new Color32(color, color, color, 255);
    private readonly Vector2 selectedScale   = new Vector2(1, 1);
    private readonly Vector2 deselectedScale = new Vector2(scale, scale);

    // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Methods    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
    private void Start()
    {
        IsSelected = IsSelected;
        if (IsSelected) selectedTab = this;
    }
    public void Select() => If (true, () => IsSelected = true);
}
