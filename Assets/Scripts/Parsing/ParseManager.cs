using Tiles;
using System;
using UnityEngine;
using System.Linq;
using Tiles.Parsing;
using Tiles.Modules;
using ExtensionMethods;
using System.Collections.Generic;
using static TileColor;
using static TileType;
using static GameMode;
using static SwipeType;
using static PathDebugger;
using static LevelManager;
using static Tiles.Parsing.ParseConstants;

public class ParseManager : MonoBehaviour
{
    // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Properties    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
    public static int Limiter       => GetLimiter(CurrentLevel);
    public static int ShuffleCount  => GetShuffleCount(CurrentLevel);
    public static Coord BoardSize   => GetBoardSize(CurrentLevel);
    public static GameMode GameMode => GetGameMode(CurrentLevel);
    public static string Context { get; private set; }

    #region { States }

    public static List<ParseState> CurrentStates  = new List<ParseState>() { new DefaultParseState() };
    public static PlayerTile CurrentTile    { get; set; }
    public static ParseState ColorState          { get; } = new ColorParseState();
    public static ParseState DefaultState        { get; } = new DefaultParseState();
    public static ParseState PropertyState       { get; } = new PropertyParseState();
    public static ParseState DirectionState      { get; } = new DirectionParseState();
    public static ParseState MultiSideState      { get; } = new MultiSideParseState();
    public static ParseState ParameterState      { get; } = new ParameterParseState();
    //public static ParseState MultiColorState     { get; } = new MultiColorParseState();
    //public static ParseState MultiPropertyState  { get; } = new MultiPropertyParseState();
    //public static ParseState MultiParameterState { get; } = new MultiParameterParseState();
    public static ParseState CurrentState => CurrentStates[CurrentStates.Count - 1];

    #endregion { States }

    private static string[] Tiles => GetTiles(CurrentLevel);

    // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Fields    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
    public static int CurrentStartTileIndex { get; set; }

    private static int currentTileIndex;
    private static char currentChar, nextChar;
    private static string parsedText = "";
    private static readonly List<PlayerTile> tiles = new List<PlayerTile>();
    private static (Coord Size, int Limiter, int ShuffleCount, GameMode GameMode) GetLevelInfo(int Level) => ParseLevelInfo(SplitLevelInfo(Level).LevelInfo);

    // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Methods    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
    // ---- Get Level Info ---- //
    public static int GetLimiter(int Level)       => GetLevelInfo(Level).Limiter;
    public static int GetShuffleCount(int Level)  => GetLevelInfo(Level).ShuffleCount;
    public static Coord GetBoardSize(int Level)   => GetLevelInfo(Level).Size;
    public static GameMode GetGameMode(int Level) => GetLevelInfo(Level).GameMode;

