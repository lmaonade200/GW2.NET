// <copyright file="ICachedRepository{TKey,TDataContract,TValue}.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Common
{
    using GW2NET.Caching;

    /// <summary>Provides the interface to query objects from the Guild Wars 2 api with a cache as backing storage.</summary>
    /// <typeparam name="TKey">The type of key used to identify items in the cache.</typeparam>
    /// <typeparam name="TDataContract">The type of date returned by the api.</typeparam>
    /// <typeparam name="TValue">The type of data to convert into.</typeparam>
    public interface ICachedRepository<TKey, in TDataContract, TValue> : IRepository<TDataContract, TValue>
    {
        /// <summary>Gets the cache used as backing storage.</summary>
        ICache<TKey, TValue> Cache { get; }
    }
}
