using ExtensionMethods;
using Tiles.Components;
using System.Collections.Generic;
using static Tiles.Components.ComponentConstants;

namespace Tiles.Factories
{
    using TC    = TileComponent;
    using TF    = TileFactory;
    using PTile = PlayerTile;

    public class BalloonFactory    : TF { public override TC GetComponent(PTile parent, List<string> param, bool isGrid) => new BalloonComponent    (parent, param, isGrid); }
    public class CamoFactory       : TF { public override TC GetComponent(PTile parent, List<string> param, bool isGrid) => new CamoComponent       (parent, param, isGrid); }
    public class IceFactory        : TF { public override TC GetComponent(PTile parent, List<string> param, bool isGrid) => new IceComponent        (parent, param, isGrid); }
    public class IronFactory       : TF { public override TC GetComponent(PTile parent, List<string> param, bool isGrid) => new IronComponent       (parent, param, param[0].Parse(), isGrid); }
    public class SteelFactory      : TF { public override TC GetComponent(PTile parent, List<string> param, bool isGrid) => new SteelComponent      (parent, param, param[0].Parse(), isGrid); }
    public class AmethystFactory   : TF { public override TC GetComponent(PTile parent, List<string> param, bool isGrid) => new AmethystComponent   (parent, param, param[0].Parse(), isGrid); }
    public class BrickFactory      : TF { public override TC GetComponent(PTile parent, List<string> param, bool isGrid) => new IronComponent       (parent, param, param[0].Parse(), isGrid); }
    public class NailFactory       : TF { public override TC GetComponent(PTile parent, List<string> param, bool isGrid) => new NailComponent       (parent, param, isGrid); }
    public class GapFactory        : TF { public override TC GetComponent(PTile parent, List<string> param, bool isGrid) => new GapComponent        (parent, param, isGrid); }
    public class WallFactory       : TF { public override TC GetComponent(PTile parent, List<string> param, bool isGrid) => new WallComponent       (parent, param, isGrid); }
    public class ScrewFactory      : TF { public override TC GetComponent(PTile parent, List<string> param, bool isGrid) => new ScrewComponent      (parent, param, isGrid); }
    public class BoltFactory       : TF { public override TC GetComponent(PTile parent, List<string> param, bool isGrid) => new BoltComponent       (parent, param, isGrid); }
    public class DiagonalFactory   : TF { public override TC GetComponent(PTile parent, List<string> param, bool isGrid) => new DiagonalComponent   (parent, param, isGrid); }
    public class LinkFactory       : TF { public override TC GetComponent(PTile parent, List<string> param, bool isGrid) => new LinkComponent       (parent, param, isGrid); }
    public class WarpFactory       : TF { public override TC GetComponent(PTile parent, List<string> param, bool isGrid) => new WarpComponent       (parent, param, param[0].Parse(), isGrid); }
    public class RotateFactory     : TF { public override TC GetComponent(PTile parent, List<string> param, bool isGrid) => new RotateComponent     (parent, param, param[0].Parse(), param[1].ToRotateDirection(), param.Count > 2 && param[2] == "mini", isGrid); }
    public class ThisSideUpFactory : TF { public override TC GetComponent(PTile parent, List<string> param, bool isGrid) => new ThisSideUpComponent (parent, param, isGrid); }
    public class ChessFactory      : TF { public override TC GetComponent(PTile parent, List<string> param, bool isGrid) => new ChessComponent      (parent, param, param[0], param.Count > 1 ? param[1] : "x", isGrid); }
    public class PawnFactory       : TF { public override TC GetComponent(PTile parent, List<string> param, bool isGrid) => new PawnComponent       (parent, param, param[0], param.Count > 1 ? param[1] : "x", isGrid); }
    public class RookFactory       : TF { public override TC GetComponent(PTile parent, List<string> param, bool isGrid) => new RookComponent       (parent, param, param[0], param.Count > 1 ? param[1] : "x", isGrid); }
    public class KnightFactory     : TF { public override TC GetComponent(PTile parent, List<string> param, bool isGrid) => new KnightComponent     (parent, param, param[0], param.Count > 1 ? param[1] : "x", isGrid); }
    public class BishopFactory     : TF { public override TC GetComponent(PTile parent, List<string> param, bool isGrid) => new BishopComponent     (parent, param, param[0], param.Count > 1 ? param[1] : "x", isGrid); }
    public class KingFactory       : TF { public override TC GetComponent(PTile parent, List<string> param, bool isGrid) => new KingComponent       (parent, param, param[0], param.Count > 1 ? param[1] : "x", isGrid); }
    public class QueenFactory      : TF { public override TC GetComponent(PTile parent, List<string> param, bool isGrid) => new QueenComponent      (parent, param, param[0], param.Count > 1 ? param[1] : "x", isGrid); }
    public class HybridFactory     : TF { public override TC GetComponent(PTile parent, List<string> param, bool isGrid) => new HybridComponent     (parent, param, isGrid); }
    public class OutletFactory     : TF { public override TC GetComponent(PTile parent, List<string> param, bool isGrid) => new OutletComponent     (parent, param, isGrid); }
    public class LightbulbFactory  : TF { public override TC GetComponent(PTile parent, List<string> param, bool isGrid) => new LightbulbComponent  (parent, param, LightbulbIndex, isGrid); }
    public class TVFactory         : TF { public override TC GetComponent(PTile parent, List<string> param, bool isGrid) => new TVComponent         (parent, param, TVIndex, isGrid); }
    public class TabletFactory     : TF { public override TC GetComponent(PTile parent, List<string> param, bool isGrid) => new TabletComponent     (parent, param, TabletIndex, isGrid); }
}