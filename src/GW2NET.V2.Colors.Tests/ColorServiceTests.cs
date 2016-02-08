// <copyright file="Foo.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading;

    using GW2NET.Caching;
    using GW2NET.Colors;
    using GW2NET.Common;
    using GW2NET.Common.Converters;
    using GW2NET.Common.Serializers;
    using GW2NET.Compression;
    using GW2NET.V2.Colors;
    using GW2NET.V2.Colors.Converters;

    using Xunit;

    public class ColorServiceTests
    {
        [Fact]
        public async void DiscoverTest()
        {
            HttpClient client = new HttpClient(new HttpClientHandler(), false)
            {
                BaseAddress = new Uri("https://api.guildwars2.com/")
            };

            JsonSerializerFactory jsonSerializerFactory = new JsonSerializerFactory();
            GzipInflator gzipInflator = new GzipInflator();

            HttpResponseConverter httpResponseConverter = new HttpResponseConverter(jsonSerializerFactory, gzipInflator);

            IConverter<int, int> idConverter = new ConverterAdapter<int>();
            ColorPaletteConverter colorConverter = new ColorPaletteConverter(new ColorConverter(), new ColorModelConverter(new ColorConverter()));

            ColorService service = new ColorService(client, httpResponseConverter, new MemoryCache<ColorPalette>(), idConverter, colorConverter);

            IEnumerable<int> ids = await service.DiscoverAsync();

            Assert.NotEmpty(ids);
        }

        [Fact]
        public async void ElementTest()
        {
            HttpClient client = new HttpClient(new HttpClientHandler(), false)
            {
                BaseAddress = new Uri("https://api.guildwars2.com/")
            };

            JsonSerializerFactory jsonSerializerFactory = new JsonSerializerFactory();
            GzipInflator gzipInflator = new GzipInflator();

            HttpResponseConverter httpResponseConverter = new HttpResponseConverter(jsonSerializerFactory, gzipInflator);

            IConverter<int, int> idConverter = new ConverterAdapter<int>();
            ColorPaletteConverter colorConverter = new ColorPaletteConverter(new ColorConverter(), new ColorModelConverter(new ColorConverter()));

            ColorService service = new ColorService(client, httpResponseConverter, new MemoryCache<ColorPalette>(), idConverter, colorConverter);

            ColorPalette color = await service.GetAsync(10, CancellationToken.None);

            Assert.NotNull(color);
            Assert.Equal(color.ColorId, 10);
        }
    }
}