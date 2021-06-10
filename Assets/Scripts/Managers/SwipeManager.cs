using System;
using UnityEngine;
using Tiles.Components;
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
    public static List<TileComponent> ComponentsSweptThisSwipe = new List<TileComponent>();

    private static List<(PlayerTile Tile, TileComponent tileComponent, Direction Direction)> directionsDue = new List<(PlayerTile, TileComponent, Direction)>();
    private static List<TileComponent> finishSwipeNotify = new List<TileComponent>();
    private static Coord? currentCoord;

    private MonoBehaviour _instance;

    // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Methods    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
    public void Start() => instance = this;
    public void Update()
    {
        if (SwipeComponent.IsSwiping) ActiveTile?.SwipingMode.BeginSwipe(SwipeComponent.SwipeDir, true);
    }

    public static void SetActivated(TileComponent component) => ComponentsSweptThisSwipe.Add(component);
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
        IEnumerator allMovesFinished(float time)
        {
            yield return new WaitForSeconds(time);

            AllMovesFinished?.Invoke();
        }
        IEnumerator globalMoveFinished(float time)
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
                directionsDue[i].tileComponent?.OnQueuedSwipeCalled();
                directionsDue[i].Tile.BeginSwipe(directionsDue[i].Direction, false);
                directionsDue.RemoveAt(i);
            }
            if (directionsDue.Count < 1) ComponentsSweptThisSwipe.Clear();
        }
    }
    public static void BeginSwipe(Direction direction)
    {
        CurrentDirection = direction;
        currentCoord     = ActiveTile?.Coord;
    }
    public static void Replace(PlayerTile currentTile, PlayerTile newTile)
    {
        var s = new List<(int, string)>();

        if (currentTile == ActiveTile) ActiveTile = newTile;

        foreach (var index in directionsDue.Index())
        {
            if (directionsDue[index.index].Tile != currentTile) continue;

            directionsDue[index.index] = (newTile, directionsDue[index.index].tileComponent, directionsDue[index.index].Direction);
        }
    }
    public static void AddDirection(PlayerTile tile, Direction direction, TileComponent component = null)
    {
        directionsDue.Add((tile, component, direction));
        tile.SwipingMode.SwipeQueueIndexes.Add(directionsDue.Count - 1);
    }
    public static void SetActiveTile<TKey>(PlayerTile tile) where TKey : ShuffleManager, IKey => ActiveTile = tile;
}