using System.Collections.Generic;
using ObstructState = PlayerTile.ObstructionState;

public partial class PlayerTile
{
    public abstract class MoveMode
    {
        public bool IsTemporary { get; set; }
        public Coord NewCoord { get; protected set; }
        public ObstructState ObstructionState { get; protected set; }

        protected static bool hasBeenObstructed;
        protected static List<PlayerTile> MovementGroup;  // These tiles will all move along the same path
        protected bool? playerTriggered;
        protected PlayerTile tile, tileMovingInto;

        // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Methods    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
        public MoveMode(PlayerTile tile) => this.tile = tile;

        /// Intitations the movement of all the tiles in the group, and checks to make sure the movement is legal
        public virtual void InitiateTilesInMovementGroup(Direction direction, bool playerTriggered) { }
        /// Each tile individually acquires the data they need for the move, such as where they will go, and turns on the update function 
        public virtual void StartMovement(Direction direction, bool? playerTriggered, bool shouldSpawnOppositeTile)  { }
        /// Drive the movement each frame
        public virtual void UpdateMovement() { }
        /// Finishes the movement and executes any duties the components or other factors dictate, otherwise, snaps tile in place.
        public virtual void FinishMovement() { }
        
        protected virtual void GetTiles(Direction direction, ref List<PlayerTile> tiles) { }
    }
}