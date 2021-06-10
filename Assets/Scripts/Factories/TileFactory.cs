using static TileType;
using Tiles.Components;
using static PathDebugger;
using System.Collections.Generic;

namespace Tiles.Factories
{
    public abstract class TileFactory
    {
        public abstract TileComponent GetComponent(PlayerTile parentTile, List<string> parameters, bool isGrid = false);

        public static TileFactory GetFactory(TileType tileType)
        {
            switch (tileType)
            {
                case Camo:       return new CamoFactory();
                case Iron:       return new IronFactory();
                case Brick:      return new BrickFactory();
                case Balloon:    return new BalloonFactory();
                case Ice:        return new IceFactory();
                case Nail:       return new NailFactory();
                case Gap:        return new GapFactory();
                case Wall:       return new WallFactory();
                case Screw:      return new ScrewFactory();
                case Steel:      return new SteelFactory();
                case Bolt:       return new BoltFactory();
                case Amethyst:   return new AmethystFactory();
                case Diagonal:   return new DiagonalFactory();
                case Link:       return new LinkFactory();
                case Warp:       return new WarpFactory();
                case Rotate:     return new RotateFactory();
                case ThisSideUp: return new ThisSideUpFactory();
                case Chess:      return new ChessFactory();
                case Pawn:       return new PawnFactory();
                case Rook:       return new RookFactory();
                case Knight:     return new KnightFactory();
                case Bishop:     return new BishopFactory();
                case King:       return new KingFactory();
                case Queen:      return new QueenFactory();
                case Hybrid:     return new HybridFactory();
                case Outlet:     return new OutletFactory();
                case Lightbulb:  return new LightbulbFactory();
                case TV:         return new TVFactory();
                case Tablet:     return new TabletFactory();

                default: return default;
            }
        }
    }
}
