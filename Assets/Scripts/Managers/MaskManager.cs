using UnityEngine;
using ExtensionMethods;
using static PathDebugger;
using static Board;

public class MaskManager : MonoBehaviour
{
    public SpriteMask Mask;

    private const int scaler = 170;
    private static MaskManager instance;

    public void Start()
    {
        instance = this;

        if (Size.X != Size.Y)
             Mask.transform.localScale = shortSideIsX() ? scale(getNewScale(), Mask.ScaleY()) : scale(Mask.ScaleX(), getNewScale());
        else Mask.transform.localScale = scale(scaler, scaler);

        // ---- Local Functions ---- //
        bool shortSideIsX() => SmallestSize == Size.X;
        float getNewScale() => Mask.ScaleX() / (SmallestSize / 1.8f);
        Vector3 scale(float a, float b) => new Vector3(a, b);
    }
    public static void Restart() => instance.Start();
}