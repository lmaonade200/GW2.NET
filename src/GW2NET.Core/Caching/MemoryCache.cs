// <copyright file="MemoryCache.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Caching
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using GW2NET.Common;

    /// <summary>Provides methods and properties to access cached items stored in memory.</summary>
    /// <typeparam name="TKey">They type of key to identify the items with.</typeparam>
    /// <typeparam name="TValue">The type of item to store in the cache.</typeparam>
    public class MemoryCache<TKey, TValue> : ICache<TKey, TValue>
    {
        private readonly IDictionary<TKey, IList<TValue>> items;

        /// <summary>Initializes a new instance of the <see cref="MemoryCache{TKey,TValue}"/> class.</summary>
        public MemoryCache()
        {
            this.items = new Dictionary<TKey, IList<TValue>>();
        }

        /// <summary>Initializes a new instance of the <see cref="MemoryCache{TKey,TValue}"/> class.</summary>
        /// <param name="items">The items to initialize the cache with.</param>
        public MemoryCache(IDictionary<TKey, IList<TValue>> items)
        {
            this.items = items;
        }

        /// <inheritdoc />
        public IEnumerator<KeyValuePair<TKey, IList<TValue>>> GetEnumerator()
        {
            return this.items.GetEnumerator();
        }

        /// <inheritdoc />
        public IList<TValue> GetStaleItems()
        {
            return this.items.Values.SelectMany(l => l).Where(this.CheckIfStale).ToList();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <inheritdoc />
        public void Add(KeyValuePair<TKey, IList<TValue>> item)
        {
            this.items.Add(item);
        }

        /// <inheritdoc />
        public void Clear()
        {
            this.items.Clear();
        }

        /// <inheritdoc />
        public bool Contains(KeyValuePair<TKey, IList<TValue>> item)
        {
            return this.items.Contains(item);
        }

        /// <inheritdoc />
        public void CopyTo(KeyValuePair<TKey, IList<TValue>>[] array, int arrayIndex)
        {
            this.items.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc />
        public bool Remove(KeyValuePair<TKey, IList<TValue>> item)
        {
            return this.items.Remove(item);
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
        public bool IsReadOnly
        {
            get
            {
                return this.items.IsReadOnly;
            }
        }

        /// <inheritdoc />
        public void Add(TKey key, IList<TValue> value)
        {
            this.items.Add(key, value);
        }

        /// <inheritdoc />
        public ICollection<TKey> Keys
        {
            get
            {
                return this.items.Keys;
            }
        }

        /// <inheritdoc />
        public ICollection<IList<TValue>> Values
        {
            get
            {
                return this.items.Values;
            }
        }

        /// <inheritdoc />
        public IList<TValue> this[TKey key]
        {
            get
            {
                IList<TValue> items;
                return this.TryGetValue(key, out items) ? items : null;
            }

            set
            {
                this.items.Add(key, value);
            }
        }

        /// <inheritdoc />
        public bool ContainsKey(TKey key)
        {
            return this.items.ContainsKey(key);
        }

        /// <inheritdoc />
        public bool Remove(TKey key)
        {
            return this.items.Remove(key);
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
        public bool TryGetValue(TKey key, out IList<TValue> value)
        {
            IList<TValue> items;
            this.items.TryGetValue(key, out items);

            IList<TValue> freshItems = items.Where(i => !this.CheckIfStale(i)).ToList();

            if (freshItems.Any())
            {
                value = freshItems;
                return true;
            }

            value = null;
            return false;
        }

        /// <inheritdoc />
        public void AddToKey(TKey key, TValue item)
        {
            if (this.items.ContainsKey(key))
            {
                if (!this.items[key].Any(i => i.Equals(item)))
                {
                    this.items[key].Add(item);
                }
                else
                {
                    throw new ArgumentException("The cache contained the item already.", nameof(item));
                }
            }
            else
            {
                this.items.Add(key, new List<TValue>(1) { item });
            }
        }

        /// <inheritdoc />
        public void RemoveStaleItems()
        {
            foreach (IList<TValue> value in this.items.Values)
            {
                foreach (TValue item in value.Where(this.CheckIfStale))
                {
                    value.Remove(item);
                }
            }
        }

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
