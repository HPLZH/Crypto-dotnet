using System.Collections;
using System.Numerics;

namespace C;

public static class EnumerableExt
{
    public static IEnumerable<T> And<T>(this IEnumerable<T> x, IEnumerable<T> y)
        where T : IBitwiseOperators<T, T, T>
    {
        var ex = x.GetEnumerator();
        var ey = y.GetEnumerator();
        while (ex.MoveNext() && ey.MoveNext())
        {
            yield return ex.Current & ey.Current;
        }
    }

    public static IEnumerable<bool> And(this IEnumerable<bool> x, IEnumerable<bool> y)
    {
        var ex = x.GetEnumerator();
        var ey = y.GetEnumerator();
        while (ex.MoveNext() && ey.MoveNext())
        {
            yield return ex.Current && ey.Current;
        }
    }

    public static IEnumerable<T> Or<T>(this IEnumerable<T> x, IEnumerable<T> y)
        where T : IBitwiseOperators<T, T, T>
    {
        var ex = x.GetEnumerator();
        var ey = y.GetEnumerator();
        while (ex.MoveNext() && ey.MoveNext())
        {
            yield return ex.Current | ey.Current;
        }
    }

    public static IEnumerable<bool> Or(this IEnumerable<bool> x, IEnumerable<bool> y)
    {
        var ex = x.GetEnumerator();
        var ey = y.GetEnumerator();
        while (ex.MoveNext() && ey.MoveNext())
        {
            yield return ex.Current || ey.Current;
        }
    }

    public static IEnumerable<T> Xor<T>(this IEnumerable<T> x, IEnumerable<T> y)
        where T : IBitwiseOperators<T, T, T>
    {
        var ex = x.GetEnumerator();
        var ey = y.GetEnumerator();
        while (ex.MoveNext() && ey.MoveNext())
        {
            yield return ex.Current ^ ey.Current;
        }
    }

    public static IEnumerable<bool> Xor(this IEnumerable<bool> x, IEnumerable<bool> y)
    {
        var ex = x.GetEnumerator();
        var ey = y.GetEnumerator();
        while (ex.MoveNext() && ey.MoveNext())
        {
            yield return ex.Current ^ ey.Current;
        }
    }

    public static IEnumerable<T> Not<T>(this IEnumerable<T> x)
        where T : IBitwiseOperators<T, T, T>
    {

        foreach (var xi in x)
        {
            yield return ~xi;
        }
    }

    public static IEnumerable<bool> Not(this IEnumerable<bool> x)
    {
        
        foreach (var xi in x)
        {
            yield return !xi;
        }
    }

    public static IEnumerable<byte> ToBytes(this IEnumerable<bool> x, bool? isLittleEndian = null)
    {
        byte buf = 0;
        int pos = 0;
        foreach (var xi in x)
        {
            if (pos == 0) buf = 0;
            if (xi)
            {
                if (isLittleEndian ?? BitConverter.IsLittleEndian)
                {
                    buf |= (byte)(1 << pos);
                }
                else
                {
                    buf |= (byte)(128 >> pos);
                }
            }
            pos++;
            if (pos == 8)
            {
                yield return buf;
                pos = 0;
            }
        }
        if (pos != 0)
        {
            yield return buf;
        }
    }

    public static string ToSeqString(this IEnumerable<bool> x) => ToNumString(x, true);

    public static string ToNumString(this IEnumerable<bool> x, bool? isLittleEndian = null)
    {
        string s = string.Empty;
        if(isLittleEndian ?? BitConverter.IsLittleEndian)
        {
            foreach(var xi in x)
            {
                s = (xi ? '1' : '0') + s;
            }
        }
        else
        {
            foreach (var xi in x)
            {
                s += (xi ? '1' : '0');
            }
        }
        return s;
    }

}
