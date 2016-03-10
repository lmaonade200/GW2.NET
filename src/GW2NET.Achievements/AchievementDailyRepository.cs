using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW2NET.Achievements
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading;

    using GW2NET.Achievements.ApiModels;
    using GW2NET.Common;
    using GW2NET.Common.Converters;
    using GW2NET.Common.Messages;

    public class AchievementDailyRepository : RepositoryBase
    {
        /// <summary>Initializes a new instance of the <see cref="AchievementDailyRepository"/> class.</summary>
        /// <param name="httpClient">The <see cref="HttpClient"/> to make connections with the GW2 api.</param>
        /// <param name="responseConverter">The <see cref="IResponseConverter"/> converting an <see cref="HttpResponseMessage"/> for further processing.</param>
        /// <param name="modelConverter">A converter used to convert the daily achivement model.</param>
        /// <exception cref="ArgumentNullException">Thrown when either parameter is null.</exception>
        public AchievementDailyRepository(HttpClient httpClient, IResponseConverter responseConverter, IConverter<DailyAchievementsDataModel, IEnumerable<DailyAchievement>> modelConverter)
            : base(httpClient, responseConverter)
        {
            if (modelConverter == null)
            {
                throw new ArgumentNullException(nameof(modelConverter));
            }

            this.ModelConverter = modelConverter;
        }

        /// <summary>Gets the <see cref="IConverter{TSource,TTarget}"/> used to convert identifiers.</summary>
        public IConverter<DailyAchievementsDataModel, IEnumerable<DailyAchievement>> ModelConverter { get; }

        /// <summary>Gets the service location without any additional paramters (i.e. culture, identifiers, etc.)</summary>
        public IParameterizedBuilder ServiceLocation
        {
            get
            {
                return ApiMessageBuilder.Init().Version(ApiVersion.V2).OnEndpoint("achievements/daily");
            }
        }

        public Task<IEnumerable<DailyAchievement>> Get()
        {
            return this.Get(CancellationToken.None);
        }

        private async Task<IEnumerable<DailyAchievement>> Get(CancellationToken cancellationToken)
        {
            return await this.ResponseConverter.ConvertElementAsync(await this.Client.SendAsync(this.ServiceLocation.Build(), cancellationToken), this.ModelConverter);
        }
    }
}
