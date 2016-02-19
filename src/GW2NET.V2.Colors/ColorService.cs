// <copyright file="ColorService.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.V2.Colors
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    using GW2NET.Caching;
    using GW2NET.Colors;
    using GW2NET.Common;
    using GW2NET.Common.Converters;
    using GW2NET.Common.Messages;

    /// <summary>Provides methods and properties to retrive colors from the GW2 api.</summary>
    public class ColorService : ServiceBase<ColorPalette>, IDiscoverService<int>, IApiService<int, ColorPalette>, ILocalizable
    {
        private readonly IConverter<int, int> identifiersConverter;

        private readonly IConverter<ColorPaletteDataContract, ColorPalette> colorConverter;

        /// <summary>Initializes a new instance of the <see cref="ColorService"/> class.</summary>
        /// <param name="httpClient">The <see cref="HttpClient"/> used to make requests against the api.</param>
        /// <param name="responseConverter">A instance of the <see cref="HttpResponseConverter"/> class used to convert api responses.</param>
        /// <param name="cache">The cache used to cache results.</param>
        /// <param name="identifiersConverter">The converter used to convert identifiers.</param>
        /// <param name="colorConverter">The converter to convert single color responses.</param>
        public ColorService(HttpClient httpClient, ResponseConverterBase responseConverter, ICache<ColorPalette> cache, IConverter<int, int> identifiersConverter, IConverter<ColorPaletteDataContract, ColorPalette> colorConverter)
            : base(httpClient, responseConverter, cache)
        {
            this.identifiersConverter = identifiersConverter;
            this.colorConverter = colorConverter;

            this.Culture = new CultureInfo("iv");
        }

        /// <inheritdoc />
        public CultureInfo Culture { get; set; }

        /// <inheritdoc />
        public Task<IEnumerable<int>> DiscoverAsync()
        {
            return this.DiscoverAsync(CancellationToken.None);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<int>> DiscoverAsync(CancellationToken cancellationToken)
        {
            HttpRequestMessage request = ApiMessageBuilder.Init().Version(ApiVersion.V2).OnEndpoint("colors").Build();
            HttpResponseMessage response = await this.Client.SendAsync(request, cancellationToken);

            return await this.ResponseConverter.ConvertSetAsync(response, this.identifiersConverter);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<ColorPalette>> GetAsync(CancellationToken cancellationToken)
        {
            IEnumerable<ColorPalette> cacheColors = this.Cache.Get(cp => cp.Culture.Equals(this.Culture));

            IEnumerable<IEnumerable<int>> idListList = this.CalculatePages((await this.DiscoverAsync(cancellationToken)).SymmetricExcept(cacheColors.Select(c => c.ColorId)));

            ConcurrentBag<ColorPalette> colors = new ConcurrentBag<ColorPalette>();
            Parallel.ForEach(idListList,
                             async idList =>
                             {
                                 HttpRequestMessage request = ApiMessageBuilder.Init()
                                                      .Version(ApiVersion.V2)
                                                      .OnEndpoint("colors")
                                                      .ForCulture(this.Culture)
                                                      .WithIdentifiers(idList)
                                                      .Build();
                                 HttpResponseMessage response = await this.Client.SendAsync(request, cancellationToken);

                                 foreach (ColorPalette color in await this.ResponseConverter.ConvertSetAsync(response, this.colorConverter))
                                 {
                                     colors.Add(color);
                                 }
                             });

            return colors;
        }

        /// <inheritdoc />
        public Task<IEnumerable<ColorPalette>> GetAsync(CancellationToken cancellationToken, params int[] identifiers)
        {
            return this.GetAsync(identifiers, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<ColorPalette>> GetAsync(IEnumerable<int> identifiers, CancellationToken cancellationToken)
        {
            IList<int> ids = identifiers as IList<int> ?? identifiers.ToList();
            IList<ColorPalette> cacheColors = this.Cache.Get(c => ids.All(i => i != c.ColorId) && c.Culture.Equals(this.Culture)).ToList();

            if (cacheColors.Count == ids.Count)
            {
                return cacheColors;
            }

            // If the id count is greater than 200 we need to partitionate
            IEnumerable<IEnumerable<int>> idsToQuery = this.CalculatePages(ids.Where(x => cacheColors.All(i => i.ItemId != x)));

            ConcurrentBag<ColorPalette> colors = new ConcurrentBag<ColorPalette>(cacheColors);

            Parallel.ForEach(idsToQuery,
                async idList =>
                {
                    HttpRequestMessage request =
                        ApiMessageBuilder.Init()
                                         .Version(ApiVersion.V2)
                                         .OnEndpoint("colors")
                                         .ForCulture(this.Culture)
                                         .WithIdentifiers(idList)
                                         .Build();

                    HttpResponseMessage response = await this.Client.SendAsync(request, cancellationToken);

                    foreach (ColorPalette color in await this.ResponseConverter.ConvertSetAsync(response, this.colorConverter))
                    {
                        colors.Add(color);
                    }
                });

            return colors;
        }

        /// <inheritdoc />
        public async Task<ColorPalette> GetAsync(int identifier, CancellationToken cancellationToken)
        {
            // Check if the requested object is in the cache
            ColorPalette color = this.Cache.Get(c => c.ColorId == identifier && c.Culture.Equals(this.Culture)).SingleOrDefault();

            if (color != null)
            {
                return color;
            }

            // If the opbject is not in the cache, request it from the api
            HttpRequestMessage request = ApiMessageBuilder.Init()
                .Version(ApiVersion.V2)
                .OnEndpoint("colors")
                .ForCulture(this.Culture)
                .WithIdentifier(identifier)
                .Build();

            // Send the request
            HttpResponseMessage response = await this.Client.SendAsync(request, cancellationToken);

            // Convert the response and return the object
            return await this.ResponseConverter.ConvertElementAsync(response, this.colorConverter);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<ColorPalette>> GetAsync(Func<ColorPalette, bool> selector, CancellationToken cancellationToken)
        {
            var cacheColors = this.Cache.Get(selector);

            var colorIdListList = this.CalculatePages((await this.DiscoverAsync(cancellationToken)).SymmetricExcept(cacheColors.Select(c => c.ColorId)));

            ConcurrentBag<ColorPalette> colors = new ConcurrentBag<ColorPalette>();
            Parallel.ForEach(
                             colorIdListList,
                             async colorIdList =>
                             {
                                 foreach (var message in typeof(ApiCultures)
                                    .AsEnumerable<CultureInfo>()
                                    .Select(culture => ApiMessageBuilder.Init()
                                        .Version(ApiVersion.V2)
                                        .OnEndpoint("colors")
                                        .ForCulture(culture)
                                        .WithIdentifiers(colorIdList)
                                        .Build()))
                                 {
                                     HttpResponseMessage response = await this.Client.SendAsync(message, cancellationToken);

                                     IEnumerable<ColorPalette> responseColors = await this.ResponseConverter.ConvertSetAsync(response, this.colorConverter);

                                     foreach (ColorPalette color in responseColors.Where(selector))
                                     {
                                         colors.Add(color);
                                     }
                                 }
                             });

            return colors;
        }
    }
}
