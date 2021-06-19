using System.Collections.Generic;
using static TileColor;
using static GameMode;
using static TileType;
using static Direction;
using static SwipeType;

namespace Tiles.Parsing
{
    public static class ParseConstants
    {
        public static readonly string[] TestData = new[]
        {
            "7 | x*2/b*3/z*2/ g- y.ca/z/r*3/z/o.ca/ y/z/r.ic/r.{ca, ir.10}/r.ic/z/o/ y/z/r*3/z/o/ g- z*2/b*3/x*2/",
            "3 | y/r- b- g/y-;",
            "6 | r/r/r/r/r/r/g/g/g/g/g/g/b/b/b/b/b/b/y/y/y/y/y/y/y/pi/pi/pi/pi/pi/pi/y/y/y/y/y/y;",
            "5 | r.ir.10- g.ir.10- b.ir.10- y- pi-",
            "3 | y*3/ b-g/g/g;",
            "3 | y*3/b*3/o*3;",
            "3 | y/y/y/ b/b/b/ o/o/o;",
            "3 | r.ir.10/r.ir.10/r.ir.10/ g.ir.10/g.ir.10/g.ir.10/ b.ir.10/b.ir.10/b.ir.10;",
            "3 | y/y/y/ b/b/b/ o/o/o;",
            "5 | y*5/ b/b/b/b/b/ o/o/o/o/o/ p/p/p/p/p/ r/r/r/r/r;",
            "5 | y/y/y/y/y/ b/b/b/b/b/ o/o/o/o/o/ p/p/p/p/p/ r/r/r/r/r;",
        };
        public static readonly char[] Tokens = new[] { MemberAccessor, LeftBrace, RightBrace, LeftParen, RightParen, Delimiter, TileSeperator, SemiColon, LeftAngle, RightAngle };

        // ---- Tokens ---- //
        public const char LeftBrace   = '{', RightBrace   = '}', MemberAccessor   = '.', TileSeperator = '/';
        public const char LeftParen   = '(', RightParen   = ')', Delimiter        = ',', SemiColon     = ';';
        public const char LeftBracket = '[', RightBracket = ']', LevelInfoDivider = '|', Multiplier    = '*';
        public const char LeftAngle   = '<', RightAngle   = '>';

        // ---- Property Names ---- //
        public const string NormalTile  = "",   CamoTile   = "ca", BrickTile    = "br", IronTile       = "ir";
        public const string BalloonTile = "ba", CementTile = "ce", MagnetTile   = "ma", GapTile        = "x";
        public const string HoleTile    = "z",  IceTile    = "ic", NailTile     = "n",  ScrewTile      = "s";
        public const string SteelTile   = "st", BoltTile   = "bo", AmethystTile = "am", DiagonalTile   = "d";
        public const string LinkTile    = "l",  WarpTile   = "wa", RotateTile   = "ro", ThisSideUpTile = "up";
        public const string ChessTile   = "ch", HybridTile = "h",  OutletTile   = "ol", LightbulbTile  = "lb";
        public const string TVTile      = "tv", TabletTile = "tl";

        // ---- Parse Keys ---- //
        public static readonly Dictionary<string, TileType> TileTypeParseKey   = new Dictionary<string, TileType>()
        {
            [NormalTile]  = Normal, [GapTile]     = Gap,    [HoleTile]     = Wall,     [CamoTile]       = Camo,
            [BrickTile]   = Brick,  [CementTile]  = Cement, [IronTile]     = Iron,     [BalloonTile]    = Balloon,
            [IceTile]     = Ice,    [MagnetTile]  = Magnet, [NailTile]     = Nail,     [ScrewTile]      = Screw,
            [SteelTile]   = Steel,  [BoltTile]    = Bolt,   [AmethystTile] = Amethyst, [DiagonalTile]   = Diagonal,
            [LinkTile]    = Link,   [WarpTile]    = Warp,   [RotateTile]   = Rotate,   [ThisSideUpTile] = ThisSideUp,
            [ChessTile]   = Chess,  [HybridTile]  = Hybrid, [OutletTile]   = Outlet,   [LightbulbTile]  = Lightbulb,
            [TVTile]      = TV,     [TabletTile]  = Tablet,
        };
        public static readonly Dictionary<string, TileColor> TileColorParseKey = new Dictionary<string, TileColor>()
        {
            ["w"]   = White,        ["r"]  = Red,        ["b"]   = Blue,     ["g"]  = Green,       ["y"]   = Yellow,      ["o"]  = Orange,
            ["c"]   = Cyan,         ["p"]  = Purple,     ["m"]   = Magenta,  ["gr"] = Grey,        ["lr"]  = LightRed,    ["lb"] = LightBlue,
            ["lg"]  = LightGreen,   ["ly"] = LightYellow,["lpi"] = LightPink,["lo"] = LightOrange, ["lc"]  = LightCyan,   ["lp"] = LightPurple,
            ["lm"]  = LightMagenta, ["bk"] = Black,      ["dr"]  = DarkRed,  ["db"] = DarkBlue,    ["dg"]  = DarkGreen,   ["dy"] = DarkYellow,
            ["dpi"] = DarkPink,     ["do"] = DarkOrange, ["dc"]  = DarkCyan, ["dp"] = DarkPurple,  ["dm"]  = DarkMagenta, ["z"]  = WallColor,
            ["x"]   = GapColor,     ["pi"] = Pink
        };
        public static readonly Dictionary<string, GameMode>  GameModeParseKey  = new Dictionary<string, GameMode>()  { [""]   = Moves, ["Move"] = Moves, ["Fire"] = Fire, ["Time"] = Timed, ["Brick"] = Bricks, };
        public static readonly Dictionary<string, Direction> DirectionParseKey = new Dictionary<string, Direction>() { ["u"]  = Up,    ["d"]    = Down,  ["l"]    = Left, ["r"]    = Right };
        public static readonly Dictionary<string, SwipeType> SwipeTypeParseKey = new Dictionary<string, SwipeType>() { ["bl"] = SwipeType.None, ["br"] = Bronze, ["s"]  = Silver, ["g"]  = Gold };
    }
} // 147, 123, 70