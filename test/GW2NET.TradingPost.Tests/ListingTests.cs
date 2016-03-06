namespace GW2NET.TradingPost
{
    using System.Collections.Generic;
    using System.Linq;

    public class ListingTests
    {
        private static readonly GW2Bootstrapper GW2 = new GW2Bootstrapper();

        private readonly ITestOutputHelper logger;

        public ListingTests(ITestOutputHelper logger)
        {
            this.logger = logger;
        }

        [Fact]
        public async void DiscoverAsync()
        {
            ListingRepository repository = GW2.Services.Commerce.Listings;
            IEnumerable<int> result = await repository.DiscoverAsync();
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Theory]
        [InlineData(24)]
        [InlineData(68)]
        [InlineData(69)]
        public async void FindAsync(int identifier)
        {
            ListingRepository repository = GW2.Services.Commerce.Listings;
            Listing result = await repository.GetAsync(identifier);
            Assert.NotNull(result);
            Assert.StrictEqual(identifier, result.ItemId);
        }


        [Fact]
        public async void FindAllAsync_ServiceException()
        {
            ListingRepository repository = GW2.Services.Commerce.Listings;
            ServiceException exception = await Assert.ThrowsAsync<ServiceException>(() => repository.GetAsync());
            this.logger.WriteLine(exception.Message);
        }

        [Theory]
        [InlineData(new[] { 24, 68, 69 })]
        public async void FindAllAsync_WithIdList(int[] filter)
        {
            ListingRepository repository = GW2.Services.Commerce.Listings;
            List<Listing> result = (await repository.GetAsync(filter)).ToList();
            Assert.NotNull(result);
            Assert.StrictEqual(filter.Length, result.Count);
            foreach (int identifier in filter)
            {
                Assert.NotNull(result[identifier]);
                Assert.StrictEqual(identifier, result[identifier].ItemId);
            }
        }

        [Fact]
        public async void FindAllAsync_WithIdListTooLong_ServceException()
        {
            ListingRepository repository = GW2.Services.Commerce.Listings;
            int[] filter = Enumerable.Range(1, 201).ToArray();
            ServiceException exception = await Assert.ThrowsAsync<ServiceException>(() => repository.GetAsync(filter));
            this.logger.WriteLine(exception.Message);
        }
    }
}