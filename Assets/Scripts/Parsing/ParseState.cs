using System;
using System.Linq;
using Tiles.Factories;
using ExtensionMethods;
using Tiles.Modules;
using System.Collections.Generic;
using static TileType;
using static PathDebugger;
using static ParseManager;
using static Tiles.Parsing.ParseConstants;
using static Tiles.Parsing.PropertyParseState;
using static Tiles.Parsing.ParameterParseState;
using static ExpandedFlowControl.LambdaExt;

namespace Tiles.Parsing
{
    public abstract class ParseState
    {
        protected string ExpectedMessage = "";
        private static List<ParseState> parseStates;
        private static bool delete;

        // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Methods    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
        public abstract void Reset();

        #region ( Parse Methods )
        public virtual void Parse(in string str) { }
        public virtual void ParseAccessor()     => Error(MemberAccessor);
        public virtual void ParseDelimiter()    => Error(Delimiter);
        public virtual void ParseLeftBrace()    => Error(LeftBrace);
        public virtual void ParseLeftParen()    => Error(LeftParen);
        public virtual void ParseLeftAngle()    => Error(LeftAngle);
        public virtual void ParseSemiColon()    => L (CreateModule, ResetAll);
        public virtual void ParseRightBrace()   => Error(RightBrace);
        public virtual void ParseRightParen()   => Error(RightParen);
        public virtual void ParseRightAngle()   => Error(RightAngle);
        public virtual void ParseLeftBracket()  => Error(LeftBracket);
        public virtual void ParseRightBracket() => Error(RightBracket);
        public virtual void ParseSeperator()
        {
            CreateModule();
            ParseNextTile();
            ResetAll();
        }
        protected void ParseNextTile() { }
        protected void Error(char token) => throw new ArgumentException($"Invalid token '{token}'. {ExpectedMessage} expected. Context: {Context}");
        #endregion ( Parse Methods )

        public static void ResetAll()
        {
            if (parseStates == null) parseStates = new List<ParseState>() { ColorState, PropertyState, ParameterState, DirectionState }; 

            parseStates.ForEach(s => s.Reset());
            ParseManager.Reset();
        }
      
        protected void CreateModule()
        {
            if (CurrentProperty == null) return;

            var type = TileTypeParseKey[CurrentProperty];

            if (type == Chess && ParsedParameters.Count > 0)
            {
                string chessType = ParsedParameters[0];
                delete = true;

                var piece = ChessModule.GetPiece(chessType);
                type = piece.ToTileType();
            }

            var factory = TileFactory.GetFactory(type);
            if (factory == null || CurrentTile.Sides.HasModule(type)) return;

            var component = factory.GetModule(CurrentTile, ParsedParameters, IsGrid);

            CurrentTile.AddModule(type, component);
            CurrentTile.AddModules();

            ParameterState.Reset();
            PropertyState.Reset();
        }
        protected void FinishSide(bool isFinalSide)
        {
            ParseSeperator();

            var tile = CurrentTile.To<PlayerTile>();

            if (!isFinalSide)
            {
                 tile.Sides.AddSide();
                 tile.Sides.NextSide();
            }
            else tile.Sides.SetSidesToOneSwipe();

            tile.Sides.Count.Log();
        }
        protected void ExitStates(params ParseState[] states) => states.ForEach(s => ParseManager.ExitState(s));
        protected void ExitMultiSideState(params ParseState[] states) => ExitState(MultiSideState, () => Error(RightAngle), states);

        private void ExitState(ParseState removingState, Action errorCode, ParseState[] states)
        {
            var list = states.ToList();

            list.Add(removingState);
            states = list.ToArray();

            if (!CurrentStates.Contains(removingState))
                errorCode();
            else ExitStates(states);
        }
    }
}