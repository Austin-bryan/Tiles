using System;
using UnityEngine;
using static ColorExt;
using static TileType;
using static PathDebugger;

public abstract class Tile : GridSlot 
{
    // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Properties      ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
    protected TileColor _color;
    public virtual TileColor Color
    {
        get => _color;
        set => (_color, GetComponent<SpriteRenderer>().color) = (value, value.ToColor());
    }

    // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Fields    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //    
    public Sprite Sprite;
    public static int LastColor = 1, MaxColorCount = 3, ColorDifficulty = 3;
    public static TileColor PrevColor;
    protected bool shouldColor = false;

    private const bool forceMaxColor = true;
    private static int colorUsesRemaining = 0;
    private static TileColor currentColor;

    // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Methods    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
    public void MatchColor(Tile tile) => Color = tile.Color;
    public static GameObject CreateTile<T>() where T : Tile => Instantiate(LoadTile<T>()) as GameObject;

    private static void GetColor(out TileColor color) => color = (TileColor)UnityEngine.Random.Range(1, MaxColorCount + 1);
    private static GameObject LoadTile<T>() where T : Tile => Resources.Load(GetName<T>()) as GameObject;

    private static string GetName<T>() where T : Tile
    {
        if (IsType<TargetTile>()) return GetPathName<TargetTile>();
        return GetPathName<PlayerTile>();

        // ---- Local Functions ---- //
        string GetPathName<U>()
        {
            bool useFolderLocation = true;
            string location = "GameObjects/";

            return $"{(useFolderLocation ? location : "")}{typeof(U)}";
        }
        bool IsType<V>() => typeof(T) == typeof(V);
    }
}//88