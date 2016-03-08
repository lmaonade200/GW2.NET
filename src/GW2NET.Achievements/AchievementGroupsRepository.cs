// <copyright file="AchievementGroupsRepository.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Achievements
{
    using System;
    using System.Net.Http;

    using GW2NET.Achievements.ApiModels;
    using GW2NET.Caching;
    using GW2NET.Common;
    using GW2NET.Common.Converters;
    using GW2NET.Common.Messages;

    /// <summary>Provides access to to the achievement groups at the Guild Wars 2 api.</summary>
    public class AchievementGroupRepository : CachedRepository<Guid, Group>, IDiscoverable<string, Guid>, ICachedRepository<Guid, AchievementGroupDataModel, Group>
    {
        /// <summary>Initializes a new instance of the <see cref="AchievementGroupsRepository"/> class.</summary>
        /// <param name="httpClient">The <see cref="HttpClient"/> to make connections with the GW2 api.</param>
        /// <param name="responseConverter">The <see cref="IResponseConverter"/> converting an <see cref="HttpResponseMessage"/> for further processing.</param>
        /// <param name="cache">The cache used to store objects.</param>
        /// <param name="identifiersConverter">A converter used to convert identifiers.</param>
        /// <param name="modelConverter">A converter used to convert data contracts into objects.</param>
        /// <exception cref="ArgumentNullException">Thrown when either parameter is null.</exception>
        public AchievementGroupRepository(HttpClient httpClient, IResponseConverter responseConverter, ICache<Guid, Group> cache, IConverter<string, Guid> identifiersConverter, IConverter<AchievementGroupDataModel, Group> modelConverter)
            : base(httpClient, responseConverter, cache)
        {
            if (identifiersConverter == null)
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
        public IConverter<string, Guid> IdentifiersConverter { get; }

        /// <inheritdoc />
        public IConverter<AchievementGroupDataModel, Group> ModelConverter { get; }

        /// <inheritdoc />
        public IParameterizedBuilder ServiceLocation
        {
            get
            {
                return ApiMessageBuilder.Init().Version(ApiVersion.V2).OnEndpoint("achievements/groups");
            }
        }
    }
}
