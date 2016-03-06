// <copyright file="ColorTests.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Miscellaneous
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using GW2NET.Colors;
    using GW2NET.Common;
    using GW2NET.Miscellaneous.ApiModels;

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
            ColorRepository repository = GW2.Services.Colors;
            IEnumerable<int> result = await repository.DiscoverAsync();
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async void GetAsync(int identifier)
        {
            ColorRepository repository = GW2.Services.Colors;
            ColorPalette result = await repository.GetAsync(identifier);
            Assert.NotNull(result);
            Assert.StrictEqual(identifier, result.ColorId);
        }

        [Fact]
        public async void FindAllAsync()
        {
            ColorRepository repository = GW2.Services.Colors;
            List<ColorPalette> result = (await repository.GetAsync<int, ColorPaletteDataModel, ColorPalette>()).ToList();
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            foreach (ColorPalette kvp in result)
            {
                Assert.NotNull(kvp);
            }
        }

        [Theory]
        [InlineData(new[] { 1, 2, 3 })]
        public async void FindAllAsync_WithIdList(int[] filter)
        {
            ColorRepository repository = GW2.Services.Colors;
            IList<ColorPalette> result = (await repository.GetAsync<int, ColorPaletteDataModel, ColorPalette>(filter, CancellationToken.None)).OrderBy(e => e.ColorId).ToList();
            Assert.NotNull(result);
            Assert.StrictEqual(filter.Length, result.Count);

            for (int i = 0; i < filter.Length; i++)
            {
                Assert.NotNull(result[i]);
                Assert.StrictEqual(filter[i], result[i].ColorId);
            }
        }
    }
}