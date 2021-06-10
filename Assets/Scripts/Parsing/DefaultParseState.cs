using ExtensionMethods;
using static ParseManager;
using static PathDebugger;
using System.Collections.Generic;

namespace Tiles.Parsing
{
    public class DefaultParseState : ParseState
    {
        public static readonly List<string> ParsedParameters = new List<string>();
        public DefaultParseState() => ExpectedMessage = "";

        // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Methods    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
        public override void Reset() => ParsedParameters.Clear();
        public override void Parse(in string str) { }
        public override void ParseLeftBracket() => EnterState(DirectionState);
        public override void ParseLeftAngle()   => EnterState(MultiSideState);
        public override void ParseRightAngle()
        {
            FinishSide(true);
            ExitMultiSideState(ColorState);
        }
    }
}
