using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static class TypeExtensions
{
    public static IEnumerable<T> AsEnumerable<T>(this Type type)
    {
        if (ReferenceEquals(null, type))
        {
            yield break;
        }

        foreach (T val in type.GetRuntimeProperties().Select(f => f.GetValue(null)).OfType<T>())
        {
            yield return val;
        }
    }
}