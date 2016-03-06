// <copyright file="GuildRepository.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Guilds
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    using GW2NET.Common;
    using GW2NET.Common.Converters;
    using GW2NET.Common.Messages;
    using GW2NET.Guilds.ApiModels;

    /// <summary>Represents a repository that retrieves data from the /v1/guild_details.json interface.</summary>
    public class GuildRepository : RepositoryBase, IGuildRepository
    {
        private readonly IConverter<GuildDataModel, Guild> modelConverter;

        /// <summary>Initializes a new instance of the <see cref="GuildRepository"/> class.</summary>
        /// <param name="httpClient">The <see cref="HttpClient"/> used to make connections with the ArenaNet servers.</param>
        /// <param name="responseConverter">The <see cref="IResponseConverter"/> used to convert <see cref="HttpResponseMessage"/> into objects.</param>
        /// <param name="modelConverter">A converter used to convert data contracts into objects.</param>
        /// <exception cref="ArgumentNullException">Thrown when either parameter is null.</exception>
        public GuildRepository(HttpClient httpClient, IResponseConverter responseConverter, IConverter<GuildDataModel, Guild> modelConverter)
            : base(httpClient, responseConverter)
        {
            if (modelConverter == null)
            {
                throw new ArgumentNullException(nameof(modelConverter));
            }

            this.modelConverter = modelConverter;
        }

        /// <inheritdoc />
        public Task<Guild> FindAsync(Guid identifier)
        {
            return this.FindAsync(identifier, CancellationToken.None);
        }

        /// <inheritdoc />
        public async Task<Guild> FindAsync(Guid identifier, CancellationToken cancellationToken)
        {
            IParameterizedBuilder request = ApiMessageBuilder.Init().Version(ApiVersion.V1).OnEndpoint("guild_details.json").WithParameter("guild_id", identifier.ToString());
            return await this.ResponseConverter.ConvertElementAsync(await this.Client.SendAsync(request.Build(), cancellationToken), this.modelConverter);
        }

        /// <inheritdoc />
        public Task<Guild> FindByNameAsync(string name)
        {
            return this.FindByNameAsync(name, CancellationToken.None);
        }

        /// <inheritdoc />
        public async Task<Guild> FindByNameAsync(string name, CancellationToken cancellationToken)
        {
            IParameterizedBuilder request = ApiMessageBuilder.Init().Version(ApiVersion.V1).OnEndpoint("guild_details.json").WithParameter("guild_name", name);
            return await this.ResponseConverter.ConvertElementAsync(await this.Client.SendAsync(request.Build(), cancellationToken), this.modelConverter);
        }
    }
}