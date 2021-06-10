using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using ExtensionMethods;
using System.Collections;
using System.Linq.Expressions;
using System.Collections.Generic;
using static System.Linq.Expressions.Expression;

namespace ExtensionMethods
{
    public static class MonoBehaviourExt
    {
        public static T Get<T>(this MonoBehaviour mono) where T : Component  => mono.GetComponent<T>();
        public static T Get<T>(this MonoBehaviour mono, int childIndex) where T : Component                      => mono.transform.GetChild(childIndex).GetComponent<T>();
        public static T Get<T>(this MonoBehaviour mono, params int[] childIndexes) where T : Component           => mono.GetChild(childIndexes).GetComponent<T>();
        public static T Get<T>(this MonoBehaviour mono, int childIndex, int grandChildIndex) where T : Component => mono.transform.GetChild(childIndex).GetChild(grandChildIndex).GetComponent<T>();

        public static GameObject GetChild(this MonoBehaviour mono, params int[] indexes)
        {
            var child = mono.gameObject;

            for (int i = 0; i < indexes.Length; i++)
                child = child.transform.GetChild(indexes[i]).gameObject;

            return child;
        }

        public static void Destroy          (this MonoBehaviour mono)                            => GameObject.Destroy(mono.gameObject);
        public static void Show             (this MonoBehaviour mono)                            => mono.Get<Renderer>().enabled = false;
        public static void Hide             (this MonoBehaviour mono)                            => mono.Get<Renderer>().enabled = true;
        public static void SetParent        (this MonoBehaviour child, GameObject newParent)     => child.transform.SetParent(newParent.transform, true);
        public static void SetParent        (this MonoBehaviour child, MonoBehaviour newParent)  => child.transform.SetParent(newParent.transform, true);
        public static void Visible          (this MonoBehaviour mono, bool visible)              => mono.Get<Renderer>().enabled = visible;
        public static void SetLocalScale    (this MonoBehaviour mono, float scale)               => mono.transform.localScale    = new Vector3(scale, scale);
        public static void SetLocalScale    (this MonoBehaviour mono, float a, float b)          => mono.transform.localScale    = new Vector3(a, b);
        public static void SetLocalScale    (this MonoBehaviour mono, float a, float b, float c) => mono.transform.localScale    = new Vector3(a, b, c);
        public static void SetLocalScale    (this MonoBehaviour mono, Vector3 scale)             => mono.transform.localScale    = scale;
        public static void SetPosition      (this MonoBehaviour mono, Vector3 position)          => mono.transform.position      = position;
        public static void SetRotation      (this MonoBehaviour mono, Vector3 rotation)          => mono.transform.eulerAngles   = rotation;
        public static void SetLocalPosition (this MonoBehaviour mono, Vector3 position)          => mono.transform.localPosition = position;
        public static void SetRotation      (this MonoBehaviour mono, Quaternion rotation)       => mono.transform.rotation      = rotation;

        public static string Name           (this MonoBehaviour mono)                            => mono.transform.name;
        public static Vector3 Scale         (this MonoBehaviour mono)                            => mono.transform.localScale;
        public static Vector3 Position      (this MonoBehaviour mono)                            => mono.transform.position;
        public static Vector3 GlobalScale   (this MonoBehaviour mono)                            => mono.transform.lossyScale;
        public static Quaternion Rotation   (this MonoBehaviour mono)                            => mono.transform.rotation;
        public static Transform GetTransform(this MonoBehaviour mono)                            => new Transform(mono.transform.position, mono.transform.rotation, mono.transform.localScale);
        public static void DetatchLastChild (this MonoBehaviour mono)                            => mono.GetChild(mono.transform.childCount - 1).transform.parent = null;
        public static void DetatchChild     (this MonoBehaviour mono, int child)                 => mono.GetChild(child).transform.parent = null;
        public static GameObject GetChild   (this MonoBehaviour mono, int index)                 => mono.gameObject.transform.GetChild(index).gameObject;
        public static GameObject GetChild   (this MonoBehaviour mono, int index, int grandIndex) => mono.gameObject.transform.GetChild(index).GetChild(grandIndex).gameObject;
        public static void SetTransform     (this MonoBehaviour mono, Transform transform)
        {
            mono.SetPosition(transform.Location);
            mono.SetRotation(transform.Rotation);
            mono.SetLocalScale(transform.Scale);
        }

