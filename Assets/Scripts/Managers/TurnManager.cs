using UnityEngine;
using static PathDebugger;

public class TurnManager : MonoBehaviour
{
    public static bool ShouldSubtractMoves = true;
    private static int maxMoves;

    public static int MaxMoves
    {
        get => maxMoves;
        set => maxMoves = MovesRemaining = value;
    }
    public static int MovesRemaining { get; private set; }

    public void Awake() => MaxMoves = ParseManager.Limiter;
    public static void FinishTurn()
    {
        if (ShuffleManager.IsShuffling) return;
        
        if (ShouldSubtractMoves)
             MovesRemaining--;
        else ShouldSubtractMoves = true;
    }
}
