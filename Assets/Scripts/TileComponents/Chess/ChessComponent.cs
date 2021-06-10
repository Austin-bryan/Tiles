using System;
using UnityEngine;
using ExtensionMethods;
using static ChessPiece;
using static PathDebugger;
using System.Collections.Generic;
using static Tiles.Components.ComponentConstants;

public enum ChessColor { White, Black }
public enum ChessPiece { Pawn, Rook, Knight, Bishop, King, Queen }

namespace Tiles.Components
{
    public class ChessComponent : TileComponent
    {
        public override TileType TileType => throw new NotImplementedException();
        public ChessColor? Color { get; private set; }
        protected ChessPiece piece;

        private List<PlayerTile> modifiedTiles = new List<PlayerTile>();

        // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Methods    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
        public static ChessPiece GetPiece(string input)
        {
            switch (input)
            {
                case "p":  return Pawn;
                case "r":  return Rook;
                case "kn": return Knight;
                case "b":  return Bishop;
                case "ki": return King;
                case "q":  return Queen;

                default: throw new ArgumentException($"Chess piece string {input} was invalid");
            }
        }
        public ChessComponent(PlayerTile playerTile, List<string> parameters, string chessPiece, string color, bool oneSwipeOnly = false) : base(playerTile, parameters, ChessIndex, oneSwipeOnly)
        {
            piece = GetPiece(chessPiece);
            ShowChessPiece();
            ShowChessColor(color);
        }
        public override void ValidateSwipe(Direction direction, bool wasPlayerTriggered)
        {
            var coord = Tile.Coord + direction;
            var tile  = GetTile(coord);

            if (IsChess(tile))
            {
                if (IsSameColor(tile) && IsCloserTo(Tile, tile)) Tile.SetIsSwipeObstructed(true);
                else if (Tile.Coord + direction == tile.Coord) 
                    tile.Sides.RemoveComponent(TileType.Chess);
            }

            AddComponents(true);
            AddComponents(false);

            // ---- Local Functions ---- //
            bool IsChess(PlayerTile playerTile)     => playerTile?.Sides.HasComponent(TileType.Chess) ?? false;
            PlayerTile GetTile(Coord playerCoord)   => BoardManager.PBoard[playerCoord].To<PlayerTile>();
            bool IsSameColor(PlayerTile playerTile) => playerTile?.Get(TileType.Chess).To<ChessComponent>().Color == Color;
            void AddComponents(bool isPositive)
            {
                coord = SwipeManager.ActiveTile.Coord;

                while (true)
                {
                    coord += direction.Times(isPositive ? 1 : -1);

                    if (!coord.IsInFrame()) break;
                    else tile = GetTile(coord);

                    tile = GetTile(coord);

                    if (tile == Tile || !IsChess(tile) || IsCloserTo(tile, Tile)) continue;

                    tile.CreateAndAddComponent(tile.IsEdge() ? TileType.Wall : TileType.Gap);
                    modifiedTiles.Add(tile);
                }
            }
            bool IsCloserTo(PlayerTile a, PlayerTile b)
            {
                bool? result = SwipeManager.ActiveTile.Coord.IsCloserTo(a.Coord, b.Coord, direction);

                if (result == true)  return true;
                if (result == false) return false;

                var c = SwipeManager.ActiveTile.Coord;
                while (true)
                {
                    c += direction;

                    if (!c.IsInFrame()) return false;
                    if (a.Coord == c)   return true;
                }
            }
        }
        public override void FinishSwipe(bool isNewTile)
        {
            base.FinishSwipe(isNewTile);

            Tile.Delay(0f, ResetTiles);

            // ---- Local Functions ---- //
            void ResetTiles()
            {
                modifiedTiles.ForEach(t => t.Sides.RemoveComponent(TileType.Gap));
                modifiedTiles.ForEach(t => t.Sides.RemoveComponent(TileType.Wall));
                modifiedTiles.Clear();
            }
        }
        public override void SetVisibility(bool isVisible)
        {
            isVisible.Log();
            base.SetVisibility(isVisible);

            Tile.Get<SpriteRenderer>(ChessIndex).enabled = isVisible;

            if (isVisible) ShowChessPiece();
            else HideChessPiece();
        }
        private void ShowChessColor(string color)
        {
            const int white = 245;
            const int black = 20; 

            Tile.Get<SpriteRenderer>(ChessIndex).color = color == "w" ? new Color32(white, white, white, 255) :
                                                               color == "b" ? new Color32(black, black, black, 255) :
                                                                              new Color32(0, 0, 0, 0);
            Color = color == "w" ? ChessColor.White : color == "b" ? (ChessColor?)ChessColor.Black : null; 
        }
        private void ShowChessPiece() => Tile.Get<SpriteRenderer>().sprite = TileSprites.GetChessSprite(piece);
        private void HideChessPiece() => Tile.Get<SpriteRenderer>().sprite = TileSprites.Normal;
    }
}
public static class ChessExt
{
    public static TileType ToTileType(this ChessPiece piece)
    {
        switch (piece)
        {
            case Pawn:   return TileType.Pawn;
            case Rook:   return TileType.Rook;
            case Knight: return TileType.Knight;
            case Bishop: return TileType.Bishop;
            case King:   return TileType.King;
            case Queen:  return TileType.Queen;

            default:     return TileType.Pawn;
        }
    }
    public static ChessPiece ToChessPiece(this TileType piece)
    {
        switch (piece)
        {
            case TileType.Pawn:   return Pawn;
            case TileType.Rook:   return Rook;
            case TileType.Knight: return Knight;
            case TileType.Bishop: return Bishop;
            case TileType.King:   return King;
            case TileType.Queen:  return Queen;

            default: return Pawn;
        }
    }
}