using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;
using UnityEngine.SceneManagement;
using static UnityEngine.SceneManagement.SceneManager;
using static PathDebugger;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] private GameObject _selector;
    public static GameObject Selector;
    public static LevelSelector Instance { get; private set; }

    private void Start()
    {
        Selector = _selector;
        Selector.SetPosition(SaveManager.SelectorPosition);
    }
    public static void SaveLoction() => SaveManager.SaveSelectorPosition(Selector.Position());
}