        /// <summary>
        ///  Gets the specified component from the last most child
        /// </summary>
        public static T GetChild<T>(this MonoBehaviour mono, params int[] indexes) => GetChild(mono, indexes).GetComponent<T>();

        /// <summary>
        /// Prevents this MonoBehavior from being destroyed between loads
        /// and prevents duplicates from being created
        /// </summary>
        /// <param name="hasBeenCreated">Variable that will keep track of if it has been created yet</param>
        public static void MakePersistentSingleton(this MonoBehaviour mono, ref bool hasBeenCreated)
        {
            if (hasBeenCreated)
            {
                mono.Destroy();
                return;
            }
            else hasBeenCreated = true;
            MonoBehaviour.DontDestroyOnLoad(mono);
        }
        public static void Delay(this MonoBehaviour mono, float time, Action action)
        {
            if (mono == null) return;
            mono.StartCoroutine(delay(time));

            // ---- Local Functions ---- //
            IEnumerator delay(float delayTime)
            {
                yield return new WaitForSeconds(delayTime);
                action();
            }
        }
    }
    public static class MaskExt
    {
        public static float ScaleY(this SpriteMask mask) => mask.transform.localScale.y;
        public static float ScaleX(this SpriteMask mask) => mask.transform.localScale.x;
    }
    public static class InputExt
    {
        public static bool Ctrl(this Input input)  => Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
        public static bool Shift(this Input input) => Input.GetKey(KeyCode.LeftShift)   || Input.GetKey(KeyCode.RightShift);
    }
    public static class ArrayExt
    {
        public static IEnumerable<(int index, T item)> Index<T>(this T[] array)
        {
            for (int i = 0; i < array.Length; i++)
                yield return (i, array[i]);
        }
        public static IEnumerable<(int index, T item)> Index<T>(this T[,] array) => array.ToArray().Index();
        public static List<T> ToList<T>(this T[,] array)
        {
            var list = new List<T>();

            foreach (var x in array) list.Add(x);

            return list;
        }
        public static T[] ToArray<T>(this T[,] array) => array.ToList().ToArray();
        public static void ForEach<T>(this T[,] array, Action<T> action)
        {
            foreach (T t in array) action(t);
        }
        public static void ForEach<T>(this T[] array, Action<T> action)
        {
            foreach (T t in array) action(t);
        }
        public static void ForEach<T>(this T[][] array, Action<T> action)
        {
            foreach (T[] t in array)
                foreach (T x in t)
                    action(x);
        }

        public static bool Contains<T>(this T[,] arrays, T t)
        {
            for (int i = 0; i < arrays.GetLength(0); i++)
                for (int j = 0; j < arrays.GetLength(1); j++)
                    if (EqualityComparer<T>.Default.Equals(arrays[i, j], t))
                        return true;
            return false;
        }
        public static bool Contains<T>(this T[][] arrays, T t)
        {
            foreach (var array in arrays)
                if (array.Contains(t))
                    return true;
            return false;
        }
    }
    public static class ListExt
    {
        public static IEnumerable<(int index, T item)> Index<T>(this List<T> list)
        {
            for (int i = 0; i < list.Count; i++)
                yield return (i, list[i]);
        }
        public static void AddRange<T>(this List<T> list, params T[] ts) => list.AddRange(ts);
        public static List<T> Duplicate<T>(this List<T> list)
        {
            var newList = new List<T>();

            foreach (var item in list)
                newList.Add(item);

            return newList;
        }
        public static void AddUnique<T>(this List<T> list, T t)
        {
            if (!list.Contains(t)) 
                list.Add(t);
        }
    }
    public static class GameObjectExt
    {
        public static GameObject GetChild(this GameObject go, params int[] indexes)
        {
            GameObject child = go.gameObject;

            foreach (var i in indexes)
                child = child.transform.GetChild(indexes[i]).gameObject;

            return child;
        }

        public static T Get<T>(this GameObject go) where T : Component => go.GetComponent<T>();
        public static T Get<T>(this GameObject go, int index) where T : Component => go.GetChild(index).GetComponent<T>();
        public static T Get<T>(this GameObject go, params int[] indexes) where T : Component => go.GetChild(indexes).GetComponent<T>();
        public static T Get<T>(this GameObject go, int index, int grandIndex) where T : Component => go.GetChild(grandIndex).GetChild(index).GetComponent<T>();

