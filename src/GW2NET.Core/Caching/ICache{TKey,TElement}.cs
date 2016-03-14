// <copyright file="ICache{TKey,TElement}.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Caching
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    /// <summary>Provides the interface for a cache used by the library services.</summary>
    /// <typeparam name="TKey">The type of identifier used to identify the items.</typeparam>
    /// <typeparam name="TElement">The type of item to store in the cache.</typeparam>
    public interface ICache<TKey, TElement> : IReadOnlyCollection<IGrouping<TKey, TElement>>
    {
        /// <summary>Retrieves a element with the specified key and culture.</summary>
        /// <param name="key">The key to indentify the item.</param>
        /// <param name="culture">The items culture.</param>
        /// <returns>The item, if it exists, otherwise the default value</returns>
        /// <exception cref="InvalidOperationException">Thrown when there is more than one item with the specified key and culture.</exception>
        TElement this[TKey key, CultureInfo culture = null] { get; }

        /// <summary>Gets a collection of elements for the specified key.</summary>
        /// <param name="key">The key to identify the collection.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of elements with the specified key.</returns>
        IReadOnlyCollection<TElement> this[TKey key] { get; }

        /// <summary>Checks if a key exists in the cache.</summary>
        /// <param name="key">The key to check.</param>
        /// <returns>True when the key exists, otherwise false.</returns>
        bool Contains(TKey key);

        /// <summary>Adds an element with the specified key to the cache.</summary>
        /// <param name="key">The key to identify the element.</param>
        /// <param name="element">The element to add.</param>
        /// <returns>A copy of the cache with the underlying collection changed.</returns>
        ICache<TKey, TElement> AddToKey(TKey key, TElement element);

        /// <summary>Adds as set of elements with the specified key to the cache.</summary>
        /// <param name="key">The key to identify the element.</param>
        /// <param name="elements">The elements to add.</param>
        /// <returns>A copy of the cache with the underlying collection changed.</returns>
        ICache<TKey, TElement> AddToKey(TKey key, IEnumerable<TElement> elements);

        /// <summary>Clears the cache.</summary>
        /// <returns>A copy of the cache with the underlying collection changed.</returns>
        ICache<TKey, TElement> Clear();

        /// <summary>Clears all stale items from the cache.</summary>
        /// <returns>A copy of the cache with the underlying collection changed.</returns>
        ICache<TKey, TElement> ClearStaleItems();

        /// <summary>Remove a key and all the corrseponding items from the cache.</summary>
        /// <param name="key"></param>
        /// <returns>A copy of the cache with the underlying collection changed.</returns>
        ICache<TKey, TElement> RemoveKey(TKey key);

        /// <summary>Removes one or more items with the specified selector from the cache.</summary>
        /// <param name="selector">The selector to identify the item(s)</param>
        /// <returns>A copy of the cache with the underlying collection changed.</returns>
        ICache<TKey, TElement> RemoveElement(Func<TElement, bool> selector);

        /// <summary>Retrives all items who are flagged as "stale" from the cache.</summary>
        /// <returns>An <see cref="IReadOnlyCollection{T}"/> with the stale elements.</returns>
        IReadOnlyCollection<TElement> RetrieveStaleElements();
    }
}