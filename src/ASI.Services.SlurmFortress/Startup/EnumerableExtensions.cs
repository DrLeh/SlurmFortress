using System.Collections;
using System.Dynamic;

namespace ASI.Services.SlurmFortress;

public static class EnumerableExtensions
{
    /// <summary>
    /// Adds ForEach functionality to all IEnumerable objects.  Use with caution as this does enumerate a collection.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection">The collection.</param>
    /// <param name="action">The action to perform on each item.</param>
    public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
    {
        foreach (var item in collection)
            action.Invoke(item);
    }

    /// <summary>
    /// Iterates each item and allows an action to be performed on said item and its index
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection">The collection.</param>
    /// <param name="action">The action.</param>
    public static void EachWithIndex<T>(this IList<T> collection, Action<int, T> action)
    {
        for (var i = 0; i < collection.Count; i++)
            action.Invoke(i, collection[i]);
    }

    /// <summary>
    /// Execute an action on every item in the enumeration while it is being
    /// enumerated
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IEnumerable<T> Tap<T>(this IEnumerable<T> source, Action<T> action)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        if (action == null)
            throw new ArgumentNullException(nameof(action));
        foreach (var item in source)
        {
            action(item);
            yield return item;
        }
    }

    /// <summary>
    /// Execute an action on every item in the enumeration while it is being enumerated
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IEnumerable<T> Tap<T>(this IEnumerable<T> source, Action<int, T> action)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        if (action == null)
            throw new ArgumentNullException(nameof(action));
        var i = 0;
        foreach (var item in source)
        {
            action(i, item);
            i++;
            yield return item;
        }
    }

    /// <summary>
    /// Slices the specified collection into specified segments.  Possible enumeration of collection exists with this method.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection">The collection.</param>
    /// <param name="itemsPerSlice">The number of items per slice.</param>
    /// <returns></returns>
    public static List<List<T>> Slice<T>(this IEnumerable<T> collection, int itemsPerSlice)
    {
        var enumerable = collection as T[] ?? collection.ToArray();
        var totalItems = enumerable.Count();
        if (totalItems == 0) return new List<List<T>>();

        var results = new List<List<T>>(new[] { new List<T>() });

        for (var i = 0; i < totalItems; i++)
        {
            if (i != 0 && i % itemsPerSlice == 0)
                results.Add(new List<T>());
            results.Last().Add(enumerable.ElementAt(i));
        }
        return results;
    }

    /// <summary>
    /// Shorthand way of specifying a NOT filter
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection">The collection.</param>
    /// <param name="exceptFunction">The except function.</param>
    /// <returns></returns>
    public static IEnumerable<T> Except<T>(this IEnumerable<T> collection, Func<T, bool> exceptFunction)
    {
        return collection.Where(t => !exceptFunction.Invoke(t));
    }

    /// <summary>
    /// Converts an IList&lt;T> instance into an IReadOnlyList&lt;T> instance
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection"></param>
    /// <returns></returns>
    public static IReadOnlyList<T> AsReadOnly<T>(this IList<T> collection)
    {
        if (collection == null)
            throw new ArgumentNullException(nameof(collection));

        // I now check whether it already is IReadOnlyList<T>...
        return collection as IReadOnlyList<T> ?? new ReadOnlyListAdapter<T>(collection);
    }

    private sealed class ReadOnlyListAdapter<T> : IReadOnlyList<T>
    {
        private readonly IList<T> _source;

        public int Count => _source.Count;
        public T this[int index] => _source[index];

        public ReadOnlyListAdapter(IList<T> source) { _source = source; }

        public IEnumerator<T> GetEnumerator() { return _source.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
    }
    /// <summary>
    /// Extension method that turns a dictionary of string and object to an ExpandoObject
    /// </summary>
    public static ExpandoObject ToExpando(this IDictionary<string, object> dictionary)
    {
        var expando = new ExpandoObject();
        var expandoDic = (IDictionary<string, object>)expando;

        // go through the items in the dictionary and copy over the key value pairs)
        foreach (var kvp in dictionary)
        {
            // if the value can also be turned into an ExpandoObject, then do it!
            if (kvp.Value is IDictionary<string, object> value)
            {
                var expandoValue = value.ToExpando();
                expandoDic.Add(kvp.Key, expandoValue);
            }
            else
            {
                if (kvp.Value is ICollection items)
                {
                    // iterate through the collection and convert any strin-object dictionaries
                    // along the way into expando objects
                    var itemList = new List<object>();
                    foreach (var item in items)
                    {
                        if (item is IDictionary<string, object> map)
                        {
                            var expandoItem = map.ToExpando();
                            itemList.Add(expandoItem);
                        }
                        else
                        {
                            itemList.Add(item);
                        }
                    }

                    expandoDic.Add(kvp.Key, itemList);
                }
                else
                {
                    expandoDic.Add(kvp);
                }
            }
        }

        return expando;
    }

    /// <summary>
    /// Extension method to merge multiple dictionaries into one
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="dictionaries"></param>
    /// <returns></returns>
    public static Dictionary<TKey, TValue> Merge<TKey, TValue>(this IEnumerable<IDictionary<TKey, TValue>> dictionaries)
    {
        return dictionaries.SelectMany(d => d).GroupBy(p => p.Key).ToDictionary(g => g.Key, g => g.Last().Value);
    }

    /// <summary>
    /// Extension method to compute power set
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static IEnumerable<IEnumerable<T>> PowerSet<T>(this IList<T> source)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));

        return Enumerable.Range(0, 1 << source.Count).Select(m => Enumerable.Range(0, source.Count).Where(i => (m & (1 << i)) != 0).Select(i => source[i]));
    }

    /// <summary>
    /// Extension method to compute cartesian product
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sources"></param>
    /// <returns></returns>
    public static IEnumerable<IEnumerable<T>> CartesianProduct<T>(this IEnumerable<IEnumerable<T>> sources)
    {
        if (sources == null)
            throw new ArgumentNullException(nameof(sources));

        IEnumerable<IEnumerable<T>> emptyProduct = new[] { Enumerable.Empty<T>() };
        return sources.Aggregate(emptyProduct, (accumulator, sequence) => from accseq in accumulator from item in sequence select accseq.Concat(new[] { item }));
    }

    /// <summary>
    /// Extension method to split a sequence into sub-sequences
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    public static IEnumerable<IList<T>> Split<T>(this IEnumerable<T> source, int size)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));

        if (size <= 0)
            throw new ArgumentOutOfRangeException(nameof(size));

        T[]? items = null;
        var count = 0;
        foreach (var item in source)
        {
            if (items == null)
                items = new T[size];
            items[count++] = item;
            if (count != size)
                continue;
            yield return items;
            items = null;
            count = 0;
        }
        if (items != null && count > 0)
        {
            yield return items.Take(count).ToArray();
        }
    }

    /// <summary>
    /// Concatenate more than two sequences
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sources"></param>
    /// <returns></returns>
    public static IEnumerable<T> Concat<T>(this IEnumerable<IEnumerable<T>> sources)
    {
        return sources.SelectMany(_ => _);
    }

    /// <summary>
    /// Merge more than two sequences
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sources"></param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public static IEnumerable<T> Union<T>(this IEnumerable<IEnumerable<T>> sources, IEqualityComparer<T> comparer)
    {
        return sources.SelectMany(_ => _).Distinct(comparer);
    }

    /// <summary>
    /// Merge more than two sequences
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sources"></param>
    /// <returns></returns>
    public static IEnumerable<T> Union<T>(this IEnumerable<IEnumerable<T>> sources)
    {
        return sources.Union(EqualityComparer<T>.Default);
    }

    /// <summary>
    /// Returns Enumerable.Empty{<typeparamref name="T"/>} if source is null
    /// </summary>
    public static IEnumerable<T> OrEmptyIfNull<T>(this IEnumerable<T>? source)
    {
        return source ?? Enumerable.Empty<T>();
    }

    /// <summary>
    /// Excludes null values within the enumerable
    /// </summary>
    public static IEnumerable<T> ExceptNull<T>(this IEnumerable<T?>? source)
    {
        return source.OrEmptyIfNull()
            .Where(x => x is not null)!;
    }

    public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
    {
        return !source.OrEmptyIfNull().Any();
    }

    public static LinkedList<T> ToLinkedList<T>(this IEnumerable<T> source)
    {
        return new LinkedList<T>(source);
    }

    public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T>? source, int batchSize)
    {
        //as of .net 6, use Chunk() instead of implementing batch ourselves
        return source.OrEmptyIfNull().Chunk(batchSize);
    }

    public static T? FirstOrDefaultPreferred<T>(this IEnumerable<T> source, params Func<T, bool>[] preferences)
        where T : class
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));

        IEnumerable<T> enumerable = source as IList<T> ?? source.ToList();
        foreach (var preference in preferences)
        {
            T value = enumerable.FirstOrDefault(preference);
            if (value != null)
                return value;
        }

        return enumerable.FirstOrDefault();
    }

    //trying to use this will fail because Enumerable.Append() exists in .net framework 4.7+ - DJL 11/6/2019
    public static IEnumerable<T> Append<T>(this IEnumerable<T> source, T item)
    {
        if (source == null)
            return new[] { item };
        return source.Concat(new[] { item });
    }

    public static string StringJoin(this IEnumerable<string> s, string separator = ",")
    {
        return string.Join(separator, s);
    }

    public static string StringJoin<T>(this IEnumerable<T> ob, Func<T, string> selector, string separator = ",")
    {
        return string.Join(separator, ob.Select(selector));
    }

    public static IEnumerable<T> PadRight<T>(this IEnumerable<T> source, int length)
    {
        int i = 0;
        foreach (var item in source.Take(length))
        {
            yield return item;
            i++;
        }
        for (; i < length; i++)
            yield return default(T);
    }

    public static HashSet<T> ToHashSetSafe<T>(this IEnumerable<T> source)
    {
        var set = new HashSet<T>();
        if (source == null)
            return set;

        foreach (var t in source)
        {
            if (!set.Contains(t))
                set.Add(t);
        }
        return set;
    }

    public static IEnumerable<long> SelectParseWhereValidLong(this IEnumerable<string> source)
    {
        return source
            .OrEmptyIfNull()
            .Select(s =>
            {
                bool ok = long.TryParse(s, out long l);
                return new
                {
                    Ok = ok,
                    Value = l
                };
            })
            .Where(x => x.Ok)
            .Select(x => x.Value);
    }

    public static IEnumerable<string> ExceptNullOrWhitespace(this IEnumerable<string> source)
    {
        return source.Where(x => !string.IsNullOrWhiteSpace(x));
    }


    public static IEnumerable<T>? OrNullIfEmpty<T>(this IEnumerable<T> source)
        where T : class
    {
        return source.Any() ? source : null;
    }

    public static string ToStringJoin<T>(this IEnumerable<T> source, string joiner = "")
    {
        return ToStringJoin(source, null, joiner);
    }

    public static string ToStringJoin<T>(this IEnumerable<T> source, Func<T, string>? toString = null, string joiner = "")
    {
        if (source == null)
            return string.Empty;
        toString = toString ?? (x => x.ToString()!);
        var strings = source.Select(x => toString(x));
        return string.Join(joiner, strings);
    }

    public static bool ContainsInsensitive(this IEnumerable<string> source, string toCheck)
    {
        foreach (var x in source)
            if (x.ToLower() == toCheck.ToLower())
                return true;
        return false;
    }
}
