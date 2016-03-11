// <copyright file="RepositoryExtensions.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

#pragma warning disable CS1574
namespace GW2NET.Common
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    using GW2NET.Common.Messages;

    /// <summary>Contains extension methods for repositories of type <see cref="IRepository{TDataContract,TValue}"/>.</summary>
    public static class RepositoryExtensions
    {
        /// <summary>Gets a set of items with the specified ids from the Guild Wars 2 api.</summary>
        /// <typeparam name="TKey">The type of key used to identify items.</typeparam>
        /// <typeparam name="TDataContract">The type of data returned by the api.</typeparam>
        /// <typeparam name="TValue">The type of data to convert the api data into.</typeparam>
        /// <param name="repository">The repository used to make connections and store the data.</param>
        /// <param name="identifier">An identifier of type <see cref="TKey"/> used to identify the item.</param>
        /// <returns>An object of type <see cref="TValue"/> with data from the api.</returns>
        public static Task<TValue> GetAsync<TKey, TDataContract, TValue>(this IRepository<TDataContract, TValue> repository, TKey identifier)
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
        public static async Task<TValue> GetAsync<TKey, TDataContract, TValue>(this IRepository<TDataContract, TValue> repository, TKey identifier, CancellationToken cancellationToken)
        {
            IParameterizedBuilder request = repository.ServiceLocation;

            ILocalizable localizableRepository = repository as ILocalizable;
            if (localizableRepository != null)
            {
                request.ForCulture(localizableRepository.Culture);
            }

            request.WithIdentifier(identifier);

            // Send the request
            HttpResponseMessage response = await repository.Client.SendAsync(request.Build(), cancellationToken);

            // Convert the response and add it to the cache.
            return await repository.ResponseConverter.ConvertElementAsync(response, repository.ModelConverter);
        }

        /// <summary>Gets items from the Guild Wars 2 api.</summary>
        /// <typeparam name="TDataContract">The type of data returned by the api.</typeparam>
        /// <typeparam name="TValue">The type of data to convert the api data into.</typeparam>
        /// <param name="repository">The repository used to make connections and store the data.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of type <see cref="TValue"/> with data from the api.</returns>
        public static Task<IEnumerable<TValue>> GetAsync<TDataContract, TValue>(this IRepository<TDataContract, TValue> repository)
        {
            return GetAsync(repository, CancellationToken.None);
        }

        /// <summary>Gets items from the Guild Wars 2 api.</summary>
        /// <typeparam name="TDataContract">The type of data returned by the api.</typeparam>
        /// <typeparam name="TValue">The type of data to convert the api data into.</typeparam>
        /// <param name="repository">The repository used to make connections and store the data.</param>
        /// <param name="cancellationToken">A token signalling the cancellation of the operation.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of type <see cref="TValue"/> with data from the api.</returns>
        public static async Task<IEnumerable<TValue>> GetAsync<TDataContract, TValue>(this IRepository<TDataContract, TValue> repository, CancellationToken cancellationToken)
        {
            // Create the first page request
            IPartialCollection<TValue> firstResponse = await GetItemsFromApiAsync(0, repository, cancellationToken);

            if (firstResponse.TotalCount <= 200)
            {
                return firstResponse.AsEnumerable();
            }

            // If the total count was greater than 200 we need to do additional requests
            List<TValue> returnCollection = new List<TValue>(firstResponse.TotalCount);
            returnCollection.AddRange(firstResponse);

            int pageCount = (firstResponse.TotalCount - 200) / 200;
            if ((firstResponse.TotalCount - 200) % 200 > 0)
            {
                pageCount += 1;
            }

            IList<Task<IPartialCollection<TValue>>> queryTasks = new List<Task<IPartialCollection<TValue>>>(pageCount);
            for (int i = 0; i < pageCount; i++)
            {
                queryTasks.Add(GetItemsFromApiAsync(i + 1, repository, cancellationToken));
            }

            returnCollection.AddRange((await Task.WhenAll(queryTasks)).SelectMany(list => list));

            return returnCollection;
        }

        /// <summary>Gets a set of items with the specified ids from the Guild Wars 2 api.</summary>
        /// <typeparam name="TKey">The type of key used to identify items.</typeparam>
        /// <typeparam name="TDataContract">The type of data returned by the api.</typeparam>
        /// <typeparam name="TValue">The type of data to convert the api data into.</typeparam>
        /// <param name="repository">The repository used to make connections and store the data.</param>
        /// <param name="identifiers">An <see cref="IEnumerable{T}"/> of type <see cref="TKey"/> used to identify the items.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of type <see cref="TValue"/> with data from the api.</returns>
        public static Task<IEnumerable<TValue>> GetAsync<TKey, TDataContract, TValue>(this IRepository<TDataContract, TValue> repository, IEnumerable<TKey> identifiers)
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
        public static async Task<IEnumerable<TValue>> GetAsync<TKey, TDataContract, TValue>(this IRepository<TDataContract, TValue> repository, IEnumerable<TKey> identifiers, CancellationToken cancellationToken)
        {
            IList<TKey> ids = identifiers as IList<TKey> ?? identifiers.ToList();

            // Split the id list into a set of sets with |S| <= 200
            IEnumerable<IList<TKey>> idListList = CalculatePages(ids);

            // Query the api asnychronously with each set of ids and await all
            IEnumerable<TValue>[] result = await Task.WhenAll(idListList.Select(idList => GetItemsFromApiAsync(idList, repository, cancellationToken)));

            // Flatten the list and return a copy so the user can't change the internal list
            // ReSharper disable once PossibleMultipleEnumeration
            return result.SelectMany(list => list).ToList();
        }

        /// <summary>Calls the Guild Wars 2 Api and gets the items with the specified ids.</summary>
        /// <typeparam name="TKey">The type of key used to identify items.</typeparam>
        /// <typeparam name="TDataContract">The type of data returned by the api.</typeparam>
        /// <typeparam name="TValue">The type of data to convert the api data into.</typeparam>
        /// <param name="idList">The list of ids to query.</param>
        /// <param name="repository">The repository containing client and converters</param>
        /// <param name="cancellationToken">A token signalling the cancellation of the operation.</param>
        /// <returns>An <see cref="IPartialCollection{T}"/> of type <see cref="TValue"/> with data from the api.</returns>
        private static async Task<IPartialCollection<TValue>> GetItemsFromApiAsync<TKey, TDataContract, TValue>(IEnumerable<TKey> idList, IRepository<TDataContract, TValue> repository, CancellationToken cancellationToken)
        {
            IParameterizedBuilder request = repository.ServiceLocation;
            ILocalizable localizableRepository = repository as ILocalizable;
            if (localizableRepository != null)
            {
                request.ForCulture(localizableRepository.Culture);
            }

            request.WithIdentifiers(idList);

            return await repository.ResponseConverter.ConvertSetAsync(await repository.Client.SendAsync(request.Build(), cancellationToken), repository.ModelConverter);
        }

        /// <summary>Calls the Guild Wars 2 api and gets the items on the specified page.</summary>
        /// <typeparam name="TDataContract">The type of data returned by the api.</typeparam>
        /// <typeparam name="TValue">The type of data to convert the api data into.</typeparam>
        /// <param name="page">The page to query.</param>
        /// <param name="repository">The repository containing client and converters</param>
        /// <param name="cancellationToken">A token signalling the cancellation of the operation.</param>
        /// <returns>An <see cref="IPartialCollection{T}"/> of type <see cref="TValue"/> with data from the api.</returns>
        private static async Task<IPartialCollection<TValue>> GetItemsFromApiAsync<TDataContract, TValue>(int page, IRepository<TDataContract, TValue> repository, CancellationToken cancellationToken)
        {
            IParameterizedBuilder request = repository.ServiceLocation;
            ILocalizable localizableRepository = repository as ILocalizable;
            if (localizableRepository != null)
            {
                request.ForCulture(localizableRepository.Culture);
            }

            request.OnPage(page).WithSize(200);

            return await repository.ResponseConverter.ConvertSetAsync(await repository.Client.SendAsync(request.Build(), cancellationToken), repository.ModelConverter);
        }

        /// <summary>Creates a set of sets for api querying.</summary>
        /// <typeparam name="TKey">The type of keys in the set.</typeparam>
        /// <param name="identifiers">The identifiers to split.</param>
        /// <returns>A set containing a set with up to 200 ids to query the Guild Wars 2 api.</returns>
        private static IEnumerable<IList<TKey>> CalculatePages<TKey>(IEnumerable<TKey> identifiers)
        {
            IList<TKey> idList = identifiers.ToList();
            IList<IList<TKey>> returnList = new List<IList<TKey>>();

            int setCount = idList.Count / 200;
            int setRemainder = idList.Count % 200;

            for (int i = 0; i < setCount; i++)
            {
                returnList.Add(idList.Skip(200 * i).Take(200).ToList());
            }

            if (setRemainder > 0)
            {
                returnList.Add(idList.Skip(200 * setCount).Take(setRemainder).ToList());
            }

            return returnList;
        }
    }
}