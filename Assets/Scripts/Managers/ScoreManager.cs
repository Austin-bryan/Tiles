using UnityEngine;
using ExtensionMethods;
using static PathDebugger;

public class ScoreManager : MonoBehaviour
{
    public const int GoldPoints = 100;
    public int ChipPoints   { get; private set; }
    public int BreakPoints  { get; private set; }
    public int GoldCount    { get; set; }
    public int CurrentScore { get; private set; }

    public int GetGoldScore()           => ((GoldCount * GoldCount) - 1) * GoldPoints;
    private int GetGoldScore(int count) => ((count * GoldCount) - 1) * GoldPoints;
    public void UpdateScore()
    {
        int newScore = GetGoldScore();

        GoldCount = 0;

        if (newScore == 0) return;

        CurrentScore += newScore;
        // show score anim

    }
    public int GetMaxGoldScore() => GetGoldScore(Board.X * Board.Y);
    public void GetMaxBrickScore() { }
    public void SaveNewScore()
    {

    }
}
