namespace GW2NET
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GW2NET.Caching;
    using GW2NET.Common;

    using Xunit;

    public class MemoryCacheTests
    {
        [Fact]
        public void AddPair()
        {
            var cache = new MemoryCache<int, string>();

            cache.Add(new KeyValuePair<int, string>(0, "Item0"));
            cache.Add(new KeyValuePair<int, string>(1, "Item1"));

            Assert.StrictEqual(2, cache.Count());
        }

        [Fact]
        public void AddRange()
        {
            var cache = new MemoryCache<int, string>();

            cache.AddRange(new List<KeyValuePair<int, string>>
            {
                new KeyValuePair<int, string>(0, "Item0"),
                new KeyValuePair<int, string>(1, "Item1")
            });

            Assert.StrictEqual(2, cache.Count());
        }

        [Fact]
        public void Clear()
        {
            var cache = new MemoryCache<int, string>();

            cache.AddRange(new List<KeyValuePair<int, string>>
            {
                new KeyValuePair<int, string>(0, "Item0"),
                new KeyValuePair<int, string>(1, "Item1")
            });

            cache.Clear();

            Assert.StrictEqual(0, cache.Count());
        }

        [Fact]
        public void GetById()
        {
            var cache = new MemoryCache<int, string>();

            cache.AddRange(new List<KeyValuePair<int, string>>
            {
                new KeyValuePair<int, string>(0, "Item0"),
                new KeyValuePair<int, string>(0, "Item01"),
                new KeyValuePair<int, string>(0, "Item02"),
                new KeyValuePair<int, string>(1, "Item1")
            });

            Assert.StrictEqual(3, cache.GetByIdentifier(0).Count());
        }

        [Fact]
        public void GetByIdNotPresent()
        {
            var cache = new MemoryCache<int, string>();

            Assert.Equal(0, cache.GetByIdentifier(0).Count());
        }

        [Fact]
        public void Remove()
        {
            var cache = new MemoryCache<int, string>
            {
                new KeyValuePair<int, string>(0, "Item0"),
                new KeyValuePair<int, string>(0, "Item01"),
                new KeyValuePair<int, string>(0, "Item02"),
                new KeyValuePair<int, string>(1, "Item1")
            };

            cache.Remove(s => s == "Item02");

            var cacheItems = cache.ToList();

            Assert.Equal(
                new List<string>
                {
                    "Item0",
                    "Item01",
                    "Item1"
                },
                cacheItems);

            Assert.StrictEqual(3, cacheItems.Count);
        }

        [Fact]
        public void StaleItems()
        {
            var cache = new MemoryCache<int, MockCacheItem>
            {
                { 0, new MockCacheItem { Expires = DateTimeOffset.MinValue, Id = 0, Value = "Item0" } },
                { 0, new MockCacheItem { Expires = DateTimeOffset.MinValue, Id = 1, Value = "Item1" } }
            };

            cache.UpdateStaleItems();

            Assert.StrictEqual(2, cache.StaleItems.Count());
        }

        [Fact]
        public void ClearStaleItems()
        {
            var cache = new MemoryCache<int, MockCacheItem>
            {
                { 0, new MockCacheItem { Expires = DateTimeOffset.MinValue, Id = 0, Value = "Item0" } },
                { 0, new MockCacheItem { Expires = DateTimeOffset.MinValue, Id = 1, Value = "Item1" } }
            };

            cache.ClearStaleItems();

            Assert.StrictEqual(0, cache.StaleItems.Count());
        }
    }

    public class MockCacheItem : ITimeSensitive
    {
        public int Id { get; set; }

        public string Value { get; set; }

        /// <summary>Gets or sets a timestamp.</summary>
        public DateTimeOffset Timestamp { get; set; }

        public DateTimeOffset Expires { get; set; }
    }
}
