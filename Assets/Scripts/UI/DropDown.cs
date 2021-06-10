using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;
using static PathDebugger;

public class DropDown : MonoBehaviour
{
    public float SizeX, SizeY;

    public void OnOpen()
    {
        var child = transform?.GetChild(4);
        if (child == null) return;

        child.GetComponent<RectTransform>().sizeDelta = new Vector2(SizeX, SizeY);
    }
}
