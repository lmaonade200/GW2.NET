// <copyright file="CachedRepository.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;

    using GW2NET.Common.Converters;

    public abstract class RepositoryBase<T>
    {
        /// <summary>Initializes a new instance of the <see cref="RepositoryBase{T}"/> class.</summary>
        /// <param name="client">The <see cref="HttpClient"/> to make connections with the GW2 api.</param>
        /// <param name="responseConverter">The <see cref="ResponseConverterBase"/> converting an <see cref="HttpResponseMessage"/> for further processing.</param>
        /// <param name="cache">The cache used to store objects.</param>
        protected RepositoryBase(HttpClient client, ResponseConverterBase responseConverter)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            if (responseConverter == null)
            {
                throw new ArgumentNullException(nameof(responseConverter));
            }

            this.Client = client;
            this.ResponseConverter = responseConverter;
        }

        /// <summary>Gets the http client.</summary>
        public HttpClient Client { get; }

        /// <summary>Gets the response converter.</summary>
        public ResponseConverterBase ResponseConverter { get; }

        protected IEnumerable<int> CalculatePageSizes(int queryCount)
        {
            if (queryCount <= 200)
            {
                return new List<int> { queryCount };
            }

            return new List<int> { 200 }.Concat(this.CalculatePageSizes(queryCount - 200));
        }

        protected IEnumerable<IEnumerable<TKey>> CalculatePages<TKey>(IEnumerable<TKey> identifiers)
        {
            IList<TKey> idList = identifiers.ToList();
            IList<IEnumerable<TKey>> returnList = new List<IEnumerable<TKey>>();

            int setCount = idList.Count / 200;
            int setRemainder = idList.Count % 200;

            for (int i = 0; i < setCount; i++)
            {
                returnList.Add(idList.Skip(200 * i).Take(200));
            }

            if (setRemainder > 0)
            {
                returnList.Add(idList.Skip(200 * setCount).Take(setRemainder));
            }

            return returnList;
        }
    }
}