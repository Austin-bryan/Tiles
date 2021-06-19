using Tiles.Modules;
using System.Collections.Generic;
using static TileType;
using static PathDebugger;

namespace Tiles.Factories
{
    public abstract class TileFactory
    {
        public abstract TileModule GetModule(PlayerTile parentTile, List<string> parameters, bool isGrid = false);

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
                case Bolt:       return new BoltFactory();
                case Amethyst:   return new AmethystFactory();
                case Diagonal:   return new DiagonalFactory();
                case Link:       return new LinkFactory();
                case Warp:       return new WarpFactory();
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
