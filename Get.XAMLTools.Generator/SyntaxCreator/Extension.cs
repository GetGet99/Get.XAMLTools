using System;
using System.Collections;
using System.Collections.Generic;

namespace EasyCSharp.GeneratorTools.SyntaxCreator;

static partial class Extension
{
    public static string? GetString(this SyntaxVisibility visibility)
        => visibility switch
        {
            SyntaxVisibility.Default => null,
            SyntaxVisibility.Public => "public",
            SyntaxVisibility.Protected => "protected",
            SyntaxVisibility.Private => "private",
            // Incluing DoNotGenerate
            _ => throw new ArgumentException()
        };
    public static string JoinWith(this IEnumerable<string> enumerable, string separator)
        => string.Join(separator, enumerable);

    /// <summary>
    /// Allias helper for newer syntax
    /// </summary>
    /// <param name="values">Enumerable to add</param>
    public static void Add<T>(this ICollection<T> collection, IEnumerable<T> values)
    {
        foreach (var value in values)
            collection.Add(value);
    }
    public static void Add<T>(this ICollection<T> collection, Func<IEnumerable<T>> valuesCreator)
    {
        foreach (var value in valuesCreator())
            collection.Add(value);
    }
    public static void Add<T>(this ICollection<T> collection, Func<T?> valuesCreator)
    {
        var value = valuesCreator();
        if (value is not null)
            collection.Add(value);
    }
    public static void Add<T>(this T collection, Action<T> valuesCreator) where T : ICollection
    {
        valuesCreator(collection);
    }
    /// <summary>
    /// Allias helper for newer syntax
    /// </summary>
    /// <param name="values">Enumerable to add</param>
    public static void Add<T>(this LinkedList<T> collection, T value)
        => collection.AddLast(value);
}
