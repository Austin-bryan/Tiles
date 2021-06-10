using UnityEngine;
using ExtensionMethods;
using static PathDebugger;

public class BoardManager : MonoBehaviour
{
    // ---- Boards ---- //
    public static PlayerBoard  PBoard;
    public static TargetBoard  TBoard;
    public static CreatorBoard CBoard;

    // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Methods    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
    public void OnLevelWasLoaded()
    {
        if (TBoard != null) TBoard?.Begin();
        if (PBoard != null) PBoard?.Begin();
    }
}
