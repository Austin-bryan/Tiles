using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;
using UnityEngine.SceneManagement;
using static PathDebugger;

public class InGameMenu : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject exitPanel;

    private bool _menuIsOpen;
    private bool MenuIsOpen
    {
        get => _menuIsOpen;
        set
        {
            _menuIsOpen = value;
            menuPanel.Get<Animator>().SetBool("ShouldOpen", value);
        }
    }

    // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Methods    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
    public void Restart() => SceneManager.LoadScene("Level");
    public void Shuffle()
    {

    }
    public void ToggleOpen() => ToggleOpen(!MenuIsOpen, true);
    public void Exit() => ToggleOpen(false, false);

    private void ToggleOpen(bool menuOpen, bool exitPanelActive)
    {
        MenuIsOpen = menuOpen;
        exitPanel.SetActive(exitPanelActive);
    }

    public void ExitToLevelSelector() => SceneManager.LoadScene("LevelSelector");
}