    //public static void ReturnToStartingPosition()       => currentTileIndex = CurrentStartTileIndex - 1;
    //public static TileType ParseType(string seed)       => TileTypeParseKey.Find(seed);
    //public static TileColor ParseColor(string seed)     => TileColorParseKey.Find(seed, Red);
    //public static SwipeType ParseSwipeType(string seed) => SwipeTypeParseKey.Find(seed, Bronze);
    //public static string GetTile(int level, int tile)   => GetTiles(level)[tile];
    public static void ResetState()
    {
        CurrentStates = new List<ParseState>();
        CurrentStates.Add(DefaultState);
    }
    public static void Reset(bool shouldAdd = true)
    {
        if (CurrentState != MultiSideState)
        {
            
            if (shouldAdd && CurrentTile != null)
            {
                tiles.Add(CurrentTile);
                CurrentTile.Sides.SetToDefault();
            }

            CurrentStartTileIndex = currentTileIndex;
            CurrentTile = Instantiate(Resources.Load("GameObjects/PlayerTile")).To<GameObject>().Get<PlayerTile>();
        }

        parsedText = "";
    }
    public static void EnterState(ParseState state)
    {
        CurrentStates.Add(state);
        //$"Enter {FormatState(state)}".Log();
        OnStateChange();
        //History.Add($"Enter {FormatState(state)}");
    }
    public static void ExitState(ParseState state)
    {
        CurrentStates.Remove(state);
        //$"Exit {FormatState(state)}".Log();
        OnStateChange();
        //History.Add($"Exit {FormatState(state)}");
    }
    public static List<PlayerTile> ParseTiles(string _tileData)
    {
        ResetState();
        string tileData = _tileData.RemoveWhiteSpace().Split('|')[1];
        Reset(shouldAdd: false);

        var converter = new ShorthandConverter(tileData, BoardSize.X);
        tileData = converter.Convert();
        tileData = tileData.Replace("/;", ";");
        tileData.RemoveWhiteSpace();
        tiles.Clear();

        for (currentTileIndex = 0; currentTileIndex < tileData.Length; currentTileIndex++)
        {
            if (currentTileIndex + 1 < tileData.Length)
                nextChar = tileData[currentTileIndex + 1];

            Context = GetContext();
            currentChar  = tileData[currentTileIndex];

            switch (currentChar)
            {
                case LeftBrace:      ProcessList(); CurrentState.ParseLeftBrace();    break;
                case LeftAngle:      ProcessList(); CurrentState.ParseLeftAngle();    break;
                case LeftParen:      ProcessList(); CurrentState.ParseLeftParen();    break;
                case SemiColon:      ProcessList(); CurrentState.ParseSemiColon();    break;
                case Delimiter:      ProcessList(); CurrentState.ParseDelimiter();    break;
                case RightBrace:     ProcessList(); CurrentState.ParseRightBrace();   break;
                case RightParen:     ProcessList(); CurrentState.ParseRightParen();   break;
                case RightAngle:     ProcessList(); CurrentState.ParseRightAngle();   break;
                case LeftBracket:    ProcessList(); CurrentState.ParseLeftBracket();  break;
                case RightBracket:   ProcessList(); CurrentState.ParseRightBracket(); break;
                case TileSeperator:  ProcessList(); CurrentState.ParseSeperator();    break;
                case MemberAccessor: ProcessList(); CurrentState.ParseAccessor();     break;
                case '-': break;
                default: parsedText += currentChar; break;
            }
            if ((CurrentState == DefaultState || CurrentState == MultiSideState) && TileColorParseKey.ContainsKey(parsedText))
                EnterState(ColorState);

            // ---- Local Functions ---- //
            string GetContext()
            {
                int frontChars, backChars;
                string str = "";

                backChars = currentTileIndex;
                if (backChars > 4) backChars = 4;

                frontChars = tileData.Count() - 1 - currentTileIndex;
                if (frontChars > 4) frontChars = 4;

                for (int i = currentTileIndex - 4; i < currentTileIndex; i++)
                    if (i > 0) str += tileData[i];

                for (int i = currentTileIndex; i <= currentTileIndex + frontChars; i++)
                    if (i < tileData.Length - 1) str += tileData[i];

                return str;
            }
        }

        return tiles;

        // ---- Local Functions ---- //
        void ProcessList()
        {
            if (parsedText == "") return;

            CurrentState.Parse(parsedText.RemoveWhiteSpace());    // We have a fully parsed item, now have CurrentState do something with it
            parsedText = "";
        }
    }

    private static void OnStateChange()
    {
        //"           BEGIN Current States".Log();
        //CurrentStates.ForEach(h => h.Log());
        //"           END Current States".Log();
        //History.ForEach(h => h.Log());
    }
    private static string FormatState(ParseState state) => state.ToString().Remove("Tiles.Parsing.").Remove("Parse");
    private static string[] GetTiles(int level) => SplitLevelInfo(level).TileInfo.Remove("-").Split('/');

