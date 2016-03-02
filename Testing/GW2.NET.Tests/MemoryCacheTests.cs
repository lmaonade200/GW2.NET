namespace GW2NET
{
    using System.Linq;
    using GW2NET.Caching;
    using Xunit;

    public class MemoryCacheTests
    {
        [Fact]
        public void AddTest()
        {
            var cache = new MemoryCache<int, string> { { 0, "Item0" }, { 1, "Item1" } };

            Assert.StrictEqual(2, cache.Count());
        }
    }
}
