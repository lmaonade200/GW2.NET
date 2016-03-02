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
        /// <param name="client">The <see cref="HttpClient"/> to make connections with the GW2 api.</param>
        /// <param name="responseConverter">The <see cref="IResponseConverter"/> converting an <see cref="HttpResponseMessage"/> for further processing.</param>
        /// <param name="cache">The cache used to store objects.</param>
        protected CachedRepository(HttpClient client, IResponseConverter responseConverter, ICache<TKey, TValue> cache)
            : base(client, responseConverter)
        {
            if (cache == null)
            {
                throw new ArgumentNullException(nameof(cache));
            }

            this.Cache = cache;
        }

        /// <summary>Gets ot sets the cache used to retrieve not yet obsolete objects.</summary>
        public ICache<TKey, TValue> Cache { get; }
    }
}