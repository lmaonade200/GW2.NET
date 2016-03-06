namespace GW2NET.Miscellaneous
{
    using Xunit;

    public class WorldTests
    {
        private static readonly GW2Bootstrapper GW2 = new GW2Bootstrapper();

        [Fact]
        public async void DiscoverAsync()
        {
            var repository = GW2.Services.Worlds.ForDefaultCulture();
            var result = await repository.DiscoverAsync();
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Theory]
        [InlineData(1001)]
        [InlineData(1002)]
        [InlineData(1003)]
        public async void FindAsync(int identifier)
        {
            var repository = GW2.Services.Worlds.ForDefaultCulture();
            var result = await repository.FindAsync(identifier);
            Assert.NotNull(result);
            Assert.StrictEqual(identifier, result.WorldId);
        }
        
        [Fact]
        public async void FindAllAsync()
        {
            var repository = GW2.Services.Worlds.ForDefaultCulture();
            var result = await repository.FindAllAsync();
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            foreach (var kvp in result)
            {
                Assert.NotNull(kvp.Value);
                Assert.StrictEqual(kvp.Key, kvp.Value.WorldId);
            }
        }

        [Theory]
        [InlineData(new[] { 1001, 1002, 1003 })]
        public async void FindAllAsync_WithIdList(int[] filter)
        {
            var repository = GW2.Services.Worlds.ForDefaultCulture();
            var result = await repository.FindAllAsync(filter);
            Assert.NotNull(result);
            Assert.StrictEqual(filter.Length, result.Count);
            foreach (var identifier in filter)
            {
                Assert.NotNull(result[identifier]);
                Assert.StrictEqual(identifier, result[identifier].WorldId);
            }
        }
    }
}