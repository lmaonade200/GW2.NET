// <copyright file="AuthorizedServices.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Factories.Services
{
    using System;
    using System.Net.Http;

    using DryIoc;

    using GW2NET.Accounts;
    using GW2NET.Authenticated;
    using GW2NET.Authenticated.ApiModels;
    using GW2NET.Authenticated.Converter;
    using GW2NET.Caching;
    using GW2NET.Common;
    using GW2NET.Common.Converters;

    /// <summary>Provides access to the authorized part of the version 2 API.</summary>
    public class AuthorizedServices
    {
        private readonly Container container;

        /// <summary>Initializes a new instance of the <see cref="AuthorizedServices"/> class.</summary>
        /// <param name="container"></param>
        /// <exception cref="ArgumentNullException">The value of <paramref name="container"/> is a null reference.</exception>
        public AuthorizedServices(Container container)
        {
            this.container = container;
        }

        /// <summary>Gets access to the accounts endpoint.</summary>
        public AccountRepository Accounts
        {
            get
            {
                this.container.Register<AccountRepository>(
                    Made.Of(
                        () => new AccountRepository(
                            Arg.Of<HttpClient>("AuthenticatedRepositoryClient"),
                            Arg.Of<IResponseConverter>(),
                            Arg.Of<ICache<Guid, Account>>(),
                            Arg.Of<IConverter<AccountDataModel, Account>>())));

                this.container.Register<IConverter<AccountDataModel, Account>, AccountConverter>();

                return this.container.Resolve<AccountRepository>();
            }
        }
    }
}