using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static PathDebugger;

public class NextLevelManager : MonoBehaviour
{
    private bool nextHasBeenPressed;

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.N) == true && nextHasBeenPressed == false)
        {
            nextHasBeenPressed = true;
            Path3();
            LevelManager.CurrentLevel++;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
