namespace GW2NET.Caching
{
    using System;
    using System.Collections.Generic;

    /// <summary>Provides the interface for a cache used by the library services.</summary>
    /// <typeparam name="T">The type of item to store in the cache.</typeparam>
    public interface ICache<T>
    {
        /// <summary>Gets all items in the cache, even if they are stale.</summary>
        IEnumerable<T> Value { get; }

        /// <summary>Adds an item to the cache.</summary>
        /// <param name="item"></param>
        void Add(T item);

        /// <summary>Adds a set of items to the cache.</summary>
        /// <param name="items"></param>
        void AddRange(IEnumerable<T> items);

        /// <summary>Clears the cache of all items.</summary>
        void Clear();

        /// <summary>Gets a set of items from the cache.</summary>
        /// <param name="selector">A <see cref="Func{T,TResult}"/> to filter the items to return.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of type <see cref="T"/> containing the requested items.</returns>
        IEnumerable<T> Get(Func<T, bool> selector);
    }
}