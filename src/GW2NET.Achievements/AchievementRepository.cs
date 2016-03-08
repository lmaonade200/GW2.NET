namespace GW2NET.Achievements
{
    using System.Net.Http;

    using GW2NET.Achievements.ApiModels;
    using GW2NET.Caching;
    using GW2NET.Common;
    using GW2NET.Common.Converters;
    using GW2NET.Common.Messages;

    public class AchievementRepository : CachedRepository<int, Achievement>, IDiscoverable<int>, ICachedRepository<int, AchievementDataModel, Achievement>
    {
        /// <summary>Initializes a new instance of the <see cref="CachedRepository{TKey,TValue}"/> class.</summary>
        /// <param name="httpClient">The <see cref="HttpClient"/> to make connections with the GW2 api.</param>
        /// <param name="responseConverter">The <see cref="IResponseConverter"/> converting an <see cref="HttpResponseMessage"/> for further processing.</param>
        /// <param name="cache">The cache used to store objects.</param>
        public AchievementRepository(HttpClient httpClient, IResponseConverter responseConverter, ICache<int, Achievement> cache)
            : base(httpClient, responseConverter, cache)
        {
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
    }
}
