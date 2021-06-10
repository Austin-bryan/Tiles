using ExtensionMethods;
using static ParseManager;

namespace Tiles.Parsing
{
    public class MultiSideParseState : ParseState
    {
        public MultiSideParseState() => ExpectedMessage = "Angle";

        // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Methods    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
        public override void Reset() { }
        public override void ParseDelimiter() => FinishSide(false);
        public override void ParseRightAngle()
        {
            FinishSide(true);
            ExitMultiSideState(ColorState);
        }
    }
}