    // ---- Parsing ---- //
    public static string ReverseParseColor(TileColor tileColor)
    {
        var swappedKey = TileColorParseKey.Swap();
        return TileColorParseKey.Swap().ContainsKey(tileColor) ? swappedKey[tileColor] : default;
    }
    public static string ReverseParseModules(List<TileModule> components)
    {
        var parseKey = TileTypeParseKey.Swap();
        return ReverseParseProperty();

        // ---- Local Functions ---- //
        // Converts components and their parameters into code
        string ReverseParseProperty() => ReverseParseItem(false, components, c => $".{parseKey[c.TileType]}{ReverseParseParameters(c)}", c => $"{parseKey[c.TileType]}{ReverseParseParameters(c)}", r => $".{{{r}}}");

        // Converts paramters into code
        string ReverseParseParameters(TileModule component) => ReverseParseItem(true, component.Parameters, p => $".{p}", p => p, r => $"({r})");
        string ReverseParseItem<T>(bool should, List<T> list, Func<T, string> formatSingle, Func<T, string> multiReverseFormat, Func<string, string> formatMulti)
        {
            string result = "";

            if (list == null || list.Count == 0) return "";
            if (list.Count == 1) return formatSingle(list[0]);

            foreach (var item in list)
                result += multiReverseFormat(item) + ", ";

            result = formatMulti(result.Substring(0, result.Length - 2));
            return result;
        }
    }
    public static string ReverseParseGameMode(GameMode gameMode)    => GameModeParseKey .Swap()[gameMode];
    public static string ReverseParseDirection(Direction direction) => DirectionParseKey.Swap()[direction];
    public static GameMode ParseGameMode(string seed)
    {
        if (GameModeParseKey.ContainsKey(seed)) 
            return GameModeParseKey[seed];
        return Moves;
    }

    private static List<Direction> ParseDirections(string tileDirections, TileType type)
    {
        var directions = new List<Direction>();
        CompileDirections();

        // For the Tile types where not writing any directions means all directions can be moved in, fill the directions fully
        if (directions.Count == 0 && !TileTypeShouldntUseArrows())
            FillDirections();

        return directions;

        #region <= Local Functions =>
        void CompileDirections()
        {
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                if (direction == Direction.None) continue;
                else if (TileContainsDirection(direction))
                    directions.Add(direction);
            }

            bool TileContainsDirection(Direction direction) => tileDirections?.Contains(ReverseParseDirection(direction)) ?? false;
        }
        bool TileTypeShouldntUseArrows() => type.HasFlag(Brick);
        void FillDirections()
        {
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
                directions.Add(direction);
        }
        #endregion <= Local Functions =>
    }
    private static (string LevelInfo, string TileInfo) SplitLevelInfo(int level)
    {
        string[] split = Levels[CurrentLevel].Split(LevelInfoDivider);

        return  (split[0].Trim(), split[1].Trim());
    }
    private static (Coord Size, int Limiter, int ShuffleCount, GameMode GameMode) ParseLevelInfo(string seed)
    {
        string[] split = seed.Split(',');
        GameMode gameMode = ParseGameMode(GetGameMode());

        return (ParseSize(), GetLimiter(), GetShuffleCount(), Moves/*, getParse(1), getParse(2), gameMode*/);

        // ---- Local Functions ---- //
        Coord ParseSize()
        {
            var (x, y) = split[0].Contains("x") ? split[0].SplitInTwo('x') : (split[0], split[0]);
            return new Coord((x, y).Parst());
        }
        string GetGameMode()
        {
            foreach (var x in GameModeParseKey.Keys.ToList())
                if (seed.Contains(x)) return x;
            return "";
        }
        int GetLimiter()
        {
            int limiterIndex = seed.IndexOf("(");

            if (limiterIndex == -1) return -1;

            string limiter = "";
            char c = ' ';
            int counter = 0;

            while (c != ')')
            {
                c = seed[limiterIndex++];

                if (counter++ > 100) return -1;
                
                if (c.IsInt())
                    limiter += c;
            }

            return limiter.Parse();
        }
        int GetShuffleCount()
        {
            int index = seed.IndexOf(',');
            char c = ' ';
            string shuffleCount = "";

            while (seed[++index] != ',' && index.InBounds(seed))
            {
                if (seed[index].IsInt())
                    shuffleCount += seed[index];
            }

            return shuffleCount.Parse();
        }
    }
}//319