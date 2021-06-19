using System;
using UnityEngine;
using UnityEngine.UI;
using ExtensionMethods;
using static PathDebugger;
using static ParseManager;

public class SeedCreator : MonoBehaviour
{
    public static string Seed { get; private set; }
    [SerializeField] InputField seedText;
    private static InputField SeedText;

    public static void GetSeedText(GameObject levelSeed)
    {
        levelSeed.SetActive(false);
        SeedText = levelSeed.transform.GetChild(4).Get<InputField>();
    }
    public void updateSeed() => UpdateSeed();
    public static void UpdateSeed() => SeedText.text = Seed = GetSeed();
    public static string GetSeed()
    {
        Board.Size = new Coord(GetValue(0).Parse(), GetValue(1).Parse());
        return $"{GetBoardSeed()} | {GetTileSeed()};";

        // ---- Local Functions ---- //
        string GetBoardSeed() => $"{Board.Size.X}x{Board.Size.Y}, {GetValue(2)}, {GetValue(3)}, m";
        string GetTileSeed()
        {
            string seed   = "";
            string spacer = " ";
            int x = 0, y = 1;

            foreach (var tile in CreatorBoard.Tiles)
            {
                if (tile == null) continue;

                seed += $"/{ReverseParse(tile)}";
                x++;

                if (x == Board.Size.X && y != Board.Size.Y)
                {
                    x = 0;
                    y++;
                    seed += spacer;
                }
            }
            return seed?.Substring(1);

            // ---- Nested Local Functions ---- //
            string ReverseParse(CreatorTile tile)
            {
                string parsedType = ReverseParseModules(tile.Modules);

                if (parsedType == "x" || parsedType == "z") return parsedType;
                if (parsedType == "") return ReverseParseColor(tile.Color);

                return $"{ReverseParseColor(tile.Color)}{parsedType}";
            }
        }
        string GetValue(int index) => CreatorManager.ValueBoxes[index].Value.ToString();
    }
}
