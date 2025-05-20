using System.Management.Automation;
using L5Sharp.Core;

// ReSharper disable InvertIf

namespace LogixShell;

public static class Extensions
{
    /// <summary>
    /// Filters an enumerable collection based on a selector function and an optional filter string.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input collection.</typeparam>
    /// <param name="collection">The collection of elements to be filtered.</param>
    /// <param name="selector">A function that extracts a string value from an element of the collection for comparison.</param>
    /// <param name="filter">A string used to filter the elements. Supports wildcard characters.</param>
    /// <returns>
    /// A collection containing the filtered elements that match the given filter string.
    /// If the filter is null or empty, the original collection is returned.
    /// </returns>
    public static IEnumerable<T> TryFilterText<T>(this IEnumerable<T> collection, Func<T, string?> selector,
        string? filter)
    {
        if (string.IsNullOrEmpty(filter))
        {
            return collection;
        }

        if (WildcardPattern.ContainsWildcardCharacters(filter))
        {
            var pattern = new WildcardPattern(filter, WildcardOptions.IgnoreCase);
            return collection.Where(x => pattern.IsMatch(selector(x)));
        }

        return collection.Where(x => StringComparer.OrdinalIgnoreCase.Equals(selector(x), filter));
    }

    /// <summary>
    /// Filters a collection of tags based on an optional dimension rank.
    /// </summary>
    /// <param name="tags">The collection of tags to be filtered.</param>
    /// <param name="dimensions">An optional dimension rank to filter tags by. If null, no filtering is applied based on dimensions.</param>
    /// <returns>
    /// A collection of tags where each tag matches the specified dimension rank, if provided.
    /// If no dimension rank is specified, the original collection is returned.
    /// </returns>
    public static IEnumerable<Tag> TryFilterRank(this IEnumerable<Tag> tags, int? dimensions)
    {
        return dimensions.HasValue
            ? tags.Where(t => t.Dimensions.Rank == dimensions)
            : tags;
    }
}