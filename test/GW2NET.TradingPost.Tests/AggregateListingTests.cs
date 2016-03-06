// <copyright file="AggregateListingTests.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.TradingPost
{
    using System.Linq;

    public class AggregateListingTests
    {
        private static readonly GW2Bootstrapper GW2 = new GW2Bootstrapper();

        private readonly ITestOutputHelper logger;

        public AggregateListingTests(ITestOutputHelper logger)
        {
            this.logger = logger;
        }

        [Fact]
        public async void DiscoverAsync()
        {
            var repository = GW2.Services.Commerce.Prices;
            var result = await repository.DiscoverAsync();
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Theory]
        [InlineData(24)]
        [InlineData(68)]
        [InlineData(69)]
        public async void FindAsync(int identifier)
        {
            var repository = GW2.Services.Commerce.Prices;
            var result = await repository.GetAsync(identifier);
            Assert.NotNull(result);
            Assert.StrictEqual(identifier, result.ItemId);
        }

        [Fact]
        public async void FindAllAsync_ServiceException()
        {
            var repository = GW2.Services.Commerce.Prices;
            var exception = await Assert.ThrowsAsync<ServiceException>(() => repository.GetAsync());
            this.logger.WriteLine(exception.Message);
        }

        [Theory]
        [InlineData(new[] { 24, 68, 69 })]
        public async void FindAllAsync_WithIdList(int[] filter)
        {
            var repository = GW2.Services.Commerce.Prices;
            var result = (await repository.GetAsync(filter)).ToList();
            Assert.NotNull(result);
            Assert.StrictEqual(filter.Length, result.Count);
            foreach (var identifier in filter)
            {
                Assert.NotNull(result[identifier]);
                Assert.StrictEqual(identifier, result[identifier].ItemId);
            }
        }

        [Fact]
        public async void FindAllAsync_WithIdListTooLong_ServceException()
        {
            var repository = GW2.Services.Commerce.Prices;
            var filter = Enumerable.Range(1, 201).ToArray();
            var exception = await Assert.ThrowsAsync<ServiceException>(() => repository.GetAsync(filter));
            this.logger.WriteLine(exception.Message);
        }
    }
}