        public static void Destroy         (this GameObject gameObject)                         => Destroy(gameObject.gameObject);
        public static string Name          (this GameObject gameObject)                         => gameObject.transform.name;
        public static Vector3 Position     (this GameObject gameObject)                         => gameObject.transform.position;
        public static Vector3 Scale        (this GameObject gameObject)                         => gameObject.transform.localScale;
        public static Vector3 GlobalScale  (this GameObject gameObject)                         => gameObject.transform.lossyScale;
        public static Quaternion Rotation  (this GameObject gameObject)                         => gameObject.transform.rotation;
        public static void SetParent       (this GameObject a, GameObject b)                    => a.transform.SetParent(b.transform, true);
        public static void SetLocalScale   (this GameObject gameObj, Vector3 scale)             => gameObj.transform.localScale = scale;
        public static void SetLocalPosition(this GameObject gameObj, Vector3 position)          => gameObj.transform.localPosition = position;
        public static void SetPosition     (this GameObject gameObj, Vector3 position)          => gameObj.transform.position   = position;
        public static void SetRotation     (this GameObject gameObj, Quaternion rotation)       => gameObj.transform.rotation   = rotation;
        public static void SetLocalScale   (this GameObject gameObj, float a, float b)          => gameObj.transform.localScale = new Vector3(a, b);
        public static void SetLocalScale   (this GameObject gameObj, float a, float b, float c) => gameObj.transform.localScale = new Vector3(a, b, c);
        public static void DetatchLastChild(this GameObject gameObj)                            => gameObj.GetChild(gameObj.transform.childCount - 1).transform.parent = null;
        public static void DetatchChild    (this GameObject gameObj, int child)                 => gameObj.GetChild(child).transform.parent = null;
        public static void SetTransform    (this GameObject gameObj, Transform transform)
        {
            gameObj.SetPosition(transform.Location);
            gameObj.SetRotation(transform.Rotation);
            gameObj.SetLocalScale(transform.Scale);
        }
        
        /// <summary>
        ///  Gets the specified component from the last most child
        /// </summary>
        public static T GetChild<T>(this GameObject go, params int[] indexes) => GetChild(go, indexes).GetComponent<T>();
        public static void Show(this GameObject go) => go.Visible(true);
        public static void Hide(this GameObject go) => go.Visible(false);
        public static void Visible(this GameObject go, bool visible) => go.GetComponent<Renderer>().enabled = visible;
    }
    public static class IEnumerableExt
    {
        public static IEnumerable<(int index, T item)> Index<T>(this IEnumerable<T> list)
        {
            var count = list.Count();
            for (int i = 0; i < count; i++)
                yield return (i, list.ElementAt(i));
        }
        public static int IndexOf<T>(this IEnumerable<T> enumerable, T item) => enumerable.ToList().IndexOf(item);
        public static void LogEach<T>(this IEnumerable<T> ts)
        {
            string res = "";

            foreach (var item in ts) res += ts.ToString() + ", ";
            if (res.Length > 2)  res = res.Substring(0, res.Length - 2);
        }
        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable => listToClone.Select(item => (T)item.Clone()).ToList();

