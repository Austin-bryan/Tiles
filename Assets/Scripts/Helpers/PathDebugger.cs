using System;
using UnityEngine;
using ExtensionMethods;
using System.Linq.Expressions;

public static class PathDebugger
{
    public static void Path1() => PathX(1);
    public static void Path2() => PathX(2);
    public static void Path3() => PathX(3);
    public static void Path4() => PathX(4);
    public static void Path5() => PathX(5);
    public static void Path6() => PathX(6);
    public static void Path7() => PathX(7);
    public static void Path8() => PathX(8);
    public static void Path9() => PathX(9);
    public static void Path0() => PathX(0);
    public static void PathX(int x) => Debug.Log($"Path {x}");

    public static string Pair(string nameA, object objA, string nameB, object objB) => $"{nameA}: {objA}, {nameB}: {objB}";
    public static string Pair(string nameA, object objA, string nameB, object objB, string nameC, object objC) => $"{nameA}: {objA}, {nameB}: {objB}, {nameC}: {objC}";

    public static string Pair(params object[] objects)
    {
        string str = string.Empty;

        for (int i = 0; i < objects.Length; i++)
            str += objects[i].ToString() + (i == objects.Length - 1 ? "" : ";   ");

        return str;
    }
    public static string GetNameValue<T>(Expression<Func<T>> var1)
    {
        return $"{GetBody().Member.Name.Captialize()}: {GetBody().GetValue<T>()}";

        // ---- Local Functions ---- //
        MemberExpression GetBody() => (MemberExpression)var1.Body;
    }
}
