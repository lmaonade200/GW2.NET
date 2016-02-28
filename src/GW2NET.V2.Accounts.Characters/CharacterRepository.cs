// <copyright file="CharacterRepository.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.V2.Accounts.Characters
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    using GW2NET.Caching;
    using GW2NET.Characters;
    using GW2NET.Common;
    using GW2NET.Common.Converters;
    using GW2NET.Common.Messages;

    /// <summary>Represents a repository that retrieves data from the /v2/characters interface.</summary>
    public sealed class CharacterRepository : CachedRepository<Character>, IDiscoverService<string>, IApiService<string, Character>
    {
        private readonly IConverter<string, string> identifiersConverter;

        private readonly IConverter<CharacterDataContract, Character> characterConverter;

        /// <summary>Initializes a new instance of the <see cref="CharacterRepository"/> class.</summary>
        /// <param name="client"></param>
        /// <param name="cache"></param>
        /// <param name="identifiersConverter"></param>
        /// <param name="responseConverter"></param>
        /// <param name="characterConverter"></param>
        /// <exception cref="ArgumentNullException">Thrown when any constructor argument is null.</exception>
        public CharacterRepository(HttpClient client, ResponseConverterBase responseConverter, ICache<Character> cache, IConverter<string, string> identifiersConverter, IConverter<CharacterDataContract, Character> characterConverter)
            : base(client, responseConverter, cache)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            if (identifiersConverter == null)
            {
                throw new ArgumentNullException(nameof(identifiersConverter));
            }

            if (characterConverter == null)
            {
                throw new ArgumentNullException(nameof(responseConverter));
            }

            this.identifiersConverter = identifiersConverter;
            this.characterConverter = characterConverter;
        }

        /// <inheritdoc />
        public CultureInfo Culture { get; set; }

        /// <inheritdoc />
        public Task<IEnumerable<string>> DiscoverAsync()
        {
            return this.DiscoverAsync(CancellationToken.None);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<string>> DiscoverAsync(CancellationToken cancellationToken)
        {
            HttpRequestMessage request = ApiMessageBuilder.Init().Version(ApiVersion.V2).OnEndpoint("characters").Build();

            return await this.ResponseConverter.ConvertSetAsync(await this.Client.SendAsync(request, cancellationToken), this.identifiersConverter);
        }

        /// <inheritdoc />
        public Task<Character> GetAsync(string identifier)
        {
            return this.GetAsync(identifier, CancellationToken.None);
        }

        /// <inheritdoc />
        public async Task<Character> GetAsync(string identifier, CancellationToken cancellationToken)
        {
            Character cacheChar = this.Cache.Get(c => c.Name == identifier).SingleOrDefault();
            if (cacheChar != null)
            {
                return cacheChar;
            }

            HttpRequestMessage request = ApiMessageBuilder.Init().Version(ApiVersion.V2).OnEndpoint("characters").ForCulture(this.Culture).WithIdentifier(identifier).Build();

            return await this.ResponseConverter.ConvertElementAsync(await this.Client.SendAsync(request, cancellationToken), this.characterConverter);
        }

        /// <inheritdoc />
        public Task<IEnumerable<Character>> GetAsync()
        {
            return this.GetAsync(CancellationToken.None);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Character>> GetAsync(CancellationToken cancellationToken)
        {
            IEnumerable<IEnumerable<string>> idListList = this.CalculatePages((await this.DiscoverAsync(cancellationToken)).SymmetricExcept(this.Cache.Value.Select(c => c.Name)));

            ConcurrentBag<Character> characters = new ConcurrentBag<Character>();
            Parallel.ForEach(idListList,
                             async idList =>
                             {
                                 HttpRequestMessage request = ApiMessageBuilder.Init()
                                 .Version(ApiVersion.V2)
                                 .OnEndpoint("characters")
                                 .WithIdentifiers(idList)
                                 .Build();

                                 HttpResponseMessage response = await this.Client.SendAsync(request, cancellationToken);

                                 foreach (Character character in await this.ResponseConverter.ConvertSetAsync(response, this.characterConverter))
                                 {
                                     characters.Add(character);
                                 }
                             });

            return characters;
        }

        /// <inheritdoc />
        public Task<IEnumerable<Character>> GetAsync(IEnumerable<string> identifiers)
        {
            return this.GetAsync(identifiers, CancellationToken.None);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Character>> GetAsync(IEnumerable<string> identifiers, CancellationToken cancellationToken)
        {
            IList<string> ids = identifiers as IList<string> ?? identifiers.ToList();
            var cacheChars = this.Cache.Get(c => ids.All(i => i != c.Name)).ToList();

            if (cacheChars.Count == ids.Count)
            {
                return cacheChars;
            }

            // If the id count is greater than 200 we need to partitionate
            IEnumerable<IEnumerable<string>> idsToQuery = this.CalculatePages(ids.Where(x => cacheChars.All(i => i.Name != x)));

            ConcurrentBag<Character> characters = new ConcurrentBag<Character>(cacheChars);

            Parallel.ForEach(idsToQuery,
                async idList =>
                {
                    HttpRequestMessage request = ApiMessageBuilder.Init()
                                         .Version(ApiVersion.V2)
                                         .OnEndpoint("colors")
                                         .ForCulture(this.Culture)
                                         .WithIdentifiers(idList)
                                         .Build();

                    HttpResponseMessage response = await this.Client.SendAsync(request, cancellationToken);

                    foreach (Character color in await this.ResponseConverter.ConvertSetAsync(response, this.characterConverter))
                    {
                        characters.Add(color);
                    }
                });

            return characters;
        }
    }
}
