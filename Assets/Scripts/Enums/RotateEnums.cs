using System;
using static RotateDirection;

public enum RotateDirection { None, Clockwise = 1, CounterClockwise = 2 }

public static class RotateDirectionExt
{
    public static string MakeString(this RotateDirection direction)  => direction  == CounterClockwise ? "cc" : "cw";
    public static RotateDirection ToRotateDirection(this string str) => str == "cc" ? CounterClockwise : str == "cw" ? Clockwise : throw new ArgumentException("Invalid Rotation Direction");
}