namespace GW2NET.Common
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IApiService<in TKey, TValue>
    {
        Task<TValue> GetAsync(TKey identifier);

        Task<TValue> GetAsync(TKey identifier, CancellationToken cancellationToken);

        Task<IEnumerable<TValue>> GetAsync();

        Task<IEnumerable<TValue>> GetAsync(CancellationToken cancellationToken);

        Task<IEnumerable<TValue>> GetAsync(IEnumerable<TKey> identifiers);

        Task<IEnumerable<TValue>> GetAsync(IEnumerable<TKey> identifiers, CancellationToken cancellationToken);

        Task<IEnumerable<TValue>> GetAsync(Func<TValue, bool> selector);

        Task<IEnumerable<TValue>> GetAsync(Func<TValue, bool> selector, CancellationToken cancellationToken);
    }
}