        public static void Deconstruct<T>(this IEnumerable<T> array, out T first)
        {
            first   = GetElement(array, 0);
        }
        public static void Deconstruct<T>(this IEnumerable<T> array, out T first, out T second)
        {
            first   = GetElement(array, 0);
            second  = GetElement(array, 1);
        }
        public static void Deconstruct<T>(this IEnumerable<T> array, out T first, out T second, out T third)
        {
            first   = GetElement(array, 0);
            second  = GetElement(array, 1);
            third   = GetElement(array, 2);
        }
        public static void Deconstruct<T>(this IEnumerable<T> array, out T first, out T second, out T third, out T fourth)
        {
            first   = GetElement(array, 0);
            second  = GetElement(array, 1);
            third   = GetElement(array, 2);
            fourth  = GetElement(array, 3);
        }
        public static void Deconstruct<T>(this IEnumerable<T> array, out T first, out T second, out T third, out T fourth, out T fifth)
        {
            first   = GetElement(array, 0);
            second  = GetElement(array, 1);
            third   = GetElement(array, 2);
            fourth  = GetElement(array, 3);
            fifth   = GetElement(array, 4);
        }
        public static void Deconstruct<T>(this IEnumerable<T> array, out T first, out T second, out T third, out T fourth, out T fifth, out T sixth)
        {
            first   = GetElement(array, 0);
            second  = GetElement(array, 1);
            third   = GetElement(array, 2);
            fourth  = GetElement(array, 3);
            fifth   = GetElement(array, 4);
            sixth   = GetElement(array, 5);
        }
        public static void Deconstruct<T>(this IEnumerable<T> array, out T first, out T second, out T third, out T fourth, out T fifth, out T sixth, out T seventh)
        {
            first   = GetElement(array, 0);
            second  = GetElement(array, 1);
            third   = GetElement(array, 2);
            fourth  = GetElement(array, 3);
            fifth   = GetElement(array, 4);
            sixth   = GetElement(array, 5);
            seventh = GetElement(array, 6);
        }

        private static IEnumerable<T> GetRestOfArray<T>(this IEnumerable<T> array, int skip) => array.Skip(skip).ToArray();
        private static T GetElement<T>(this IEnumerable<T> en, int index) => index >= en.Count() ? default(T) : en.ElementAt(index);
    }
    public static class MiscExt
    {
        public static bool ContainsTile(this List<(PlayerTile, Direction)> list, PlayerTile tile)
        {
            foreach (var (t, d) in list)
                if (t == tile) return true;
            return false;
        }
    }
    public static class ObjectExt
    {
        public static void NullCheck(this object obj)         => Debug.Log("Object is " + (obj == null ? "" : "not ") + "null");
        public static void Log(this object obj)               => Debug.Log(obj);
        public static void Log(this object obj, string name)  => Debug.Log($"{name}: {obj}");
        public static void Log(this object obj, object item)  => Debug.Log($"{item.ToString()}: {obj}");
        public static bool RefEquals(this object a, object b) => ReferenceEquals(a, b);
        public static string Pair(this object obj, params object[] objects) => PathDebugger.Pair(obj, objects);
        public static (T CastedItem, bool Success) CastTo<T>(this object obj)
        {
            try   { return ((T)obj, true); }
            catch { return (default, false); }
        }
        public static bool To<T>(this object obj, out T castedItem)
        {
            bool success;
            (castedItem, success) = obj.CastTo<T>();

            return success;
        }
        public static T To<T>(this object obj, out bool success)
        {
            T item;
            (item, success) = obj.CastTo<T>();

            return item;
        }
        public static T To<T>(this object obj) => obj.CastTo<T>().CastedItem;
    }
    public static class FloatExt
    {
        public static float Mod     (this float f, float m) => f % m;
        public static float Abs     (this float f) => Mathf.Abs(f);
        public static int RoundToInt(this float f) => Mathf.RoundToInt(f);
        public static int FloorToInt(this float f) => Mathf.FloorToInt(f);
        public static int CeilToInt (this float f) => Mathf.CeilToInt(f);
        public static float Round   (this float f) => Mathf.Round(f);
        public static float Floor   (this float f) => Mathf.Floor(f);
        public static float CelT    (this float f) => Mathf.Ceil(f);
        public static float Sin     (this float f) => Mathf.Sin(f);
        public static float Cos     (this float f) => Mathf.Cos(f);
        
        public static Vector2 ToVector2(this float f) => new Vector2(f, f);
        public static Vector3 ToVector3(this float f) => new Vector3(f, f, f);
    }
    public static class IntExt
    {
        public static int Abs(this int x) => Math.Abs(x);
        public static int Wrap(this int x, int min, int max)
        {
            if (x < min) return max;
            if (x > max) return min;
            return x;
        }
        public static int Clamp(this int n, int min, int max)    => n < min ? min :
                                                                    n > max ? max : n;
        public static bool IsEven(this int n) => n % 2 == 0;
        
        public static float Mod (this int i, int m) => i % m;
        public static float Sin (this int i) => Mathf.Sin(i);
        public static float Cos (this int i) => Mathf.Cos(i);

