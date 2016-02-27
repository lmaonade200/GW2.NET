namespace GW2NET.Common
{
    using System;
    using System.Net.Http;

    using GW2NET.Caching;
    using GW2NET.Common.Converters;

    /// <summary>Contains methods and properties for repositories.</summary>
    /// <typeparam name="T">The type of objects used in the service.</typeparam>
    public abstract class CachedRepository<T> : RepositoryBase<T>
    {
        /// <summary>Initializes a new instance of the <see cref="CachedRepository{T}"/> class.</summary>
        /// <param name="client">The <see cref="HttpClient"/> to make connections with the GW2 api.</param>
        /// <param name="responseConverter">The <see cref="ResponseConverterBase"/> converting an <see cref="HttpResponseMessage"/> for further processing.</param>
        /// <param name="cache">The cache used to store objects.</param>
        protected CachedRepository(HttpClient client, ResponseConverterBase responseConverter, ICache<T> cache)
            : base(client, responseConverter)
        {
            if (cache == null)
            {
                throw new ArgumentNullException(nameof(cache));
            }

            this.Cache = cache;
        }

        /// <summary>Gets ot sets the cache used to retrieve not yet obsolete objects.</summary>
        public ICache<T> Cache { get; }
    }
}