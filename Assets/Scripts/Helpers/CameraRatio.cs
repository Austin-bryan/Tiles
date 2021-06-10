using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;
using static PathDebugger;

public class CameraRatio : MonoBehaviour
{
    private static readonly float newX = 480, newY = 800;

    public void Start()
    {
        GetComponent<Camera>().aspect = newX / newY;
        Screen.SetResolution(Screen.resolutions[6].width, Screen.resolutions[6].height, true);
        GetComponent<Camera>().ResetAspect();
    }
}
