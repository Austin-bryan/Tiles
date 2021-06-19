using ExtensionMethods;
using Tiles.Modules;
using System.Collections.Generic;
using static Tiles.Modules.ModuleConstants;

namespace Tiles.Factories
{
    using TC    = TileModule;
    using TF    = TileFactory;
    using PTile = PlayerTile;

    public class BalloonFactory    : TF { public override TC GetModule(PTile parent, List<string> param, bool isGrid) => new BalloonModule    (parent, param, isGrid); }
    public class CamoFactory       : TF { public override TC GetModule(PTile parent, List<string> param, bool isGrid) => new CamoModule       (parent, param, isGrid); }
    public class IceFactory        : TF { public override TC GetModule(PTile parent, List<string> param, bool isGrid) => new IceModule        (parent, param, isGrid); }
    public class IronFactory       : TF { public override TC GetModule(PTile parent, List<string> param, bool isGrid) => new IronModule       (parent, param, param[0].Parse(), isGrid); }
    public class AmethystFactory   : TF { public override TC GetModule(PTile parent, List<string> param, bool isGrid) => new AmethystModule   (parent, param, param[0].Parse(), isGrid); }
    public class BrickFactory      : TF { public override TC GetModule(PTile parent, List<string> param, bool isGrid) => new IronModule       (parent, param, param[0].Parse(), isGrid); }
    public class NailFactory       : TF { public override TC GetModule(PTile parent, List<string> param, bool isGrid) => new NailModule       (parent, param, isGrid); }
    public class GapFactory        : TF { public override TC GetModule(PTile parent, List<string> param, bool isGrid) => new GapModule        (parent, param, isGrid); }
    public class WallFactory       : TF { public override TC GetModule(PTile parent, List<string> param, bool isGrid) => new WrapModule       (parent, param, isGrid); }
    public class ScrewFactory      : TF { public override TC GetModule(PTile parent, List<string> param, bool isGrid) => new ScrewModule      (parent, param, isGrid); }
    public class BoltFactory       : TF { public override TC GetModule(PTile parent, List<string> param, bool isGrid) => new BoltModule       (parent, param, isGrid); }
    public class DiagonalFactory   : TF { public override TC GetModule(PTile parent, List<string> param, bool isGrid) => new DiagonalModule   (parent, param, isGrid); }
    public class LinkFactory       : TF { public override TC GetModule(PTile parent, List<string> param, bool isGrid) => new LinkModule       (parent, param, isGrid); }
    public class WarpFactory       : TF { public override TC GetModule(PTile parent, List<string> param, bool isGrid) => new WarpModule       (parent, param, param[0].Parse(), isGrid); }
    public class ThisSideUpFactory : TF { public override TC GetModule(PTile parent, List<string> param, bool isGrid) => new ThisSideUpModule (parent, param, isGrid); }
    public class ChessFactory      : TF { public override TC GetModule(PTile parent, List<string> param, bool isGrid) => new ChessModule      (parent, param, param[0], param.Count > 1 ? param[1] : "x", isGrid); }
    public class PawnFactory       : TF { public override TC GetModule(PTile parent, List<string> param, bool isGrid) => new PawnModule       (parent, param, param[0], param.Count > 1 ? param[1] : "x", isGrid); }
    public class RookFactory       : TF { public override TC GetModule(PTile parent, List<string> param, bool isGrid) => new RookModule       (parent, param, param[0], param.Count > 1 ? param[1] : "x", isGrid); }
    public class KnightFactory     : TF { public override TC GetModule(PTile parent, List<string> param, bool isGrid) => new KnightModule     (parent, param, param[0], param.Count > 1 ? param[1] : "x", isGrid); }
    public class BishopFactory     : TF { public override TC GetModule(PTile parent, List<string> param, bool isGrid) => new BishopModule     (parent, param, param[0], param.Count > 1 ? param[1] : "x", isGrid); }
    public class KingFactory       : TF { public override TC GetModule(PTile parent, List<string> param, bool isGrid) => new KingModule       (parent, param, param[0], param.Count > 1 ? param[1] : "x", isGrid); }
    public class QueenFactory      : TF { public override TC GetModule(PTile parent, List<string> param, bool isGrid) => new QueenModule      (parent, param, param[0], param.Count > 1 ? param[1] : "x", isGrid); }
    public class HybridFactory     : TF { public override TC GetModule(PTile parent, List<string> param, bool isGrid) => new EightWayModule     (parent, param, isGrid); }
    public class OutletFactory     : TF { public override TC GetModule(PTile parent, List<string> param, bool isGrid) => new OutletModule     (parent, param, isGrid); }
    public class LightbulbFactory  : TF { public override TC GetModule(PTile parent, List<string> param, bool isGrid) => new LightbulbModule  (parent, param, LightbulbIndex, isGrid); }
    public class TVFactory         : TF { public override TC GetModule(PTile parent, List<string> param, bool isGrid) => new TVModule         (parent, param, TVIndex, isGrid); }
    public class TabletFactory     : TF { public override TC GetModule(PTile parent, List<string> param, bool isGrid) => new TabletModule     (parent, param, TabletIndex, isGrid); }
}