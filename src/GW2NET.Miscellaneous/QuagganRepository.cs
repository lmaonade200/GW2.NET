// <copyright file="QuagganRepository.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Miscellaneous
{
    using System;
    using System.Net.Http;

    using GW2NET.Caching;
    using GW2NET.Common;
    using GW2NET.Common.Converters;
    using GW2NET.Common.Messages;
    using GW2NET.Miscellaneous.ApiModels;
    using GW2NET.Quaggans;

    /// <summary>Represents a repository that retrieves data from the /v2/quaggans interface.</summary>
    public class QuagganRepository : CachedRepository<string, Quaggan>, IDiscoverable<string>, ICachedRepository<string, QuagganDataContract, Quaggan>
    {
        /// <summary>Initializes a new instance of the <see cref="QuagganRepository"/> class.</summary>
        /// <param name="httpClient">The <see cref="HttpClient"/> used to make connections with the ArenaNet servers.</param>
        /// <param name="responseConverter">The <see cref="IResponseConverter"/> used to convert <see cref="HttpResponseMessage"/> into objects.</param>
        /// <param name="cache">The <see cref="ICache{TKey, TValue}"/> used to cache api responses.</param>
        /// <param name="identifiersConverter">A converter used to convert identifiers.</param>
        /// <param name="modelConverter">A converter used to convert data contracts into objects.</param>
        /// <exception cref="ArgumentNullException">Thrown when either parameter is null.</exception>
        public QuagganRepository(HttpClient httpClient, IResponseConverter responseConverter, ICache<string, Quaggan> cache, IConverter<string, string> identifiersConverter, IConverter<QuagganDataContract, Quaggan> modelConverter)
            : base(httpClient, responseConverter, cache)
        {
            if (identifiersConverter == null)
            {
                throw new ArgumentNullException(nameof(identifiersConverter));
            }

            if (responseConverter == null)
            {
                throw new ArgumentNullException(nameof(modelConverter));
            }

            this.IdentifiersConverter = identifiersConverter;
            this.ModelConverter = modelConverter;
        }


        /// <inheritdoc />
        public IConverter<string, string> IdentifiersConverter { get; }

        /// <inheritdoc />
        public IConverter<QuagganDataContract, Quaggan> ModelConverter { get; }

        /// <inheritdoc />
        public IParameterizedBuilder ServiceLocation
        {
            get
            {
                return ApiMessageBuilder.Init().Version(ApiVersion.V2).OnEndpoint("quaggans");
            }
        }
    }
}