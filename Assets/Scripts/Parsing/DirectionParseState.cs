using System;
using ExtensionMethods;
using System.Collections.Generic;
using static ParseManager;
using static PathDebugger;
using static Tiles.Parsing.ParseConstants;

namespace Tiles.Parsing
{
    public class DirectionParseState : ParseState
    {
        public static readonly List<string> ParsedDirections = new List<string>();
        private bool isInDelimiter;

        public DirectionParseState() => ExpectedMessage = "Directions";

        // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Methods    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
        public override void Parse(in string str)
        {
            isInDelimiter = false;

            if (DirectionParseKey.ContainsKey(str)) ParsedDirections.Add(str);
            else if (str == "v") ParsedDirections.AddRange("u", "d");
            else if (str == "h") ParsedDirections.AddRange("l", "r");
            else throw new ArgumentException($"Inputted value \"{str}\" was not a direction");
        }

        public override void Reset()             => ParsedDirections.Clear();
        public override void ParseDelimiter()
        {
            if (ParsedDirections.Count == 0) base.ParseDelimiter();
            isInDelimiter = true;
        }
        public override void ParseSeperator()    => Error(TileSeperator);
        public override void ParseRightBracket()
        {
            if (isInDelimiter) base.ParseRightBracket();
            else FinalizeDirections();
        }
        protected void FinalizeDirections()
        {
            if (ParsedDirections.Count == 0)
                ParsedDirections.AddRange("u", "d", "l", "r");

            var directions = new List<Direction>();

            foreach (var dir in ParsedDirections)
                directions.Add(DirectionParseKey[dir]);

            CurrentTile.SetDirections(directions);
            ExitState(DirectionState);
        }
    }
}
