using System.Collections.Generic;
using ObstructState = PlayerTile.ObstructionState;

public partial class PlayerTile
{
    public abstract class  MoveMode
    {
        public bool Temporary { get; set; }
        public Coord NewCoord { get; protected set; }
        public ObstructState ObstructionState { get; protected set; }

        protected static bool hasBeenObstructed;
        protected bool? playerTriggered;
        protected PlayerTile tile, tileMovingInto;

        // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Methods    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
        public MoveMode(PlayerTile tile) => this.tile = tile;

        public virtual void BeginSwipe(bool playerTriggered) { }
        public virtual void FinishSwipe() { }
        
        protected virtual void GetTiles(Direction direction, ref List<PlayerTile> tiles) { }
    }
}