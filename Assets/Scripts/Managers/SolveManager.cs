using UnityEngine;
using ExtensionMethods;
using static PathDebugger;

public class SolveManager : MonoBehaviour
{
    public static bool IsSolved => !correctTiles.Contains(false);
    public static SolveManager Instance;

    private static bool[,] correctTiles;

    public bool this[Coord coord]
    {
        get => correctTiles[coord.X - 1, coord.Y - 1];
        set => correctTiles[coord.X - 1, coord.Y - 1] = value;
    }

    private void Start()
    {
        Instance = this;
        correctTiles = new bool[Board.Size.X, Board.Size.Y];

        for (int i = 0; i < correctTiles.GetLength(0); i++)
            for (int j = 0; j < correctTiles.GetLength(1); j++)
                correctTiles[i, j] = true;
    }
    //private void Update() => IsSolved.Log("Solved");
}