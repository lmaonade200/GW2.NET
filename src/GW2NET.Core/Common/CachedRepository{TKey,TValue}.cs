// <copyright file="CachedRepository{TKey,TValue}.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Common
{
    using System;
    using System.Net.Http;

    using GW2NET.Caching;
    using GW2NET.Common.Converters;

    /// <summary>Contains methods and properties for repositories.</summary>
    /// <typeparam name="TKey">The type of key for cache items.</typeparam>
    /// <typeparam name="TValue">The type of objects used in the service.</typeparam>
    public abstract class CachedRepository<TKey, TValue> : RepositoryBase
    {
        /// <summary>Initializes a new instance of the <see cref="CachedRepository{TKey,TValue}"/> class.</summary>
        /// <param name="httpClient">The <see cref="HttpClient"/> to make connections with the GW2 api.</param>
        /// <param name="responseConverter">The <see cref="IResponseConverter"/> converting an <see cref="HttpResponseMessage"/> for further processing.</param>
        /// <param name="cache">The cache used to store objects.</param>
        protected CachedRepository(HttpClient httpClient, IResponseConverter responseConverter, ICache<TKey, TValue> cache)
            : base(httpClient, responseConverter)
        {
            if (cache == null)
            {
                throw new ArgumentNullException(nameof(cache));
            }

            this.Cache = cache;
        }

        /// <summary>Gets ot sets the cache used to retrieve not yet obsolete objects.</summary>
        public ICache<TKey, TValue> Cache { get; set; }
    }
}