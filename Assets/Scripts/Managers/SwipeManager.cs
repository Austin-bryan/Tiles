using System;
using UnityEngine;
using Tiles.Modules;
using ExtensionMethods;
using System.Collections;
using System.Collections.Generic;
using static PathDebugger;

public class SwipeManager : MonoBehaviour
{
    private static MonoBehaviour instance;

    [SerializeField]
    public static event Action AllMovesFinished, GlobalMoveFinished, swag;
    public static Direction CurrentDirection;
    public static PlayerTile ActiveTile { get; set; }
    public static List<TileModule> ModulesSweptThisSwipe = new List<TileModule>();

    private static List<(PlayerTile Tile, TileModule tileModule, Direction Direction)> directionsDue = new List<(PlayerTile, TileModule, Direction)>();
    private static List<TileModule> finishSwipeNotify = new List<TileModule>();
    private static Coord? currentCoord;

    private MonoBehaviour _instance;

    // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Methods    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
    public void Start() => instance = this;
    public void Update() { if (SwipeDetector.IsSwiping) ActiveTile?.IntiateSwipe(SwipeDetector.SwipeDir, true); }

    public static void SetActivated(TileModule component) => ModulesSweptThisSwipe.Add(component);
    public static void ShuffleFinish() { }
    public static void FinishTurn()
    {
        InGameUI.UpdateLimiter();
        instance.StartCoroutine(globalMoveFinished(0.3f));

        if (directionsDue.Count <= 0)
        {
            instance.StartCoroutine(allMovesFinished(0.3f));
            return;
        }
        instance.StartCoroutine(SwipeAgain(0.1f));

        TurnManager.FinishTurn();

        // ---- Local Functions ---- //
        static IEnumerator allMovesFinished(float time)
        {
            yield return new WaitForSeconds(time);

            AllMovesFinished?.Invoke();
        }
        static IEnumerator globalMoveFinished(float time)
        {
            yield return new WaitForSeconds(time);
            GlobalMoveFinished?.Invoke();
        }
        IEnumerator SwipeAgain(float time)
        {
            yield return new WaitForSeconds(time);
            int i = 0;

            string s = "";
            directionsDue.ForEach(d => s += d.Tile.ID.ToString() + ", ");
            s = s.Remove(s.Length - 2);

            while (i < directionsDue.Count && directionsDue[i].Tile == null)
                directionsDue.RemoveAt(i);

            if (!directionsDue[i].Tile.IsSwipeObstructed())
            {
                directionsDue[i].tileModule?.OnQueuedSwipeCalled();
                directionsDue[i].Tile.IntiateSwipe(directionsDue[i].Direction, false);
                directionsDue.RemoveAt(i);
            }
            if (directionsDue.Count < 1) ModulesSweptThisSwipe.Clear();
        }
    }
    public static void BeginSwipe(Direction direction)
    {
        CurrentDirection = direction;
        currentCoord     = ActiveTile?.Coord;
    }
    public static void Replace(PlayerTile currentTile, PlayerTile newTile)
    {
        if (currentTile == ActiveTile) ActiveTile = newTile;

        foreach (var index in directionsDue.Index())
        {
            if (directionsDue[index.index].Tile != currentTile) continue;

            directionsDue[index.index] = (newTile, directionsDue[index.index].tileModule, directionsDue[index.index].Direction);
        }
    }
    public static void AddDirection(PlayerTile tile, Direction direction, TileModule component = null)
    {
        directionsDue.Add((tile, component, direction));
        //tile.SwipingMode.SwipeQueueIndexes.Add(directionsDue.Count - 1);
    }
    public static void SetActiveTile<TKey>(PlayerTile tile) where TKey : ShuffleManager, IKey => ActiveTile = tile;
}