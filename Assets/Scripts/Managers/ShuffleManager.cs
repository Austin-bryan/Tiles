using UnityEngine;
using System.Collections;
using static LayerType;
using static PathDebugger;
using static BoardManager;

public class ShuffleManager : MonoBehaviour
{
    public static int ShuffleCount { get; private set; }
    public static bool IsShuffling        = false;

    private const float shuffleDelay      = 0.15f;
    private static int swipesRemaining    = 1;
    private static LayerType curLayerType = Column;
    private static Direction curDirection, lastDirection;
    private static ShuffleManager instance;
    private static bool shouldShuffle = false;

    private class ShuffleManagerKey : ShuffleManager, IKey { }

    public void Start()
    {
        if (shouldShuffle)
        {
            ShuffleCount = ParseManager.ShuffleCount;
            StartCoroutine(Shuffle(0.5f));
        }
        else EndShuffle();

        // ---- Local Functions ---- //
        IEnumerator Shuffle(float time)
        {
            yield return new WaitForSeconds(time);
            BeginShuffleFromInstance();
        }
    }
    public void Update() => instance = this;
    public static void BeginShuffle() => instance.BeginShuffleFromInstance();

    private void BeginShuffleFromInstance()
    {
        IsShuffling     = true;
        //IsShuffling     = false;
        swipesRemaining = ShuffleCount;
        //SwipeManager.ShuffleFinish();

        StartCoroutine(SwipeRandomLayer(shuffleDelay));

        IEnumerator SwipeRandomLayer(float time)
        {
            yield return new WaitForSeconds(time);

            PlayerTile playerTile;

            do playerTile = PBoard.Get(Coord.Random());
            while (!playerTile.IsMovable);

            SwipeManager.SetActiveTile<ShuffleManagerKey>(playerTile);

            lastDirection = curDirection;
            curDirection  = DirectionUtilities.Random(curLayerType);
            curLayerType  = curLayerType.GetOppositeType();

            playerTile?.BeginSwipe(curDirection, false);
            swipesRemaining--;

            if (swipesRemaining > 0) StartCoroutine(SwipeRandomLayer(time));
            else EndShuffle();
        }
    }
    private static void EndShuffle()
    {
        IsShuffling = false;
        SwipeManager.ShuffleFinish();
    }
}
