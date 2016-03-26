// <copyright file="CachedRepositoryExtensions.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

#pragma warning disable CS1574
namespace GW2NET.Common
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using GW2NET.Caching;

    /// <summary>Contains extension methods for repositories of type <see cref="IRepository{TDataContract,TValue}"/> with an <see cref="ICache{TKey,TValue}"/> as backing storage.</summary>
    public static class CachedRepositoryExtensions
    {
        /// <summary>Gets a set of items with the specified ids from the Guild Wars 2 api.</summary>
        /// <typeparam name="TKey">The type of key used to identify items.</typeparam>
        /// <typeparam name="TDataContract">The type of data returned by the api.</typeparam>
        /// <typeparam name="TValue">The type of data to convert the api data into.</typeparam>
        /// <param name="repository">The repository used to make connections and store the data.</param>
        /// <param name="identifier">An identifier of type <see cref="TKey"/> used to identify the item.</param>
        /// <returns>An object of type <see cref="TValue"/> with data from the api.</returns>
        public static Task<TValue> GetAsync<TKey, TDataContract, TValue>(this ICachedRepository<TKey, TDataContract, TValue> repository, TKey identifier)
        {
            return GetAsync(repository, identifier, CancellationToken.None);
        }

        /// <summary>Gets a set of items with the specified ids from the Guild Wars 2 api.</summary>
        /// <typeparam name="TKey">The type of key used to identify items.</typeparam>
        /// <typeparam name="TDataContract">The type of data returned by the api.</typeparam>
        /// <typeparam name="TValue">The type of data to convert the api data into.</typeparam>
        /// <param name="repository">The repository used to make connections and store the data.</param>
        /// <param name="identifier">An identifier of type <see cref="TKey"/> used to identify the item.</param>
        /// <param name="cancellationToken">A token signalling the cancellation of the operation.</param>
        /// <returns>An object of type <see cref="TValue"/> with data from the api.</returns>
        public static async Task<TValue> GetAsync<TKey, TDataContract, TValue>(this ICachedRepository<TKey, TDataContract, TValue> repository, TKey identifier, CancellationToken cancellationToken)
        {
            ILocalizable localizableRepository = repository as ILocalizable;
            TValue item = localizableRepository != null
                              ? repository.Cache[identifier].SingleOrDefault(i => Equals(((ILocalizable)i).Culture, localizableRepository.Culture))
                              : repository.Cache[identifier].SingleOrDefault();

            // To avoid boxing, the best way to compare generics for equality is with EqualityComparer<T>.Default.
            // This respects IEquatable<T> (without boxing) as well as object.Equals, and handles all the Nullable<T> "lifted" nuances.
            // See: http://stackoverflow.com/questions/65351/null-or-default-comparison-of-generic-argument-in-c-sharp
            if (!EqualityComparer<TValue>.Default.Equals(item, default(TValue)))
            {
                return item;
            }

            TValue apiItem = await ((IRepository<TDataContract, TValue>)repository).GetAsync(identifier, cancellationToken);

            repository.Cache = repository.Cache.AddToKey(identifier, item);

            return apiItem;
        }

        /// <summary>Gets items from the Guild Wars 2 api.</summary>
        /// <typeparam name="TKey">The type of key used to identify items.</typeparam>
        /// <typeparam name="TDataContract">The type of data returned by the api.</typeparam>
        /// <typeparam name="TValue">The type of data to convert the api data into.</typeparam>
        /// <param name="repository">The repository used to make connections and store the data.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of type <see cref="TValue"/> with data from the api.</returns>
        public static Task<IEnumerable<TValue>> GetAsync<TKey, TDataContract, TValue>(this ICachedRepository<TKey, TDataContract, TValue> repository)
        {
            return GetAsync(repository, CancellationToken.None);
        }

        /// <summary>Gets items from the Guild Wars 2 api.</summary>
        /// <typeparam name="TKey">The type of key used to identify items.</typeparam>
        /// <typeparam name="TDataContract">The type of data returned by the api.</typeparam>
        /// <typeparam name="TValue">The type of data to convert the api data into.</typeparam>
        /// <param name="repository">The repository used to make connections and store the data.</param>
        /// <param name="cancellationToken">A token signalling the cancellation of the operation.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of type <see cref="TValue"/> with data from the api.</returns>
        public static async Task<IEnumerable<TValue>> GetAsync<TKey, TDataContract, TValue>(this ICachedRepository<TKey, TDataContract, TValue> repository, CancellationToken cancellationToken)
        {
            IEnumerable<TKey> serviceIds = await ((IDiscoverable<TKey>)repository).DiscoverAsync(cancellationToken);

            return await repository.GetAsync(serviceIds, cancellationToken);
        }

        /// <summary>Gets a set of items with the specified ids from the Guild Wars 2 api.</summary>
        /// <typeparam name="TKey">The type of key used to identify items.</typeparam>
        /// <typeparam name="TDataContract">The type of data returned by the api.</typeparam>
        /// <typeparam name="TValue">The type of data to convert the api data into.</typeparam>
        /// <param name="repository">The repository used to make connections and store the data.</param>
        /// <param name="identifiers">An <see cref="IEnumerable{T}"/> of type <see cref="TKey"/> used to identify the items.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of type <see cref="TValue"/> with data from the api.</returns>
        public static Task<IEnumerable<TValue>> GetAsync<TKey, TDataContract, TValue>(this ICachedRepository<TKey, TDataContract, TValue> repository, IEnumerable<TKey> identifiers)
        {
            return GetAsync(repository, identifiers, CancellationToken.None);
        }

        /// <summary>Gets a set of items with the specified ids from the Guild Wars 2 api.</summary>
        /// <typeparam name="TKey">The type of key used to identify items.</typeparam>
        /// <typeparam name="TDataContract">The type of data returned by the api.</typeparam>
        /// <typeparam name="TValue">The type of data to convert the api data into.</typeparam>
        /// <param name="repository">The repository used to make connections and store the data.</param>
        /// <param name="identifiers">An <see cref="IEnumerable{T}"/> of type <see cref="TKey"/> used to identify the items.</param>
        /// <param name="cancellationToken">A token signalling the cancellation of the operation.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of type <see cref="TValue"/> with data from the api.</returns>
        public static async Task<IEnumerable<TValue>> GetAsync<TKey, TDataContract, TValue>(this ICachedRepository<TKey, TDataContract, TValue> repository, IEnumerable<TKey> identifiers, CancellationToken cancellationToken)
        {
            IList<TKey> ids = identifiers as IList<TKey> ?? identifiers.ToList();

            Dictionary<TKey, TValue> itemList = new Dictionary<TKey, TValue>(ids.Count);
            ILocalizable localizableRepository = repository as ILocalizable;
            foreach (TKey id in ids)
            {
                TValue item = localizableRepository != null
                                  ? repository.Cache[id].SingleOrDefault(i => Equals(((ILocalizable)i).Culture, localizableRepository.Culture))
                                  : repository.Cache[id].SingleOrDefault();

                // To avoid boxing, the best way to compare generics for equality is with EqualityComparer<T>.Default.
                // This respects IEquatable<T> (without boxing) as well as object.Equals, and handles all the Nullable<T> "lifted" nuances.
                // See: http://stackoverflow.com/questions/65351/null-or-default-comparison-of-generic-argument-in-c-sharp
                if (!EqualityComparer<TValue>.Default.Equals(item, default(TValue)))
                {
                    itemList.Add(id, item);
                }
            }

            // If the count of both lists is the same we can simply return
            if (itemList.Count == ids.Count)
            {
                // Make sure we always return a fresh list so the user cannot modify the collection in this method
                return itemList.Select(i => i.Value).ToList();
            }

            // Get the ids to query. Order it, so we can safely zip them later.
            IEnumerable<TKey> idsToQuery = itemList.Keys.SymmetricExcept(ids).OrderBy(i => i);

            Task<IEnumerable<TValue>> apiItemsTask = ((IRepository<TDataContract, TValue>)repository).GetAsync(idsToQuery, cancellationToken);

            IList<KeyValuePair<TKey, TValue>> itemsDictionary = idsToQuery.Zip(await apiItemsTask, (id, item) => new KeyValuePair<TKey, TValue>(id, item)).ToList();

            // Add items to the cache
            foreach (KeyValuePair<TKey, TValue> item in itemsDictionary)
            {
                repository.Cache.AddToKey(item.Key, item.Value);
            }

            // Make sure we always return a fresh list so the user cannot modify the collection in this method
            return itemsDictionary.Select(i => i.Value).Union(itemList.Values).ToList();
        }
    }
}