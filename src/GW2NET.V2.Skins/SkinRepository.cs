// <copyright file="SkinRepository.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.V2.Skins
{
    using System;
    using System.Globalization;
    using System.Net.Http;

    using GW2NET.Caching;
    using GW2NET.Common;
    using GW2NET.Common.Converters;
    using GW2NET.Common.Messages;
    using GW2NET.Skins;
    using GW2NET.V2.Skins.Json;

    /// <summary>Represents a repository that retrieves data from the /v2/items interface. See the remarks section for important limitations regarding this implementation.</summary>
    /// <remarks>
    /// This implementation does not retrieve associated entities.
    /// </remarks>
    public sealed class SkinRepository : CachedRepository<int, Skin>, IDiscoverable<int>, ICachedRepository<int, SkinDTO, Skin>, ILocalizable
    {
        /// <summary>Initializes a new instance of the <see cref="SkinRepository"/> class.</summary>
        /// <param name="httpClient">The <see cref="HttpClient"/> used to make connections with the ArenaNet servers.</param>
        /// <param name="responseConverter">The <see cref="IResponseConverter"/> used to convert <see cref="HttpResponseMessage"/> into objects.</param>
        /// <param name="cache">The <see cref="ICache{TKey, TValue}"/> used to cache api responses.</param>
        /// <param name="identifiersConverter">A converter used to convert identifiers.</param>
        /// <param name="modelConverter">A converter used to convert data contracts into objects.</param>
        /// <exception cref="ArgumentNullException">Thrown when either parameter is null.</exception>
        public SkinRepository(HttpClient httpClient, IResponseConverter responseConverter, ICache<int, Skin> cache, IConverter<int, int> identifiersConverter, IConverter<SkinDTO, Skin> modelConverter)
            : base(httpClient, responseConverter, cache)
        {
            if (identifiersConverter == null)
            {
                throw new ArgumentNullException(nameof(modelConverter));
            }

            if (modelConverter == null)
            {
                throw new ArgumentNullException(nameof(modelConverter));
            }

            this.IdentifiersConverter = identifiersConverter;
            this.ModelConverter = modelConverter;
        }

        /// <inheritdoc />
        CultureInfo ILocalizable.Culture { get; set; }

        /// <inheritdoc />
        public IConverter<int, int> IdentifiersConverter { get; }

        /// <inheritdoc />
        public IConverter<SkinDTO, Skin> ModelConverter { get; }

        /// <inheritdoc />
        public IParameterizedBuilder ServiceLocation
        {
            get
            {
                return ApiMessageBuilder.Init().Version(ApiVersion.V2).OnEndpoint("skins");
            }
        }
    }
}