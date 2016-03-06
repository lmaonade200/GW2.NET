// <copyright file="AccountRepository.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Authenticated
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    using GW2NET.Accounts;
    using GW2NET.Authenticated.ApiModels;
    using GW2NET.Caching;
    using GW2NET.Common;
    using GW2NET.Common.Converters;
    using GW2NET.Common.Messages;

    /// <summary>Represents a repository that retrieves data from the authorized /v2/account interface.</summary>
    public sealed class AccountRepository : CachedRepository<Guid, Account>, IAccountRepository
    {
        /// <summary>Converts an account data contract into an account model.</summary>
        private readonly IConverter<AccountDataContract, Account> accountConverter;

        /// <summary>Initializes a new instance of the <see cref="AccountRepository"/> class.</summary>
        /// <param name="httpClient">The client used to make requests against the api.</param>
        /// <param name="responseConverter">The response converter.</param>
        /// <param name="cache">The cache used to cache results.</param>
        /// <param name="accountConverter">The converter to convert the data contract into the appropriate model.</param>
        public AccountRepository(HttpClient httpClient, IResponseConverter responseConverter, ICache<Guid, Account> cache, IConverter<AccountDataContract, Account> accountConverter)
            : base(httpClient, responseConverter, cache)
        {
            this.accountConverter = accountConverter;
        }

        /// <inheritdoc />
        public Task<Account> GetInformationAsync()
        {
            return this.GetInformationAsync(CancellationToken.None);
        }

        /// <inheritdoc />
        public async Task<Account> GetInformationAsync(CancellationToken cancellationToken)
        {
            HttpRequestMessage request = ApiMessageBuilder.Init().Version(ApiVersion.V2).OnEndpoint("account").Build();

            return await this.ResponseConverter.ConvertElementAsync(await this.Client.SendAsync(request, cancellationToken), this.accountConverter);
        }
    }
}