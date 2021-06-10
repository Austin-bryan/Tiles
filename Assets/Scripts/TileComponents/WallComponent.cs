using static Board;
using ExtensionMethods;
using static BoardManager;
using System.Collections.Generic;
using static Tiles.Components.ComponentConstants;

namespace Tiles.Components
{
    //todo: gap and hole common ancestor
    public class WallComponent : TileComponent
    {
        public override TileType TileType => TileType.Wall;
        public WallComponent(PlayerTile playerTile, List<string> parameters, bool oneSwipeOnly = false) : base(playerTile, parameters, WallIndex, oneSwipeOnly) { }

        public override void SetVisibility(bool isVisible)
        {
            base.SetVisibility(isVisible);
            Tile.IsMovable = !isVisible;
        }
        public static PlayerTile GetNearestWall(PlayerTile tile, Direction currentDirection)
        {
            var coord       = tile.Coord;
            int loopCounter = 0;
            PlayerTile wall = null;

            do coord -= currentDirection;
            while (tile.Coord.IsInFrame() && loopCounter++ < LargestSize && !tileIsWall() == true);

            return wall;

            // ---- Local Functions ---- //
            bool? tileIsWall()
            {
                wall = (PlayerTile)PBoard[coord];

                if (wall == null) return null;
                else return wall.Sides.HasComponent(TileType.Wall);
            }
        }
        public static Coord GetCoordOfWallToSpawnAt(Direction currentDirection)
        {
            var workingCoord = SwipeManager.ActiveTile.Coord;
            int size = currentDirection.IsHorizontal() ? Size.X : Size.Y;

            for (int i = 0; i < size; i++)
            {
                workingCoord -= currentDirection;

                if (!workingCoord.IsInFrame() || ((PlayerTile)PBoard[workingCoord]).Sides.HasComponent(TileType.Wall))
                    break;
            }

            return workingCoord;
        }
    }
}