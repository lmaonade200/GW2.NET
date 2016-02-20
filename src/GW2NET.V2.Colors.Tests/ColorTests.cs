// <copyright file="ColorTests.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.V2.Colors.Tests
{
    using System.Linq;

    using Xunit;
    using Xunit.Abstractions;

    public class ColorTests
    {
        private static readonly GW2Bootstrapper GW2 = new GW2Bootstrapper();

        private readonly ITestOutputHelper logger;

        public ColorTests(ITestOutputHelper logger)
        {
            this.logger = logger;
        }

        [Fact]
        public async void DiscoverAsync()
        {
            var repository = GW2.Services.Colors;
            var result = await repository.DiscoverAsync();
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async void GetAsync(int identifier)
        {
            var repository = GW2.Services.Colors;
            var result = await repository.GetAsync(identifier);
            Assert.NotNull(result);
            Assert.StrictEqual(identifier, result.ColorId);
        }

        [Fact]
        public async void FindAllAsync()
        {
            var repository = GW2.Services.Colors;
            var result = (await repository.GetAsync()).ToList();
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            foreach (var kvp in result)
            {
                Assert.NotNull(kvp);
            }
        }

        [Theory]
        [InlineData(new[] { 1, 2, 3 })]
        public async void FindAllAsync_WithIdList(int[] filter)
        {
            var repository = GW2.Services.Colors;
            var result = (await repository.GetAsync(filter)).ToList();
            Assert.NotNull(result);
            Assert.StrictEqual(filter.Length, result.Count);
            foreach (var identifier in filter)
            {
                Assert.NotNull(result[identifier]);
                Assert.StrictEqual(identifier, result[identifier].ColorId);
            }
        }
    }
}