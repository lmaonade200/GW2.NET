// <copyright file="CurrencyExchange.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.V2.Commerce.Exchange
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    using GW2NET.Commerce;
    using GW2NET.Common;
    using GW2NET.Common.Converters;
    using GW2NET.Common.Messages;

    /// <summary>Represents a currency exchange service that retrieves data from the /v2/commerce/exchange interface.</summary>
    public sealed class CurrencyExchange : RepositoryBase, ICurrencyExchange
    {
        private readonly IConverter<ExchangeDataContract, Exchange> exchangeConverter;

        /// <summary>Initializes a new instance of the <see cref="CurrencyExchange"/> class.</summary>
        /// <param name="httpClient">The client used to make requests against the api.</param>
        /// <param name="responseConverter">The converter used to convert the <see cref="HttpResponseMessage"/>.</param>
        /// <param name="exchangeConverter">The connverter used to convert data contracts into actual objects.</param>
        public CurrencyExchange(HttpClient httpClient, IResponseConverter responseConverter, IConverter<ExchangeDataContract, Exchange> exchangeConverter)
            : base(httpClient, responseConverter)
        {
            if (exchangeConverter == null)
            {
                throw new ArgumentNullException(nameof(exchangeConverter));
            }

            this.exchangeConverter = exchangeConverter;
        }

        /// <inheritdoc />
        public Task<Exchange> GetCoinsAsync(int gems)
        {
            return this.GetCoinsAsync(gems, CancellationToken.None);
        }

        /// <inheritdoc />
        public async Task<Exchange> GetCoinsAsync(int gems, CancellationToken cancellationToken)
        {
            HttpRequestMessage request = ApiMessageBuilder.Init().Version(ApiVersion.V2).OnEndpoint("commerce/exchange/gems").WithQuantity(gems).Build();

            Exchange exchange = await this.ResponseConverter.ConvertElementAsync(await this.Client.SendAsync(request, cancellationToken), this.exchangeConverter);

            // Patch the quantity because it is not a property of the response object
            exchange.Send = gems;

            return exchange;
        }

        /// <inheritdoc />
        public Task<Exchange> GetGemsAsync(int coins)
        {
            return this.GetGemsAsync(coins, CancellationToken.None);
        }

        /// <inheritdoc />
        public async Task<Exchange> GetGemsAsync(int coins, CancellationToken cancellationToken)
        {
            HttpRequestMessage request = ApiMessageBuilder.Init().Version(ApiVersion.V2).OnEndpoint("commerce/exchange/coins").WithQuantity(coins).Build();

            Exchange exchange = await this.ResponseConverter.ConvertElementAsync(await this.Client.SendAsync(request, cancellationToken), this.exchangeConverter);

            // Patch the quantity because it is not a property of the response object
            exchange.Send = coins;

            return exchange;
        }
    }
}