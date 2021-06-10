using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtensionMethods;

public static class InequalityDelegate
{
    public delegate bool Inequal(int a, int b);

    public static bool GreaterThan(int a, int b)        => a >  b;
    public static bool LessThan(int a, int b)           => a <  b;
    public static bool GreaterThanEqualTo(int a, int b) => a >= b;
    public static bool LessThanEqualTo(int a, int b)    => a <= b;
    public static bool EqualTo(int a, int b)            => a == b;
    public static bool NotEqualTo(int a, int b)         => a != b;
}
