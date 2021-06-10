using UnityEngine;
using System.Linq;
using ExtensionMethods;
using System.Collections.Generic;
using static Direction;
using static PathDebugger;
using static RotateDirection;
using static Tiles.Components.ComponentConstants;

namespace Tiles.Components
{
    public class RotateComponent : TileComponent
    {
        public override TileType TileType   => TileType.Rotate;
        public override bool HasMiniVersion => true;
        public int rotateNumber;

        private readonly bool isMini;
        private readonly RotateDirection direction;
        private readonly List<PlayerTile> connectedTiles;
        private readonly List<RotateComponent> componentsToUpdate = new List<RotateComponent>();
        private List<Coord> connectedCoords = new List<Coord>();

        // ==================== Methods ==================== //
        public override List<string> Parameters { get => new List<string>() { rotateNumber.ToString(), direction.MakeString() }; protected set => base.Parameters = value; }
        public Vector3 Position { get; private set; }

        public RotateComponent(PlayerTile playerTile, List<string> parameters, int rotateNumber, RotateDirection direction, bool isMini, bool oneSwipeOnly = false) : base(playerTile, parameters, new[] { RotateIndex, (int)direction - 1 }, oneSwipeOnly)
        {
            (this.rotateNumber, this.direction) = (rotateNumber, direction);
            connectedTiles = new List<PlayerTile>();

            this.isMini = isMini;

            if (isMini || rotateNumber < 4) return;

            Tile.Delay(0.1f, SetupCoords);
        }
    
        public override void Swipe(Direction direction, bool? wasPlayerTriggered, bool shouldSpawnOpposite, bool wasObstructed)
        {
            Tile.Delay(0.05f, foo);
            void foo() => SetupCoords();
        }
        public void UpdateConnectedTiles()
        {
            connectedTiles.Clear();
            connectedCoords.ForEach(c => connectedTiles.Add(BoardManager.PBoard[c].To<PlayerTile>()));
        }
        public void SetUpConnectedTiles(params Coord[] coords)
        {
            var orderedCoords = new Coord[coords.Length];
            connectedCoords.Clear();

            if (coords.Length > 2 && isMini)
            {
                int smallestX = coords[0].X;
                int smallestY = coords[0].Y;

                orderedCoords[0] = coords[0];

                for (int i = 1; i < coords.Length; i++)
                {
                    if (coords[i].X == smallestX)
                         orderedCoords[1] = coords[i];
                    else if (coords[i].Y != smallestY)
                         orderedCoords[2] = coords[i];
                    else orderedCoords[3] = coords[i];
                }
                if (direction == Clockwise) orderedCoords = orderedCoords.Reverse().ToList().ToArray();
            }
            else orderedCoords = coords;

            connectedCoords = orderedCoords.ToList();
            UpdateConnectedTiles();
        }
        public override void Activate(bool wasPlayerTriggered)
        {
            base.Activate(wasPlayerTriggered);

            if (Tile.IsRotateObstructed()) return;

            if (rotateNumber == 1) 
            { 
                Tile.transform.eulerAngles = Tile.transform.eulerAngles + new Vector3(0, 0, 90 * (direction == Clockwise ? -1 : 1));
                CallOnRotated();
                return; 
            }
            else
            {
                SetupCoords();
                if (rotateNumber > connectedCoords.Count) return;

                foreach (var tile in connectedTiles)
                    if (tile.IsRotateObstructed()) return;
            }

            CallOnRotated();

            Path5();

            Tile.RotatingMode.BeginRotate(direction, wasPlayerTriggered);

            //var rotationParent = new GameObject();

            //rotationParent.SetParent(BoardManager.PBoard.gameObject);
            //rotationParent.AddComponent<Rotator>();
            //rotationParent.SetPosition(Tile.Position());
            //rotationParent.Get<Rotator>().Angle = (rotateNumber == 2 ? 180 : 90) * (direction == Clockwise ? -1 : 1);
            //rotationParent.Get<Rotator>().Direction = direction;

            //if (connectedCoords[connectedCoords.Count - 1] == Tile.Coord)
            //{
            //    connectedCoords.RemoveAt(connectedCoords.Count - 1);
            //    rotationParent.Get<Rotator>().HasCenter = true;
            //}
            //if (rotateNumber == 8 || rotateNumber == 9)
            //{
            //    rotationParent.Get<Rotator>().SkippingNumber = 2;
            //}
            //rotationParent.Get<Rotator>().Coords = connectedCoords;

            //foreach (var (i, tile) in connectedTiles.Index())
            //    tile.SetParent(rotationParent);

            // Local Functions
            void CallOnRotated() => Tile.Sides.CurrentSideComponents.ForEach(c => c.OnWasRotated(wasPlayerTriggered));
        }
        
        private void SetupCoords()
        {
            // Adds coords of the tiles that surround the rotator
            var coordsList = new List<Coord>();
        
            foreach (var dir in new[] { UpLeft, Up, UpRight, Right, DownRight, Down, DownLeft, Left, Direction.None })
                AddCoord(ref coordsList, dir);
        
            SetUpConnectedTiles(coordsList.ToArray());

            // ---- Local Functions ---- //
            void AddCoord(ref List<Coord> coords, Direction dir)
            {
                var coord = Tile.Coord + dir;

                if (!coord.IsInFrame() || (dir.IsDiagonal() && rotateNumber < 8)) return;
                if (dir == Direction.None && !(rotateNumber == 5 || rotateNumber == 9)) return;

                coords.Add(coord);
            }
        }
    }
}//116, 97