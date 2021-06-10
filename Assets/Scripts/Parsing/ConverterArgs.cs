using System.Text;
using ExtensionMethods;

namespace Tiles.Parsing
{
    public partial class ShorthandConverter
    {
        private class ConverterArgs
        {
            public int TileCount = 0, StartIndex = 0;
            public string Tile  { get; private set; } = "";
            public string Tiles { get; private set; } = "";
            public StringBuilder Temp { get; private set; }

            public ConverterArgs(string seed) => Temp = new StringBuilder(seed.RemoveWhiteSpace());

            public void PrepareForNextTile(int startIndex) => (Tile, Tiles, StartIndex) = ("", "", startIndex);
            public void Finalize(int i, int jMax, int buffer, int builderBuffer, int count = 0)
            {
                GetTile(StartIndex, i);
                MergeTiles(jMax, count);
                InsertCode(StartIndex, i, builderBuffer);
            }

            private string MergeTiles(int jMax, int count)
            {
                for (int j = 0; j < jMax; j++)
                {
                    Tiles += Tile;
                    if (j != count - 1)
                        Tiles += '/';
                }
                return Tiles;
            }
            private string GetTile(int tileStartIndex, int i)
            {
                for (int j = StartIndex; j < i; j++)
                {
                    if (Temp[j] == '-') break;
                    Tile += Temp[j];
                }
                return Tile;
            }
            public void InsertCode(int tileStartIndex, int i, int buffer, string code = "")
            {
                var builder = new StringBuilder(Temp.ToString());

                builder.Remove(StartIndex, i - StartIndex + buffer);
                builder.Insert(StartIndex, code == "" ? Tiles : code);

                Temp = new StringBuilder(builder.ToString());
            }
        }
    }
}
