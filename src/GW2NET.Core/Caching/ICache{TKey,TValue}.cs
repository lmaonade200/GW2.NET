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
    public interface ICache<TKey, TValue> : IDictionary<TKey, IList<TValue>>
    {
        /// <summary>Adds an item to the cache.</summary>
        /// <param name="key">The identifier of the item to add.</param>
        /// <param name="item">The item to add.</param>
        void AddToKey(TKey key, TValue item);

        /// <summary>Enumerates all stalte items.</summary>
        /// <returns>A <see cref="IList{T}"/> of all stale items.</returns>
        IList<TValue> GetStaleItems();

        /// <summary>Removes the specified items from the cache.</summary>
        /// <param name="selector">A <see cref="Func{T, TResult}"/> selecting the items to be removed.</param>
        void Remove(Func<TValue, bool> selector);

        /// <summary>Clears the cache of all stale items.</summary>
        void RemoveStaleItems();
    }
}