// <copyright file="Foo.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;

    using GW2NET.Colors;
    using GW2NET.Common.Converters;
    using GW2NET.Common.Serializers;
    using GW2NET.Compression;
    using GW2NET.V2.Colors.Converters;
    using GW2NET.V2.Colors.newStuff;

    using Xunit;

    public class Foo
    {
        [Fact]
        public async void DiscoverTest()
        {
            HttpClient client = new HttpClient(new GW2ApiHandler(), false)
            {
                BaseAddress = new Uri("https://api.guildwars2.com/")
            };

            JsonSerializerFactory jsonSerializerFactory = new JsonSerializerFactory();
            GzipInflator gzipInflator = new GzipInflator();

            HttpResponseConverter httpResponseConverter = new HttpResponseConverter(jsonSerializerFactory, gzipInflator);

            ConverterAdapter<IEnumerable<int>> idConverter = new ConverterAdapter<IEnumerable<int>>();
            var colorConverter = new ColorPaletteConverter(new ColorConverter(), new ColorModelConverter(new ColorConverter()));

            ColorService service = new ColorService(client, httpResponseConverter, MemoryCache<ColorPalette>.Default(), idConverter, colorConverter);

            IEnumerable<int> ids = await service.DiscoverAsync();
            
            Assert.NotEmpty(ids);
        }
    }
}