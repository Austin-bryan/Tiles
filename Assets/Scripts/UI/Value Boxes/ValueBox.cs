using System;
using UnityEngine;
using UnityEngine.UI;
using ExtensionMethods;
using static ExpandedFlowControl.IfExt;
using static PathDebugger;
using static ValueBox.ValueBoxType;

public class ValueBox : MonoBehaviour
{
    // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Fields    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
    private int _value;
    public int Value
    {
        get => _value;
        set
        {
            if (!value.InRange(Min, Max)) value = value.Clamp(Min, Max);

            _value = value;
            ShowValue();
        }
    }

    public int Min, Max, Default;
    public string Label;
    public Text LabelText, ValueText;
    public ValueBoxType Type;

    private delegate bool Inequal (int a, int b);
    public enum ValueBoxType { Property, Size, ColorCount, PatternDifficulty, ShuffleCount, LimiterCount }

    // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Methods    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
    public void Start()
    {
        if (Label != "") LabelText.text = Label;
        Value = Default;
    }
    public virtual void Increase() => ChangeValue(true);
    public virtual void Decrease() => ChangeValue(false);

    protected void Log(object obj, string str = "") => If(name == "Size X", () => obj.Log(str));
    protected virtual void ChangeValue(bool increase)
    {
        Inequal inequal;
        int limit  = increase ? Max : Min;
        int amount = increase ? 1 : -1;

        if (increase) inequal = lessThan;
        else inequal = greaterThan;

        if (!inequal(Value, limit)) return;
        Value += amount;

        //todo: Disable the button if the limit has been met
        //todo: Enable the opposite button if it was previously disabled

        // ---- Local Functions ---- //
        bool lessThan(int a, int b)    => a < b;
        bool greaterThan(int a, int b) => a > b;
    }
    private void ShowValue() => ValueText.text = Value.ToString();
}
