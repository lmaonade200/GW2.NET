// <copyright file="ICache{TKey,TValue}.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Caching
{
    using System;
    using System.Collections.Generic;

    /// <summary>Provides the interface for a cache used by the library services.</summary>
    /// <typeparam name="TKey">The type of identifier used to identify the items.</typeparam>
    /// <typeparam name="TValue">The type of item to store in the cache.</typeparam>
    public interface ICache<TKey, TValue> : IEnumerable<TValue>
    {
        /// <summary>Gets all stale items in the cache.</summary>
        IEnumerable<TValue> StaleItems { get; }

        /// <summary>Adds an item to the cache.</summary>
        /// <param name="identifier">The identifier of the item to add.</param>
        /// <param name="item">The item to add.</param>
        void Add(TKey identifier, TValue item);

        /// <summary>Adds an item to the cache.</summary>
        /// <param name="item">The item to add.</param>
        void Add(KeyValuePair<TKey, TValue> item);

        /// <summary>Adds a set of items to the cache.</summary>
        /// <param name="items"></param>
        void AddRange(IEnumerable<KeyValuePair<TKey, TValue>> items);

        /// <summary>Clears the cache of all items.</summary>
        void Clear();

        /// <summary>Clears the cache of all stale items.</summary>
        void ClearStaleItems();

        /// <summary>Gets a set of items with the specified identifier from the cache.</summary>
        /// <param name="identifier">The identifier used to identify the set.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> ot type <typeparamref name="TValue"/> containing the requested items.</returns>
        IEnumerable<TValue> GetByIdentifier(TKey identifier);

        /// <summary>Removes the specified items from the cache.</summary>
        /// <param name="selector">A <see cref="Func{T, TResult}"/> selecting the items to be removed.</param>
        void Remove(Func<TValue, bool> selector);

        /// <summary>Checks all items whether they are stale.</summary>
        void UpdateStaleItems();
    }
}