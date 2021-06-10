using System;
using System.Text;
using System.Threading.Tasks;
using ExtensionMethods;
using static PathDebugger;
using static Tiles.Parsing.ParseConstants;

namespace Tiles.Parsing
{
    /// <summary>
    /// Converts shorthand parse notation into the long hand version
    /// </summary>
    public partial class ShorthandConverter
    {
        private string seed;
        private readonly int rowCount;

        public ShorthandConverter(string seed, int rowCount) => (this.seed, this.rowCount) = (seed, rowCount);

        public string Convert()
        {
            seed = seed.Substring(0, seed.Length - 1);  // Remove semi colon

            ConvertMultiple();
            ConvertRowAuto();

            if (seed[seed.Length - 1] == '/')
                seed = seed.Substring(0, seed.Length - 1);
            seed += ';';
            ConvertDirections();

            return seed;
        }
        private string ConvertRowAuto()         // Converts expressoins like 3 | r- into 3 | r/r/r/
        {
            var args = new ConverterArgs(seed);

            for (int i = 0; i < args.Temp.Length; i++)                           // Loop through the temporary seed
            {
                if (!(args.Temp[i] == '/' || args.Temp[i] == '-')) continue;     // Skip iteration if not '/' or '-'

                args.TileCount++;                                                // Since we know we are at a '/' or '-' that means we are parsing a new tile                                         
                if (args.Temp[i] == '/')    
                {
                    args.StartIndex = i + 1;
                    continue;
                }
                args.Finalize(i, Math.Abs(((args.TileCount - 1) % rowCount) - rowCount), 1, 1);     // Create the tiles to fill the remainding row
                args.PrepareForNextTile(args.StartIndex + args.Tiles.Length);
            }
            return MakeSeed(args);
        }
        private string ConvertMultiple()        // Converts expressions like r*2 into r/r/
        {
            int count;
            var args = new ConverterArgs(seed);

            for (int i = 0; i < args.Temp.Length; i++)              // Loop through the temporary seed
            {
                if (args.Temp[i] == '/' || args.Temp[i] == '-')     // New tile is beginning
                    args.PrepareForNextTile(i + 1);
                if (args.Temp[i] != '*') continue;                  // Skip iteration if not parsing '*'

                count = args.Temp[i + 1].Parse();                   // Get the number followed by '*', which represents how many tiles to use
                args.Temp.Remove(i, 2);                             // Remove '*' and number that follows
                args.Finalize(i, count, buffer: i == 1 ? 0 : 1, builderBuffer: 0, count: count);    // Adds the new tiles
            }
            return MakeSeed(args);
        }
        private string ConvertDirections()
        {
            var temp = new StringBuilder(seed.RemoveWhiteSpace());
            const string directions = "[h, v]";

            int counter = 0;

            for (int i = 1; i < temp.Length && counter < 100; i++)
            {
                counter++;

                if (!(temp[i] == '/' || temp[i] == ';')) continue;
                if (temp[i - 1] == ']') continue;

                temp.Insert(i, directions);
                i += directions.Length;
            }

            return seed = temp.ToString();
        }
        private string MakeSeed(ConverterArgs args) => seed = args.Temp.ToString();
    }
} // 115, 154, 112, 69