// <copyright file="ColorRepository.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.V2.Colors
{
    using System.Globalization;
    using System.Net.Http;

    using GW2NET.Caching;
    using GW2NET.Colors;
    using GW2NET.Common;
    using GW2NET.Common.Converters;
    using GW2NET.Common.Messages;

    /// <summary>Provides methods and properties to retrive colors from the GW2 api.</summary>
    public sealed class ColorRepository : CachedRepository<int, ColorPalette>, IDiscoverableNew<int>, ICachedRepository<int, ColorPaletteDataContract, ColorPalette>, ILocalizable
    {
        /// <summary>Initializes a new instance of the <see cref="ColorRepository"/> class.</summary>
        /// <param name="httpClient">The <see cref="HttpClient"/> used to make requests against the api.</param>
        /// <param name="responseConverter">A instance of the <see cref="HttpResponseConverter"/> class used to convert api responses.</param>
        /// <param name="cache">The cache used to cache results.</param>
        /// <param name="identifiersConverter">The converter used to convert identifiers.</param>
        /// <param name="modelConverter">The converter to convert single color responses.</param>
        public ColorRepository(HttpClient httpClient, IResponseConverter responseConverter, ICache<int, ColorPalette> cache, IConverter<int, int> identifiersConverter, IConverter<ColorPaletteDataContract, ColorPalette> modelConverter)
            : base(httpClient, responseConverter, cache)
        {
            this.IdentifiersConverter = identifiersConverter;
            this.ModelConverter = modelConverter;

            this.Culture = new CultureInfo("iv");
        }

        /// <inheritdoc />
        public CultureInfo Culture { get; set; }

        /// <inheritdoc />
        public IConverter<int, int> IdentifiersConverter { get; }

        /// <inheritdoc />
        public IConverter<ColorPaletteDataContract, ColorPalette> ModelConverter { get; }

        /// <inheritdoc />
        public IParameterizedBuilder ServiceLocation
        {
            get
            {
                return ApiMessageBuilder.Init().Version(ApiVersion.V2).OnEndpoint("colors");
            }
        }
    }
}
