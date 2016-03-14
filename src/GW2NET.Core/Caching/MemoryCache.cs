// <copyright file="MemoryCache.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Caching
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Linq;

    using GW2NET.Common;

    /// <summary>Provides methods and properties to access cached items stored in memory.</summary>
    /// <typeparam name="TKey">They type of key to identify the items with.</typeparam>
    /// <typeparam name="TElement">The type of item to store in the cache.</typeparam>
    /// <remarks>
    /// <para>This class implements <see cref="ICache{TKey,TElement}"/> as an immutable object.</para>
    /// <para>Each operation that would change the underlying collection will return a new <see cref="MemoryCache{TKey,TElement}"/> with the changed collection.
    /// This ensures thread safety and side effect free usage.</para>
    /// </remarks>>
    public sealed class MemoryCache<TKey, TElement> : ICache<TKey, TElement>
    {
        private readonly IDictionary<TKey, HashSet<TElement>> items;

        /// <summary>Initializes a new instance of the <see cref="MemoryCache{TKey,TValue}"/> class.</summary>
        public MemoryCache()
        {
            this.items = new Dictionary<TKey, HashSet<TElement>>();
        }

        /// <summary>Initializes a new instance of the <see cref="MemoryCache{TKey,TValue}"/> class.</summary>
        /// <param name="items">The items to initialize the cache with.</param>
        public MemoryCache(IEnumerable<IGrouping<TKey, TElement>> items)
        {
            this.items = items.ToDictionary(key => key.Key, group => new HashSet<TElement>(group));
        }

        /// <summary>Initializes a new instance of the <see cref="MemoryCache{TKey,TElement}"/> class.</summary>
        /// <param name="items">The dictionary of items to use in the cache.</param>
        internal MemoryCache(IDictionary<TKey, HashSet<TElement>> items)
        {
            this.items = items;
        }

        /// <inheritdoc />
        public int Count
        {
            get
            {
                return this.items.Count;
            }
        }

        /// <inheritdoc />
        public IReadOnlyCollection<TElement> this[TKey key]
        {
            get
            {
                HashSet<TElement> items;
                return this.items.TryGetValue(key, out items) ? new ReadOnlyCollection<TElement>(items.Where(i => !this.CheckIfStale(i)).ToList()) : null;
            }
        }

        /// <inheritdoc />
        public TElement this[TKey key, CultureInfo culture = null]
        {
            get
            {
                HashSet<TElement> items;
                if (!this.items.TryGetValue(key, out items))
                {
                    return default(TElement);
                }

                if (culture == null)
                {
                    return items.SingleOrDefault();
                }

                IList<TElement> returnItems = new List<TElement>();
                foreach (TElement value in items)
                {
                    ILocalizable localeItem = value as ILocalizable;
                    if (localeItem == null)
                    {
                        throw new ArgumentException("Could not cast item to ILocalizable", nameof(localeItem));
                    }

                    if (!this.CheckIfStale(value) && localeItem.Culture.Equals(culture))
                    {
                        returnItems.Add(value);
                    }
                }

                return returnItems.SingleOrDefault();
            }
        }

        /// <inheritdoc />
        public ICache<TKey, TElement> AddToKey(TKey key, TElement element)
        {
            return this.AddToKey(key, new[] { element });
        }

        /// <inheritdoc />
        public ICache<TKey, TElement> AddToKey(TKey key, IEnumerable<TElement> elements)
        {
            Dictionary<TKey, HashSet<TElement>> dictCopy = new Dictionary<TKey, HashSet<TElement>>(this.items);
            if (!dictCopy.ContainsKey(key))
            {
                dictCopy.Add(key, new HashSet<TElement>(elements));
            }
            else
            {
                HashSet<TElement> elemUnion = dictCopy[key];
                elemUnion.UnionWith(elements);

                dictCopy[key] = elemUnion;
            }

            return new MemoryCache<TKey, TElement>(dictCopy);
        }

        /// <inheritdoc />
        public ICache<TKey, TElement> Clear()
        {
            return new MemoryCache<TKey, TElement>();
        }

        /// <inheritdoc />
        public ICache<TKey, TElement> ClearStaleItems()
        {
            return this.RemoveElement(this.CheckIfStale);
        }

        /// <inheritdoc />
        public ICache<TKey, TElement> RemoveKey(TKey key)
        {
            Dictionary<TKey, HashSet<TElement>> dictCopy = new Dictionary<TKey, HashSet<TElement>>(this.items);
            dictCopy.Remove(key);

            return new MemoryCache<TKey, TElement>(dictCopy);
        }

        /// <inheritdoc />
        public ICache<TKey, TElement> RemoveElement(Func<TElement, bool> selector)
        {
            Dictionary<TKey, HashSet<TElement>> dictCopy = new Dictionary<TKey, HashSet<TElement>>(this.items);
            foreach (KeyValuePair<TKey, HashSet<TElement>> pair in this.items)
            {
                IList<TElement> elements = pair.Value.Where(i => !selector(i)).ToList();
                if (elements.Count == 0)
                {
                    dictCopy.Remove(pair.Key);
                    continue;
                }

                dictCopy[pair.Key] = new HashSet<TElement>(elements);
            }

            return new MemoryCache<TKey, TElement>(dictCopy);
        }

        /// <inheritdoc />
        public IReadOnlyCollection<TElement> RetrieveStaleElements()
        {
            return new ReadOnlyCollection<TElement>(this.items.Values.SelectMany(l => l).Where(this.CheckIfStale).ToList());
        }

        /// <inheritdoc />
        public IEnumerator<IGrouping<TKey, TElement>> GetEnumerator()
        {
            return this.items
                   .SelectMany(p => p.Value, Tuple.Create)
                   .Where(i => !this.CheckIfStale(i.Item2))
                   .ToLookup(k => k.Item1.Key, e => e.Item2).GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <inheritdoc />
        public bool Contains(TKey key)
        {
            return this.items.ContainsKey(key);
        }

        private bool CheckIfStale(TElement item)
        {
            ITimeSensitive timesensitiveItem = item as ITimeSensitive;

            if (timesensitiveItem == null)
            {
                return false;
            }

            return timesensitiveItem.Expires.UtcDateTime < DateTimeOffset.UtcNow;
        }
    }
}
