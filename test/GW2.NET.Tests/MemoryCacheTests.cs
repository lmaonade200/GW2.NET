namespace GW2NET
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using GW2NET.Caching;

    using Xunit;

    public class MemoryCacheTests
    {
        private ICache<int, MockCacheItem> cache;
        
        public MemoryCacheTests()
        {
            List<MockCacheItem> cacheItems = new List<MockCacheItem>
            {
                new MockCacheItem
                {
                    Expires = DateTimeOffset.MinValue, Id = 0, Value = "Item0",
                    Culture = new CultureInfo("en")
                },
                new MockCacheItem
                {
                    Expires = DateTimeOffset.UtcNow + new TimeSpan(10, 0, 0),
                    Id = 1,
                    Value = "Item1en",
                    Culture = new CultureInfo("en")
                },
                new MockCacheItem
                {
                    Expires = DateTimeOffset.UtcNow + new TimeSpan(10, 0, 0),
                    Id = 1,
                    Value = "Item1de",
                    Culture = new CultureInfo("de")
                },
                new MockCacheItem
                {
                    Expires = DateTimeOffset.UtcNow + new TimeSpan(10, 0, 0),
                    Id = 2,
                    Value = "Item3",
                    Culture = new CultureInfo("en")
                }
            };

            this.cache = new MemoryCache<int, MockCacheItem>(cacheItems.GroupBy(ks => ks.Id, ks => ks));
        }

        [Theory]
        [InlineData(1, "en")]
        [InlineData(1, "de")]
        public void IndexerWithCulture(int id, string cultureString)
        {
            MockCacheItem item = this.cache[id, new CultureInfo(cultureString)];

            Assert.NotNull(item);
            Assert.StrictEqual(id, item.Id);
            Assert.StrictEqual(new CultureInfo(cultureString), item.Culture);
        }

        [Fact]
        public void IndexerWithCultureNull()
        {
            List<Tuple<int, object>> cacheItems = new List<Tuple<int, object>>
            {
                new Tuple<int, object>(0, new object())
            };

            object expectedItem = new MemoryCache<int, object>(cacheItems.GroupBy(i => i.Item1, i => i.Item2))[0, null];

            Assert.Equal(cacheItems[0].Item2, expectedItem);
        }

        [Fact]
        public void IndexerWithCultureKeyNotPresent()
        {
            MemoryCache<int, object> memoryCache = new MemoryCache<int, object>();

            Assert.StrictEqual(null, memoryCache[0, null]);
        }

        [Theory]
        [InlineData(1, 2)]
        [InlineData(0, 0)]
        [InlineData(2, 1)]
        public void Indexer(int id, int expectedCount)
        {
            IReadOnlyCollection<MockCacheItem> items = this.cache[id];

            Assert.NotNull(items);
            Assert.StrictEqual(expectedCount, items.Count);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(0)]
        public void ContainsKey(int key)
        {
            Assert.True(this.cache.Contains(key));
        }

        [Fact]
        public void AddToNewKey()
        {
            MockCacheItem newElem = new MockCacheItem { Id = 3, Value = "Item2" };

            ICache<int, MockCacheItem> cache = this.cache.AddToKey(3, newElem);

            Assert.True(cache.Contains(3));
            Assert.StrictEqual(4, cache.Count);
        }

        [Fact]
        public void AddToExistingKey()
        {
            MockCacheItem newElem = new MockCacheItem { Id = 2, Value = "Item2" };

            ICache<int, MockCacheItem> cache = this.cache.AddToKey(2, newElem);

            Assert.StrictEqual(3, cache.Count);
        }

        [Fact]
        public void Clear()
        {
            ICache<int, MockCacheItem> newCache = this.cache.Clear();

            Assert.NotNull(newCache);
            Assert.StrictEqual(0, newCache.Count);
        }

        [Fact]
        public void ClearStaleItems()
        {
            ICache<int, MockCacheItem> newCache = this.cache.ClearStaleItems();

            Assert.NotNull(newCache);
            Assert.StrictEqual(2, newCache.Count);
        }

        [Theory]
        [InlineData(1, 2)]
        [InlineData(0, 2)]
        public void RemoveKey(int key, int expectedItemCount)
        {
            ICache<int, MockCacheItem> newCache = this.cache.RemoveKey(key);

            Assert.NotNull(newCache);
            Assert.StrictEqual(expectedItemCount, newCache.Count);
        }

        [Fact]
        public void RemoveElement()
        {
            ICache<int, MockCacheItem> newCache = this.cache.RemoveElement(i => i.Culture.Equals(new CultureInfo("en")));

            IEnumerable<MockCacheItem> allItems = newCache.SelectMany(g => g.ToList());

            Assert.StrictEqual(1, allItems.Count());
        }

        [Fact]
        public void StaleItems()
        {
            IReadOnlyCollection<MockCacheItem> staleItems = this.cache.RetrieveStaleElements();

            Assert.StrictEqual(1, staleItems.Count);
        }
    }
}
