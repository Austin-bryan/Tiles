using System;
using ExtensionMethods;
using static ParseManager;
using static PathDebugger;
using static Tiles.Parsing.ParseConstants;

namespace Tiles.Parsing
{
    public class PropertyParseState : ParseState
    {
        private bool isInDelimter;

        public static string CurrentProperty { get; protected set; }
        public PropertyParseState() => ExpectedMessage = "Property";

        // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Methods    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
        public override void Reset() => CurrentProperty = "";

        public override void Parse(in string property)
        {
            CurrentProperty = property;
            isInDelimter = false;
        }
        public override void ParseLeftParen()
        {
            if (isInDelimter) base.ParseLeftParen();
            EnterState(ParameterState);
        }
        public override void ParseDelimiter()
        {
            if (CurrentProperty == "")
                Error(Delimiter);

            isInDelimter = true;

            CreateComponent();
            Reset();
        }
        public override void ParseSeperator()  => Error(TileSeperator);
        public override void ParseRightBrace()
        {
            if (isInDelimter) base.ParseRightBrace();
            else
            {
                CreateComponent();
                ExitState(PropertyState);
            }
        }
        public static void AddColorAsProperty(string str)
        {
            if (str == GapTile || str == HoleTile)
                CurrentProperty = str;
        }
    }
}
