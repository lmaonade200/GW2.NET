// <copyright file="MemoryCache.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Caching
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using GW2NET.Common;

    /// <summary>Provides methods and properties to access cached items stored in memory.</summary>
    /// <typeparam name="T">The type of item to store in the cache.</typeparam>
    public class MemoryCache<T> : ICache<T>
    {
        private readonly SortedSet<T> items;

        /// <summary>Initializes a new instance of the <see cref="MemoryCache{T}"/> class.</summary>
        public MemoryCache()
        {
            this.items = new SortedSet<T>();
        }

        /// <inheritdoc />
        public IEnumerable<T> Value
        {
            get
            {
                return this.items;
            }
        }

        /// <inheritdoc />
        public void Add(T item)
        {
            this.items.Add(item);
        }

        /// <inheritdoc />
        // ReSharper disable once ParameterHidesMember
        public void AddRange(IEnumerable<T> items)
        {
            this.items.UnionWith(items);
        }

        /// <inheritdoc />
        public void Clear()
        {
            this.items.Clear();
        }

        /// <inheritdoc />
        public void Remove(Func<T, bool> selector)
        {
            this.items.RemoveWhere(new Predicate<T>(selector));
        }

        /// <inheritdoc />
        public IEnumerable<T> Get(Func<T, bool> selector)
        {
            return this.items.Where(item => selector(item) && !this.CheckIfStale(item));
        }

        private bool CheckIfStale(T item)
        {
            ITimeSensitive timesensitiveItem = item as ITimeSensitive;

            if (timesensitiveItem == null)
            {
                return false;
            }

            return timesensitiveItem.Expires >= DateTimeOffset.UtcNow;
        }
    }
}
