using static Board;
using ExtensionMethods;
using System.Collections.Generic;
using static BoardManager;
using static Tiles.Modules.ModuleConstants;

namespace Tiles.Modules
{
    //todo: gap and wrap common ancestor
    public class WrapModule : TileModule
    {
        public override TileType TileType => TileType.Wall;
        public WrapModule(PlayerTile playerTile, List<string> parameters, bool oneSwipeOnly = false) : base(playerTile, parameters, WallIndex, oneSwipeOnly) { }

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
                else return wall.Sides.HasModule(TileType.Wall);
            }
        }
        public static Coord GetCoordOfWallToSpawnAt(Direction currentDirection)
        {
            var workingCoord = SwipeManager.ActiveTile.Coord;
            int size = currentDirection.IsHorizontal() ? Size.X : Size.Y;

            for (int i = 0; i < size; i++)
            {
                workingCoord -= currentDirection;

                if (!workingCoord.IsInFrame() || ((PlayerTile)PBoard[workingCoord]).Sides.HasModule(TileType.Wall))
                    break;
            }

            return workingCoord;
        }
    }
}