        /// <summary>
        /// Returns true if x is in range of min (inclusive) and max (inclusive)
        /// </summary>
        /// <param name="min">Inclusive min</param>
        /// <param name="max">Inclusive max</param>
        public static bool HasFlag(this int x, int flag)         => (x & flag) == flag;
        public static bool HasSameSign(this int a, int b)        => (a < 0 && b < 0) || (a > 0 && b > 0) || (a == 0 || b == 0);
        public static bool InBounds(this int n, string str)      => n < str.Length;
        public static bool InRange(this int x, int min, int max) => x >= min && x <= max;
    }
    public static class Vector3Ext
    {
        public static Vector3 AddX(this ref Vector3 vect, float x) { vect.x = vect.x + x; return vect; }
        public static Vector3 AddY(this ref Vector3 vect, float y) { vect.y = vect.y + y; return vect; }
        public static Vector3 AddZ(this ref Vector3 vect, float z) { vect.z = vect.z + z; return vect; }
                                        
        public static Vector3 SubX(this ref Vector3 vect, float x) { vect.x = vect.x - x; return vect; }
        public static Vector3 SubY(this ref Vector3 vect, float y) { vect.y = vect.y - y; return vect; }
        public static Vector3 SubZ(this ref Vector3 vect, float z) { vect.z = vect.z - z; return vect; }

        public static Vector3 Multiply(this Vector3 a, Vector3 b) => new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
        public static bool ApproximatelyEqual(this Vector3 a, Vector3 b, int tolerance) => (a.x - b.x).Abs() <= tolerance && (a.y - b.y).Abs() <= tolerance;
    }
    public static class DirectionExt
    {
        /// <summary>
        /// Swaps the key and value of the dictionary.
        /// </summary>
        /// <typeparam name="TKey">The original value</typeparam>
        /// <typeparam name="TValue">The original key</typeparam>
        /// <param name="dict"></param>
        /// <returns>The inverted dictionation where the keys are now values and values are now keys</returns>
        public static Dictionary<TValue, TKey> Swap<TKey, TValue>(this Dictionary<TKey, TValue> dict)
        {
            var d = new Dictionary<TValue, TKey>();

            foreach (var pair in dict)
                d.Add(pair.Value, pair.Key);

            return d;
    }
    }
    public static class QuaternionExt
    {
        public static Quaternion AddX(this ref Quaternion quat, float x) { quat.x = quat.x + x; return quat; }
        public static Quaternion AddY(this ref Quaternion quat, float y) { quat.y = quat.y + y; return quat; }
        public static Quaternion AddZ(this ref Quaternion quat, float z) { quat.z = quat.z + z; return quat; }

