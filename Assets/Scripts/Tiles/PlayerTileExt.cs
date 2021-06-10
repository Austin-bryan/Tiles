﻿using static TileType;
using static MoveType;
using Tiles.Components;
using static PathDebugger;

public static class PlayerTileExt
{
    public static bool HasGap(this PlayerTile tile)  => tile?.Sides.HasComponent(Gap)  ?? false; 
    public static bool HasWall(this PlayerTile tile) => tile?.Sides.HasComponent(Wall) ?? false;
    public static bool Has(this PlayerTile tile, TileType type) => tile?.Sides.HasComponent(type) ?? false;
    
    public static bool IsSwipeObstructed(this PlayerTile tile)  => tile.ObstructionStates.IsSwipeObstructed;
    public static bool IsRotateObstructed(this PlayerTile tile) => tile.ObstructionStates.IsRotateObstructed;
    public static bool IsWarpObstructed(this PlayerTile tile)   => tile.ObstructionStates.IsWarpObstructed;
    
    public static TileComponent Get(this PlayerTile tile, TileType type) => tile.Sides.GetComponent(type);

    public static void SetIsSwipeObstructed(this PlayerTile tile, bool value)
    {
        if (value) tile.ObstructionStates.ObstructMove(Swipe);
        else tile.ObstructionStates.UnobstructMove(Swipe);
    }
    public static void SetIsRotateObstructed(this PlayerTile tile, bool value)
    {
        if (value) tile.ObstructionStates.ObstructMove(MoveType.Rotate);
        else tile.ObstructionStates.UnobstructMove(MoveType.Rotate);
    }
    public static void SetIsWarpObstructed(this PlayerTile tile, bool value)
    {
        if (value) tile.ObstructionStates.ObstructMove(MoveType.Warp);
        else tile.ObstructionStates.UnobstructMove(MoveType.Warp);
    }
}