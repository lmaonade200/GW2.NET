﻿// <copyright file="RepositoryBase.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Common
{
    using System;
    using System.Net.Http;

    using GW2NET.Common.Converters;

    public abstract class RepositoryBase
    {
        /// <summary>Initializes a new instance of the <see cref="RepositoryBase"/> class.</summary>
        /// <param name="httpClient">The <see cref="System.Net.Http.HttpClient"/> to make connections with the GW2 api.</param>
        /// <param name="responseConverter">The <see cref="IResponseConverter"/> converting an <see cref="HttpResponseMessage"/> for further processing.</param>
        protected RepositoryBase(HttpClient httpClient, IResponseConverter responseConverter)
        {
            if (httpClient == null)
            {
                throw new ArgumentNullException(nameof(httpClient));
            }

            if (responseConverter == null)
            {
                throw new ArgumentNullException(nameof(responseConverter));
            }

            this.Client = httpClient;
            this.ResponseConverter = responseConverter;
        }

        /// <summary>Gets the http client.</summary>
        public HttpClient Client { get; }

        /// <summary>Gets the response converter.</summary>
        public IResponseConverter ResponseConverter { get; }
    }
}