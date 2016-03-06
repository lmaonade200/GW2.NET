// <copyright file="WorldRepository.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.MapInformation
{
    using System;
    using System.Globalization;
    using System.Net.Http;

    using GW2NET.Caching;
    using GW2NET.Common;
    using GW2NET.Common.Converters;
    using GW2NET.Common.Messages;
    using GW2NET.MapInformation.ApiModels;
    using GW2NET.Worlds;

    /// <summary>Represents a repository that retrieves data from the /v2/worlds interface.</summary>
    public class WorldRepository : CachedRepository<int, World>, IDiscoverable<int>, ICachedRepository<int, WorldDataModel, World>, ILocalizable
    {
        /// <summary>Initializes a new instance of the <see cref="WorldRepository"/> class.</summary>
        /// <param name="httpClient">The <see cref="HttpClient"/> used to make connections with the ArenaNet servers.</param>
        /// <param name="responseConverter">The <see cref="IResponseConverter"/> used to convert <see cref="HttpResponseMessage"/> into objects.</param>
        /// <param name="cache">The <see cref="ICache{TKey, TValue}"/> used to cache api responses.</param>
        /// <param name="identifiersConverter">A converter used to convert identifiers.</param>
        /// <param name="modelConverter">A converter used to convert data contracts into objects.</param>
        /// <exception cref="ArgumentNullException">Thrown when either parameter is null.</exception>
        public WorldRepository(HttpClient httpClient, IResponseConverter responseConverter, ICache<int, World> cache, IConverter<int, int> identifiersConverter, IConverter<WorldDataModel, World> modelConverter)
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
        public CultureInfo Culture { get; set; }

        /// <inheritdoc />
        public IConverter<int, int> IdentifiersConverter { get; }

        /// <inheritdoc />
        public IConverter<WorldDataModel, World> ModelConverter { get; }

        /// <inheritdoc />
        public IParameterizedBuilder ServiceLocation
        {
            get
            {
                return ApiMessageBuilder.Init().Version(ApiVersion.V2).OnEndpoint("worlds");
            }
        }
    }
}