        public static Quaternion SubX(this ref Quaternion quat, float x) { quat.x = quat.x - x; return quat; }
        public static Quaternion SubY(this ref Quaternion quat, float y) { quat.y = quat.y - y; return quat; }
        public static Quaternion SubZ(this ref Quaternion quat, float z) { quat.z = quat.z - z; return quat; }
    }
    public static class TransformExt
    {
        public static Transform ToTransform(this UnityEngine.Transform transform) => new Transform(transform.position, transform.rotation, transform.localScale);
        public static T Get<T>(this UnityEngine.Transform transform)              => transform.GetComponent<T>();
    }
    public static class StringExt
    {
        public static (int, int) Parst(this (string, string) strTuple) => (strTuple.Item1.Parse(), strTuple.Item2.Parse());
        public static bool   TryParse(this string str, out int result)=> Int32.TryParse(str, out result);
        public static bool   NoCaseEquals(this string str, int a)     => String.Equals(a, StringComparison.OrdinalIgnoreCase);
        public static bool   IsInt (this string str)   => str.TryParse(out _);
        public static int    Parse(this string str)    => Int32.Parse(str);
        public static char   ToChar(this string str)   => System.Convert.ToChar(str);
        public static string Remove(this string str, params string[] charactersToRemove)
        {
            charactersToRemove.ToList().ForEach(x => str = str.Replace(x, ""));
            return str;
        }
        public static string Captialize(this string str)
        {
            switch (str)
            {
                case null : throw new ArgumentNullException(nameof(str));
                case ""   : throw new ArgumentException($"{nameof(str)} cannot be empty");
                default   : return str.First().ToString().ToUpper() + str.Substring(1);
            }
        }
        public static string RemoveWhiteSpace(this string str) => new string(str.Where(c => !c.IsWhiteSpace())
                                                                                .ToArray());
        public static (string, string) SplitInTwo(this string str, char c) => (str.Split(c)[0], str.Split(c)[1]);
    }
    public static class CharExt
    {
        public static char ToUpper(this char c)      => Char.ToUpper(c);
        public static bool IsWhiteSpace(this char c) => Char.IsWhiteSpace(c);
        public static char ToLower(this char c)      => Char.ToLower(c);
        public static int  Parse(this char c)        => Int32.Parse(c.ToString());
        public static bool IsInt(this char c)        => c.TryParse(out _);
        public static bool IsAny(this char c, params char[] cs)
        {
            foreach (var letter in cs)
                if (c == letter) return true;
            return false;
        }
        public static bool TryParse(this char c, out int result) => c.ToString().TryParse(out result);
    }
    public static class DictionaryExt
    {
        public static U Find<T, U>(this Dictionary<T, U> dict, T t, U u = default) => dict.Keys.Contains(t) ? dict[t] : u;
        public static List<(T, U)> ToTupleList<T, U>(this Dictionary<T, U> dict)
        {
            var list = new List<(T, U)>();

            foreach (var key in dict.Keys)
                list.Add((key, dict[key]));

            return list;
        }
        public static void AddUnique<T, U>(this Dictionary<T, U> dict, T t, U u)
        {
            if (!dict.ContainsKey(t)) dict.Add(t, u);
        }
    }
    public static class ButtonExt
    {
        public static void SetSprite(this Button button, Sprite sprite) => button.image.sprite = sprite;
        public static void SetColor(this Button button, Color color)    => button.image.color = color;
        public static Sprite GetSprite(this Button button) => button.image.sprite;
        public static Color GetColor(this Button button)   => button.image.color;
    }
    public static class EnumExt
    {
        // ---- Add Flag ---- //
        public static T AddFlag<T>(this ref T e, T flag)
            where T : struct, IConvertible
        {
            if (!(typeof(T).IsEnum)) throw new ArgumentException("Value must be an enum");

            e = (T)(object)((int)(object)e | (int)(object)flag);
            return e;
        }
        public static T AddFlag<T>(this ref T e, T flag1, T flag2)
            where T : struct, IConvertible
        {
            e.AddFlag(flag1);
            e.AddFlag(flag2);

            return e;
        }
        public static T AddFlag<T>(this ref T e, T flag1, T flag2, T flag3)
            where T : struct, IConvertible
        {
            e.AddFlag(flag1);
            e.AddFlag(flag2);
            e.AddFlag(flag3);

            return e;
        }
        public static T AddFlag<T>(this ref T e, T flag1, T flag2, T flag3, T flag4)
            where T : struct, IConvertible
        {
            e.AddFlag(flag1);
            e.AddFlag(flag2);
            e.AddFlag(flag3);
            e.AddFlag(flag4);

            return e;
        }
        public static T AddFlag<T>(this ref T e, params T[] flags)
            where T : struct, IConvertible
        {
            foreach (var flag in flags)
                e.AddFlag(flag);

            return e;
        }
        public static T AddFlag<T>(this ref T e, List<T> flags) where T : struct, IConvertible => e.AddFlag(flags.ToArray());

        // ---- Remove Flag ---- //
        public static T RemoveFlag<T>(this ref T e, T flag)
            where T : struct, IConvertible
        {
            if (!(typeof(T).IsEnum)) throw new ArgumentException("Value must be an enum");

            e = (T)(object)((int)(object)e & ~(int)(object)flag);
            return e;
        }
        public static T RemoveFlag<T>(this ref T e, T flag1, T flag2)
            where T : struct, IConvertible
        {
            e.RemoveFlag(flag1);
            e.RemoveFlag(flag2);

            return e;
        }
        public static T RemoveFlag<T>(this ref T e, T flag1, T flag2, T flag3)
           where T : struct, IConvertible
        {
            e.RemoveFlag(flag1);
            e.RemoveFlag(flag2);
            e.RemoveFlag(flag3);

            return e;
        }
        public static T RemoveFlag<T>(this ref T e, T flag1, T flag2, T flag3, T flag4)
          where T : struct, IConvertible
        {
            e.RemoveFlag(flag1);
            e.RemoveFlag(flag2);
            e.RemoveFlag(flag3);
            e.RemoveFlag(flag4);

            return e;
        }
        public static T RemoveFlag<T>(this ref T e, params T[] flags)
         where T : struct, IConvertible
        {
            foreach (var flag in flags)
                e.RemoveFlag(flag);

            return e;
        }
        public static T RemoveFlag<T>(this ref T e, List<T> flags) where T : struct, IConvertible => e.RemoveFlag(flags.ToArray());

