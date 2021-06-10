using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ExtensionMethods;
using static PathDebugger;

[ExecuteInEditMode]
public class OutlineButton : MonoBehaviour
{
    public Button Button;
    public Text   Text;
    public Color  Color;
    public Sprite HoverSprite;
    public Sprite UnhoverSprite { get; set; }
    public Color  HoverTextColor;

    private bool shouldUpdate = true;

    public void Start()
    {
        UnhoverSprite = Button.GetSprite();
    }

    public void Update()
    {
        //if (shouldUpdate) OnUnhover();
    }

    public void OnHover()
    {
        shouldUpdate = false;
        ShowSprite(HoverSprite);
        SetTextColor(true);
    }
    public void OnUnhover()
    {
        ShowSprite(UnhoverSprite);
        SetTextColor(false);
    }

    private void ShowSprite(Sprite sprite)
    {
        Button.SetSprite(sprite);
        Button.SetColor(Color);
    }
    private void SetTextColor(bool isHovered) => Text.color = isHovered ? HoverTextColor : Color;
}
