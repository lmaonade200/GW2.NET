// <copyright file="MemoryCache.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Caching
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using GW2NET.Common;

    /// <summary>Provides methods and properties to access cached items stored in memory.</summary>
    /// <typeparam name="TKey">They type of key to identify the items with.</typeparam>
    /// <typeparam name="TValue">The type of item to store in the cache.</typeparam>
    public class MemoryCache<TKey, TValue> : ICache<TKey, TValue>
    {
        private readonly IDictionary<TKey, IList<TValue>> items;

        private readonly ICollection<TValue> staleItems;

        /// <summary>Initializes a new instance of the <see cref="MemoryCache{TKey, TValue}"/> class.</summary>
        public MemoryCache()
        {
            this.items = new Dictionary<TKey, IList<TValue>>();
            this.staleItems = new Collection<TValue>();
        }

        /// <inheritdoc />
        public IEnumerable<TValue> StaleItems
        {
            get
            {
                return this.staleItems.AsEnumerable();
            }
        }

        /// <inheritdoc />
        public void Add(TKey identifier, TValue item)
        {
            if (this.items.ContainsKey(identifier))
            {
                // ToDo: Maybe overwrite stale items per default?
                if (!this.items[identifier].Any(i => i.Equals(item)))
                {
                    this.items[identifier].Add(item);
                }
            }
            else
            {
                this.items.Add(identifier, new List<TValue>(1) { item });
            }
        }

        /// <summary>Adds an item to the cache.</summary>
        /// <param name="item">The item to add.</param>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            this.Add(item.Key, item.Value);
        }

        /// <inheritdoc />
        [SuppressMessage("ReSharper", "ParameterHidesMember", Justification = "Naming is intended.")]
        public void AddRange(IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            foreach (KeyValuePair<TKey, TValue> pair in items)
            {
                this.Add(pair.Key, pair.Value);
            }
        }

        /// <inheritdoc />
        public void Clear()
        {
            this.items.Clear();
        }

        /// <inheritdoc />
        public void ClearStaleItems()
        {
            this.UpdateStaleItems();

            this.staleItems.Clear();
        }

        /// <inheritdoc />
        public IEnumerable<TValue> GetByIdentifier(TKey identifier)
        {
            IList<TValue> items;
            return this.items.TryGetValue(identifier, out items) ? items : new List<TValue>(0);
        }

        /// <inheritdoc />
        public void Remove(Func<TValue, bool> selector)
        {
            foreach (KeyValuePair<TKey, IList<TValue>> pair in this.items)
            {
                foreach (TValue value in pair.Value.Where(selector))
                {
                    pair.Value.Remove(value);
                }
            }
        }

        /// <inheritdoc />
        public void UpdateStaleItems()
        {
            foreach (KeyValuePair<TKey, IList<TValue>> pair in this.items)
            {
                foreach (TValue value in pair.Value.Where(this.CheckIfStale))
                {
                    this.staleItems.Add(value);
                    pair.Value.Remove(value);
                }
            }
        }
        
        /// <inheritdoc />
        public IEnumerator<TValue> GetEnumerator()
        {
            foreach (IList<TValue> values in this.items.Values)
            {
                foreach (TValue value in values)
                {
                    if (this.CheckIfStale(value))
                    {
                        this.staleItems.Add(value);
                        values.Remove(value);
                    }
                    else
                    {
                        yield return value;
                    }
                }
            }
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>Checks if an item is stale.</summary>
        /// <param name="item">The item to check.</param>
        /// <returns>True when the item is stale, otherwise false.</returns>
        private bool CheckIfStale(TValue item)
        {
            ITimeSensitive timesensitiveItem = item as ITimeSensitive;

            if (timesensitiveItem == null)
            {
                return false;
            }

            return timesensitiveItem.Expires.UtcDateTime >= DateTimeOffset.UtcNow;
        }
    }
}
