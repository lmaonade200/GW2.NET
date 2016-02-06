// <copyright file="ColorService.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.V2.Colors.newStuff
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    using GW2NET.Colors;
    using GW2NET.Common;
    using GW2NET.Common.Messages;
    using GW2NET.V2.Colors.Json;

    public class ColorService : ServiceBase<ColorPalette>, IDiscoverService<int>, IApiService<int, ColorPalette>, ILocalizable
    {
        private readonly IConverter<IEnumerable<int>, IEnumerable<int>> identifiersConverter;

        private readonly IConverter<ColorPaletteDTO, ColorPalette> colorConverter;

        private readonly IConverter<IEnumerable<ColorPaletteDTO>, IEnumerable<ColorPalette>> colorsConverter;

        public ColorService(
            HttpClient httpClient,
            HttpResponseConverter responseConverter,
            ICache<ColorPalette> cache,
            IConverter<IEnumerable<int>, IEnumerable<int>> identifiersConverter,
            IConverter<ColorPaletteDTO, ColorPalette> colorConverter)
            : base(httpClient, responseConverter, cache)
        {
            this.identifiersConverter = identifiersConverter;
            this.colorConverter = colorConverter;
        }

        /// <summary>Gets or sets the locale.</summary>
        public CultureInfo Culture { get; set; }

        public Task<IEnumerable<int>> DiscoverAsync()
        {
            return this.DiscoverAsync(CancellationToken.None);
        }

        public async Task<IEnumerable<int>> DiscoverAsync(CancellationToken cancellationToken)
        {
            HttpRequestMessage request = ApiMessageBuilder.Init().Version(ApiVersion.V2).OnEndpoint("colors").Build();
            HttpResponseMessage response = await this.Client.SendAsync(request, cancellationToken);

            return await this.ResponseConverter.ConvertAsync(response, this.identifiersConverter, cancellationToken);
        }

        public Task<IEnumerable<ColorPalette>> GetAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ColorPalette>> GetAsync(CancellationToken cancellationToken, params int[] identifiers)
        {
            return this.GetAsync(identifiers, cancellationToken);
        }

        public async Task<IEnumerable<ColorPalette>> GetAsync(IEnumerable<int> identifiers, CancellationToken cancellationToken)
        {
            IEnumerable<ColorPalette> colors = this.Cache.Get(c => identifiers.All(i => i != c.ColorId), this.Culture);

            if (colors != null)
            {
                return colors;
            }

            RequestBuilder requestBuilder = new RequestBuilder(ApiVersion.V2, "colors");
            requestBuilder.QueryParameters.SetIdentifiers(identifiers);

            var response = await this.Client.SendAsync(requestBuilder.Request, cancellationToken);

            return await this.ResponseConverter.ConvertAsync(response, this.colorsConverter, cancellationToken);
        }

        public async Task<ColorPalette> GetAsync(int identifier, CancellationToken cancellationToken)
        {
            // Check if the requested object is in the cache
            ColorPalette color = this.Cache.Get(c => c.ColorId == identifier, this.Culture).SingleOrDefault();

            if (color != null)
            {
                return color;
            }

            // If the opbject is not in the cache, request it from the api
            RequestBuilder requestBuilder = new RequestBuilder(ApiVersion.V2, "colors");
            requestBuilder.QueryParameters.SetIdentifier(identifier);

            // Send the request
            HttpResponseMessage response = await this.Client.SendAsync(requestBuilder.Request, cancellationToken);

            // Convert the response and return the object
            return await this.ResponseConverter.ConvertAsync(response, this.colorConverter, cancellationToken);
        }
    }
}
