using System;
using UnityEngine;
using static ColorExt;
using ExtensionMethods;
using System.Collections.Generic;
using static PathDebugger;
using static ExpandedFlowControl.IfExt;

public class ToggleButtonManager : MonoBehaviour
{
    #region { Propertiers }

    // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Properties      ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
    private static Dictionary<TileColor,  ToggleButton> _colorToggles;
    public  static Dictionary<TileColor,  ToggleButton> ColorToggles
    {
        get
        {
            if (_colorToggles == null)
            {
                _colorToggles = new Dictionary<TileColor, ToggleButton>();
                if (instance.ColorTogglePairs == null) return null;

                instance.ColorTogglePairs.ForEach(c => If(c != null, () => _colorToggles.Add(c.Key, c.Value)));
            }
            return _colorToggles;
        }
    }
    private static Dictionary<TileType,   ToggleButton> _typeToggles;
    public  static Dictionary<TileType,   ToggleButton> TypeToggles
    {
        get
        {
            if (_typeToggles == null)
            {
                _typeToggles = new Dictionary<TileType, ToggleButton>();
                instance.TypeTogglePairs.ForEach(c => _typeToggles.Add(c.Key, c.Value));
            }
            return _typeToggles;
        }
    }
    private static Dictionary<ColorShade, ToggleButton> _shadeToggles;
    public  static Dictionary<ColorShade, ToggleButton> ShadeToggles
    {
        get
        {
            if (_shadeToggles == null)
            {
                _shadeToggles = new Dictionary<ColorShade, ToggleButton>();
                instance.ShadeTogglePairs.ForEach(c => _shadeToggles.Add(c.Key, c.Value));
            }
            return _shadeToggles;
        }
    }

    #endregion
    #region [< Fields >]

    // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Fields    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //

    #region [ Serialized Fields ]

    [Serializable]
    public class TogglePair<TKey, TValue> where TValue : ToggleButton
    {
        public TKey Key;
        public TValue Value;
    }
    [Serializable] public class ColorTogglePair    : TogglePair<TileColor,    ToggleButton> { }
    [Serializable] public class TypeTogglePair     : TogglePair<TileType,     ToggleButton> { }
    [Serializable] public class ShadeTogglePair    : TogglePair<ColorShade,   ToggleButton> { }
    [Serializable] public class PropertyTogglePair : TogglePair<TileProperty, ToggleButton> { }
    #endregion
    #region < Normal Fields >

    public ColorTogglePair[] ColorTogglePairs;
    public ShadeTogglePair[] ShadeTogglePairs;
    public TypeTogglePair[]  TypeTogglePairs;

    private static ToggleButtonManager instance;
    #endregion
    #endregion

    // ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡    Methods    ≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡ //
    public void Start() => instance = this;
    public static void UpdateToggleButtons()
    {
        ColorToggleManager.Instance.UpdateToggleButtons(ColorToggles);
        ShadeToggleManager.Instance.UpdateToggleButtons(ShadeToggles);
        TypeToggleManager .Instance.UpdateToggleButtons(TypeToggles);
    }
}