using UnityEngine;
using static TileType;
using ExtensionMethods;
using static PathDebugger;
using System.Collections.Generic;
using Parameters = System.Collections.Generic.Dictionary<ToggleButton, System.Collections.Generic.List<UnityEngine.GameObject>>;

public class ParameterGetter : MonoBehaviour
{
    public static ValueBox BrickCount, IronCount, SteelCount, AmethystCount;
    [SerializeField] ValueBox brickCount, ironCount, steelCount, amethystCount;
    [SerializeField] ToggleButton brickButton, ironButton, steelButton, amethystButton;

    private static Parameters parameters;

    private void Start()
    {
        parameters = new Parameters()
        {
            { brickButton,    new List<GameObject>() { brickCount.gameObject }},
            { ironButton,     new List<GameObject>() { ironCount.gameObject  }},
            { steelButton,    new List<GameObject>() { steelCount.gameObject }},
            { amethystButton, new List<GameObject>() { amethystCount.gameObject }},
        };
        (BrickCount, IronCount, SteelCount, AmethystCount) = (brickCount, ironCount, steelCount, amethystCount);
    }
    public static List<string> GetParameters()
    {
        var foundParameters = new List<string>();

        foreach (var item in parameters)
        {
            if (!item.Key.IsOn) continue;

            foreach (var parameter in item.Value)
                foundParameters.Add(parameter.Get<ValueBox>().Value.ToString());
        }
        return foundParameters;
    }
    public static List<string> GetParameters(TileType property)
    {
        switch (property)
        {
            case Brick:    return MakeList(BrickCount);
            case Iron:     return MakeList(IronCount);
            case Steel:    return MakeList(SteelCount);
            case Amethyst: return MakeList(AmethystCount);
            default:       return default;
        }

        // ---- Local Functions ---- //
        List<string> MakeList(ValueBox valueBox) => new List<string>() { GetStringValue(valueBox) };
    }
    private static string GetStringValue(ValueBox gameObj) => gameObj.Value.ToString();
}
