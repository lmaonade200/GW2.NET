namespace GW2NET
{
    using System;
    using System.Linq;
    using GW2NET.Caching;

    using Xunit;

    public class MemoryCacheTests
    {
        [Fact]
        public void Clear()
        {
            var cache = new MemoryCache<int, string>();
            cache.AddToKey(0, "Item0");
            cache.AddToKey(1, "Item1");

            cache.Clear();

            Assert.StrictEqual(0, cache.Count);
        }

        [Fact]
        public void Index()
        {
            var cache = new MemoryCache<int, string>();

            cache.AddToKey(0, "Item0");
            cache.AddToKey(0, "Item01");
            cache.AddToKey(0, "Item02");
            cache.AddToKey(1, "Item1");

            Assert.StrictEqual(3, cache[0].Count);
        }

        [Fact]
        public void GetByIdNotPresent()
        {
            var cache = new MemoryCache<int, string>();

            Assert.Equal(null, cache[0]);
        }

        [Fact]
        public void Remove()
        {
            var cache = new MemoryCache<int, string>();
            cache.AddToKey(0, "Item0");
            cache.AddToKey(0, "Item01");
            cache.AddToKey(0, "Item02");
            cache.AddToKey(1, "Item1");

            cache.Remove(s => s == "Item02");

            var cacheItems = cache.ToList();

            Assert.StrictEqual(3, cacheItems.Count);
        }

        [Fact]
        public void StaleItems()
        {
            var cache = new MemoryCache<int, MockCacheItem>();
            cache.AddToKey(0, new MockCacheItem { Expires = DateTimeOffset.MinValue, Id = 0, Value = "Item0" });
            cache.AddToKey(0, new MockCacheItem { Expires = DateTimeOffset.MinValue, Id = 1, Value = "Item1" });

            Assert.StrictEqual(2, cache.GetStaleItems().Count);
        }

        [Fact]
        public void ClearStaleItems()
        {
            var cache = new MemoryCache<int, MockCacheItem>();
            cache.AddToKey(0, new MockCacheItem { Expires = DateTimeOffset.MinValue, Id = 0, Value = "Item0" });
            cache.AddToKey(0, new MockCacheItem { Expires = DateTimeOffset.MinValue, Id = 1, Value = "Item1" });

            cache.RemoveStaleItems();

            Assert.StrictEqual(0, cache.GetStaleItems().Count);
        }
    }
}
