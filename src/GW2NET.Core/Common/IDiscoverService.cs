namespace GW2NET.Common
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IDiscoverService<T>
    {
        Task<IEnumerable<T>> DiscoverAsync();

        Task<IEnumerable<T>> DiscoverAsync(CancellationToken cancellationToken);
    }
}