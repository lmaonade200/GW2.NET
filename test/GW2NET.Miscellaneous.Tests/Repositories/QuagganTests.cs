// <copyright file="QuagganTests.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Miscellaneous
{
    using System.Collections.Generic;
    using System.Linq;

    using GW2NET.Common;

    using Xunit;
    using Xunit.Abstractions;

    public class QuagganTests
    {
        private static readonly GW2Bootstrapper GW2 = new GW2Bootstrapper();

        private readonly ITestOutputHelper logger;

        public QuagganTests(ITestOutputHelper logger)
        {
            this.logger = logger;
        }

        public static IEnumerable<object[]> GetIdentifiers()
        {
            yield return new object[] { new[] { "404", "aloha", "attack" } };
        }

        [Fact]
        public async void DiscoverAsync()
        {
            var repository = GW2.Services.Quaggans;
            var result = await repository.DiscoverAsync();
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Theory]
        [InlineData("404")]
        [InlineData("aloha")]
        [InlineData("attack")]
        public async void FindAsync(string identifier)
        {
            var repository = GW2.Services.Quaggans;
            var result = await repository.FindAsync(identifier);
            Assert.NotNull(result);
            Assert.StrictEqual(identifier, result.Id);
        }

        [Fact]
        public async void FindAllAsync()
        {
            var repository = GW2.Services.Quaggans;
            var result = await repository.FindAllAsync();
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            foreach (var kvp in result)
            {
                Assert.NotNull(kvp.Value);
                Assert.StrictEqual(kvp.Key, kvp.Value.Id);
            }
        }
        
        [Theory]
        [MemberData("GetIdentifiers")]
        public async void FindAllAsync_WithIdList(string[] filter)
        {
            var repository = GW2.Services.Quaggans;
            var result = await repository.FindAllAsync(filter);
            Assert.NotNull(result);
            Assert.StrictEqual(filter.Length, result.Count);
            foreach (var identifier in filter)
            {
                Assert.NotNull(result[identifier]);
                Assert.StrictEqual(identifier, result[identifier].Id);
            }
        }
    }
}