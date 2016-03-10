// <copyright file="AchievementRepository.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Achievements
{
    using System;
    using System.Globalization;
    using System.Net.Http;

    using GW2NET.Achievements.ApiModels;
    using GW2NET.Caching;
    using GW2NET.Common;
    using GW2NET.Common.Converters;
    using GW2NET.Common.Messages;

    /// <summary>Provides access to to the achievements at the Guild Wars 2 api.</summary>
    public class AchievementRepository : CachedRepository<int, Achievement>, IDiscoverable<int>, ICachedRepository<int, AchievementDataModel, Achievement>, ILocalizable
    {
        /// <summary>Initializes a new instance of the <see cref="AchievementRepository"/> class.</summary>
        /// <param name="httpClient">The <see cref="HttpClient"/> to make connections with the GW2 api.</param>
        /// <param name="responseConverter">The <see cref="IResponseConverter"/> converting an <see cref="HttpResponseMessage"/> for further processing.</param>
        /// <param name="cache">The cache used to store objects.</param>
        /// <param name="identifiersConverter">A converter used to convert identifiers.</param>
        /// <param name="modelConverter">A converter used to convert data contracts into objects.</param>
        /// <exception cref="ArgumentNullException">Thrown when either parameter is null.</exception>
        public AchievementRepository(HttpClient httpClient, IResponseConverter responseConverter, ICache<int, Achievement> cache, IConverter<int, int> identifiersConverter, IConverter<AchievementDataModel, Achievement> modelConverter)
            : base(httpClient, responseConverter, cache)
        {
            if (this.IdentifiersConverter == null)
            {
                throw new ArgumentNullException(nameof(identifiersConverter));
            }

            if (modelConverter == null)
            {
                throw new ArgumentNullException(nameof(modelConverter));
            }

            this.IdentifiersConverter = identifiersConverter;
            this.ModelConverter = modelConverter;
        }

        /// <inheritdoc />
        public IConverter<int, int> IdentifiersConverter { get; }

        /// <inheritdoc />
        public IConverter<AchievementDataModel, Achievement> ModelConverter { get; }

        /// <inheritdoc />
        public IParameterizedBuilder ServiceLocation
        {
            get
            {
                return ApiMessageBuilder.Init().Version(ApiVersion.V2).OnEndpoint("achievements");
            }
        }

        /// <inheritdoc />
        public CultureInfo Culture { get; set; }
    }
}
