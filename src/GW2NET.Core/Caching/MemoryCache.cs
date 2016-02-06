// <copyright file="MemoryCache.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.V2.Colors.newStuff
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using GW2NET.Common;

    public class MemoryCache<T> : ICache<T>
    {
        private readonly SortedSet<T> items;

        private MemoryCache(SortedSet<T> items)
        {
            this.items = items;
        }

        public IEnumerable<T> Value
        {
            get
            {
                return this.items.Where(this.CheckIfNotStale);
            }
        }

        public static MemoryCache<T> Default()
        {
            return new MemoryCache<T>(new SortedSet<T>());
        }

        public void Add(T item)
        {
            this.items.Add(item);
        }

        public void AddRange(IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                this.Add(item);
            }
        }

        public void Clear()
        {
            this.items.Clear();
        }

        public IEnumerable<T> Get(Func<T, bool> selector, CultureInfo culture)
        {
            foreach (T item in this.items)
            {
                if (selector(item) && this.CheckIfNotStale(item))
                {
                    var localizedItem = item as ILocalizable;
                    if (localizedItem == null)
                    {
                        yield return item;
                    }
                    else
                    {
                        yield return Equals(localizedItem.Culture, culture) ? item : default(T);
                    }
                }
            }
        }

        private bool CheckIfNotStale(T item)
        {
            ITimeSensitive timesensitiveItem = item as ITimeSensitive;

            if (timesensitiveItem == null)
            {
                return true;
            }

            return timesensitiveItem.Expires < DateTimeOffset.UtcNow;
        }
    }
}