        public static IEnumerable<T> GetFlags<T>(this T _enum)
            where T : struct
        {
            int e = _enum.To<int>();

            for (int i = 1, j = 1; i < Enum.GetValues(_enum.GetType()).Length; i++, j *= 2)
                if (e.HasFlag(j))
                    yield return (T)(object)j;
        }
    }
    public static class MemberExpressionExt
    {
        public static T GetValue<T>(this MemberExpression member) => Lambda<Func<T>>(Convert(member, typeof(T))).Compile()();
    }
    public static class BoolExt
    {
        public static bool If(this bool b, Action action)
        {
            if (b) action();
            return b;
        }
        public static bool ElseIf(this bool b, Func<bool> func, Action action)
        {
            if (b) return b;
            return b.ElseIf(func(), action);
        }
        public static bool ElseIf(this bool b, bool value, Action action)
        {
            if (b)
            {
                return value;
            }

            if (value) action();
            return value;
        }
        public static bool Else(this bool b, Action action)
        {
            if (!b) action();
            return b;
        }
        public static bool Toggle(ref this bool b) => b = !b;
    }
    public static class TupleExt
    {
        public static void Log<T, U>(this (T a, U b) tuple, string a, string b) => $"{a}: {tuple.a}, {b}: {tuple.b}".Log();
        public static void Log<T, U, V>(this (T a, U b, V c) tuple, string a, string b, string c) => $"{a}: {tuple.a}, {b}: {tuple.b}, {c}: {tuple.c}".Log();
    }
}
namespace ExpandedFlowControl
{
    public static class ForExt
    {
        public static void For(int length, Action<int> body) => For(length, (i) => i < length, body);
        public static void For(int length, Action body) => For(length, body);
        public static void For(int length, Predicate<int> predicate, Action<int> body)
        {
            for (int i = 0; predicate(i); i++)
                body(i);
        }
        public static void For(int length, Predicate<int> predicate, Action body)
        {
            for (int i = 0; predicate(i); i++)
                body();
        }

        public static void For<T>(int length, Func<int, T> body) => For(length, (i) => i < length, body);
        public static void For<T>(int length, Func<T> body) => For(length, (i) => i < length, body);
        public static void For<T>(int length, Predicate<int> predicate, Func<int, T> body)
        {
            for (int i = 0; predicate(i); i++)
                body(i);
        }
        public static void For<T>(int length, Predicate<int> predicate, Func<T> body)
        {
            for (int i = 0; predicate(i); i++)
                body();
        }
    }
    public static class IfExt
    {
        public static bool If(bool b, Action action)
        {
            if (b) action();
            return b;
        }
    }
    public static class ForEachExt
    {
        public static void ForEach<T>(IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
                action(item);
        }
        public static IEnumerable<U> ForEach<T, U>(IEnumerable<T> items, Func<T, U> func)
        {
            foreach (var item in items)
                yield return func(item);
        }
    }
    public static class LambdaExt
    {
        public static void L(params Action[] actions) => actions.ForEach(a => a());
        public static Action A(params Action[] actions)
        {
            return new Action(() => actions.ForEach(a => a()));
        }
    }
}
namespace ExpandedIneqaulity
{
    public static class IntEqualityExt
    {
        public static bool LessThan(this int a, int b) => a < b;
        public static bool GreaterThan(this int a, int b) => a > b;
        public static bool LessThanOrEqual(this int a, int b) => a <= b;
        public static bool GreaterThanOrEqual(this int a, int b) => a >= b;
        public static bool NotEqual(this int a, int b) => a != b;
    }
}