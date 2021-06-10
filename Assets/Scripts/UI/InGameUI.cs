using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;
using UnityEngine.UI;
using static PathDebugger;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private Text _limiterText;
    private static Text limiterText;

    private void Start()
    {
        limiterText = _limiterText;
        UpdateLimiter();
    }
    public static void UpdateLimiter() => limiterText.text = TurnManager.MovesRemaining.ToString();
}
