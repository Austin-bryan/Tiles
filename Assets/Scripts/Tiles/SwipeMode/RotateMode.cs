using ExtensionMethods;
using System.Collections.Generic;
using ObstructState = PlayerTile.ObstructionState;
using static BoardManager;
using static PathDebugger;

public partial class PlayerTile
{
    public class RotateMode : MoveMode
    {
        protected int rotationDegree = 4;
        public PlayerTile rotatingTile;
        public PlayerTile Tile => tile;

        public RotateMode(PlayerTile tile, int rotationDegree) : base(tile) =>
            this.rotationDegree = rotationDegree;

        public void BeginRotate(RotateDirection direction, bool wasPlayerTriggered) 
        {
            tile.NullCheck();
            List<PlayerTile> rotationTiles = new List<PlayerTile>()
            {
                GetTile( 1,  0),
                GetTile(-1,  0),
                GetTile( 0,  1),
                GetTile( 0, -1),
            };

            foreach (var rotationTile in rotationTiles)
            {
                rotationTile.SetRotationCenter(tile.Position());
                rotationTile.isBeingRotated = true;
                rotationTile.RotatingMode.rotatingTile = tile;
            }

            PlayerTile GetTile(int deltaX, int deltaY) => (PlayerTile)PBoard[tile.Coord - new Coord(deltaX, deltaY)];
        }
        public void Rotate(RotateDirection direction, bool? wasPlayerTriggered)
        {
            tile.isBeingRotated = true;
        }
        public void FinishRotate() 
        {

        }
    }
}