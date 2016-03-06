// <copyright file="IApiService.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Common
{
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
    }
}