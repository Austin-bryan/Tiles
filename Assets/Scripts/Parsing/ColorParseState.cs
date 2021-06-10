using System;
using ExtensionMethods;
using System.Collections.Generic;
using static ParseManager;
using static PathDebugger;
using static Tiles.Parsing.ParseConstants;
using static Tiles.Parsing.PropertyParseState;
using static ExpandedFlowControl.LambdaExt;

namespace Tiles.Parsing
{
    public class ColorParseState : ParseState
    {
        public static readonly List<string> ParsedColors = new List<string>();
        public ColorParseState() => ExpectedMessage = "Color name";

        // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Methods    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
        public override void ParseSeperator()
        {
            FinalizeColor();
            ExitState(ColorState);
            base.ParseSeperator();
        }
        public override void Parse(in string s) => ParsedColors.Add(s);
        public override void Reset()            => ParsedColors.Clear();
        public override void ParseSemiColon()   => L(FinalizeColor, base.ParseSemiColon);
        public override void ParseLeftBracket()
        {
            FinalizeColor(); 
            ExitState(ColorState);
            EnterState(DirectionState);
        }
        public override void ParseLeftBrace()
        {
            FinalizeColor();
            ExitState(ColorState);
            EnterState(PropertyState);
        }
        public override void ParseRightAngle()
        {
            FinishSide(true);
            ExitMultiSideState(ColorState);
        }
        public override void ParseDelimiter()
        {
            if (!CurrentStates.Contains(MultiSideState))
            {
                base.ParseDelimiter();
                return;
            }
            FinishSide(false);
            ExitState(ColorState);
        }
        protected void FinalizeColor()
        {
            AddColorAsProperty(ParsedColors[0]);

            if (TileColorParseKey.ContainsKey(ParsedColors[0]))
                 CurrentTile.To<PlayerTile>().Color = TileColorParseKey[ParsedColors[0]];
        }
    }
}