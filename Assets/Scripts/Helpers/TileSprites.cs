using UnityEngine;
using ExtensionMethods;
using serial = UnityEngine.SerializeField;

public class TileSprites:  MonoBehaviour
{
    [Header("Chess")]
    [serial] Sprite pawn;
    [serial] Sprite rook;
    [serial] Sprite knight;
    [serial] Sprite bishop;
    [serial] Sprite king;
    [serial] Sprite queen;

    [Header("Misc")]
    [serial] Sprite hybrid;
    [serial] Sprite normal;

    [Header("Battery")]
    [serial] Sprite battery1;
    [serial] Sprite battery2;
    [serial] Sprite battery3;
    [serial] Sprite battery4;
    [serial] Sprite battery5;

    private static TileSprites instance;

    // ---- Chess ---- //
    public static Sprite Pawn   => instance.pawn;
    public static Sprite Rook   => instance.rook;
    public static Sprite Knight => instance.knight;
    public static Sprite Bishop => instance.bishop;
    public static Sprite King   => instance.king;
    public static Sprite Queen  => instance.queen;

    // ---- Misc ---- //
    public static Sprite Hybrid => instance.hybrid;
    public static Sprite Normal => instance.normal;

    // ---- Battery ---- //
    public static Sprite Battery1 => instance.battery1;
    public static Sprite Battery2 => instance.battery2;
    public static Sprite Battery3 => instance.battery3;
    public static Sprite Battery4 => instance.battery4;
    public static Sprite Battery5 => instance.battery5;

    // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Methods    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
    private void Awake() => instance = this;

    public static Sprite GetChessSprite(ChessPiece piece)
    {
        switch (piece)
        {
            case ChessPiece.Pawn:   return Pawn;
            case ChessPiece.Rook:   return Rook; 
            case ChessPiece.Knight: return Knight;
            case ChessPiece.Bishop: return Bishop;
            case ChessPiece.King:   return King; 
            case ChessPiece.Queen:  return Queen;

            default: return Pawn;
        }
    }
    public static Sprite GetBatterySprite(int battery)
    {
        switch (battery)
        {
            case 1: return Battery1;
            case 2: return Battery2;
            case 3: return Battery3;
            case 4: return Battery4;
            case 5: return Battery5;

            default: return null;
        }
    }
}
