using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ExtensionMethods;
using static PathDebugger;

public class LoadCreator : MonoBehaviour
{
    public void Load() => SceneManager.LoadScene(LevelManager.CurrentLevel);
}
