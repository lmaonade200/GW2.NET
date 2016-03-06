// <copyright file="BuildRepository.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Miscellaneous
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    using GW2NET.Builds;
    using GW2NET.Common;
    using GW2NET.Common.Converters;
    using GW2NET.Common.Messages;
    using GW2NET.Miscellaneous.ApiModels;

    /// <summary>Represents a service that retrieves data from the /v1/build.json interface.</summary>
    /// <remarks>See <a href="http://wiki.guildwars2.com/wiki/API:2/build">wiki</a> for more information.</remarks>
    public sealed class BuildRepository : RepositoryBase, IBuildRepository
    {
        private readonly IConverter<BuildDataContract, Build> buildConverter;

        /// <summary>Initializes a new instance of the <see cref="BuildRepository"/> class.</summary>
        /// <param name="httpClient"></param>
        /// <param name="buildConverter">The converter for <see cref="Build"/>.</param>
        /// <param name="responseConverter"></param>
        /// <exception cref="ArgumentNullException">The value of <paramref name="httpClient"/> or <paramref name="buildConverter"/> is a null reference.</exception>
        public BuildRepository(HttpClient httpClient, IResponseConverter responseConverter, IConverter<BuildDataContract, Build> buildConverter)
            : base(httpClient, responseConverter)
        {
            if (buildConverter == null)
            {
                throw new ArgumentNullException(nameof(buildConverter));
            }

            this.buildConverter = buildConverter;
        }

        /// <inheritdoc />
        public Task<Build> GetBuildAsync()
        {
            return this.GetBuildAsync(CancellationToken.None);
        }

        /// <inheritdoc />
        public async Task<Build> GetBuildAsync(CancellationToken cancellationToken)
        {
            HttpRequestMessage request = ApiMessageBuilder.Init().Version(ApiVersion.V2).OnEndpoint("build").Build();

            return await this.ResponseConverter.ConvertElementAsync(await this.Client.SendAsync(request, cancellationToken), this.buildConverter);
        }
    }
}
