using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;
using static PathDebugger;

public class ObjectToggle : MonoBehaviour
{
    public GameObject[] GameObject;

    public void Toggle() => GameObject.ToList().ForEach(x => x.SetActive(!x.activeSelf));
}
