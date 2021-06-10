using ExtensionMethods;
using System.Collections.Generic;
using static ParseManager;
using static PathDebugger;
using static Tiles.Parsing.ParseConstants;

namespace Tiles.Parsing
{
    public class ParameterParseState : ParseState
    {
        public static readonly List<string> ParsedParameters = new List<string>();
        public static bool IsGrid { get; private set; }

        private bool isInDelimiter;

        public ParameterParseState() => ExpectedMessage = "Parameter";

        // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Methods    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
        public override void Reset()
        {
            ParsedParameters.Clear();
            IsGrid = false;
        }
        public override void ParseSeperator() => Error(TileSeperator);
        public override void Parse(in string parameter)
        {
            if (parameter == "grid") IsGrid = true;
            else ParsedParameters.Add(parameter);

            isInDelimiter = false;
        }
        public override void ParseRightParen()
        {
            if (!isInDelimiter) ExitState(ParameterState);
            else base.ParseRightParen();
        }
        public override void ParseDelimiter()
        {
            if (ParsedParameters.Count == 0)
                base.ParseDelimiter();
            isInDelimiter = true;
        }
    }
}