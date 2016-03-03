// <copyright file="CharacterRepository.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.V2.Accounts.Characters
{
    using System;
    using System.Net.Http;

    using GW2NET.Caching;
    using GW2NET.Characters;
    using GW2NET.Common;
    using GW2NET.Common.Converters;
    using GW2NET.Common.Messages;

    /// <summary>Represents a repository that retrieves data from the /v2/characters interface.</summary>
    public sealed class CharacterRepository : CachedRepository<string, Character>, IDiscoverable<string>, ICachedRepository<string, CharacterDataContract, Character>
    {
        /// <summary>Initializes a new instance of the <see cref="CharacterRepository"/> class.</summary>
        /// <param name="client"></param>
        /// <param name="cache"></param>
        /// <param name="identifiersConverter"></param>
        /// <param name="responseConverter"></param>
        /// <param name="modelConverter"></param>
        /// <exception cref="ArgumentNullException">Thrown when any constructor argument is null.</exception>
        public CharacterRepository(HttpClient client, IResponseConverter responseConverter, ICache<string, Character> cache, IConverter<string, string> identifiersConverter, IConverter<CharacterDataContract, Character> modelConverter)
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

            if (modelConverter == null)
            {
                throw new ArgumentNullException(nameof(responseConverter));
            }

            this.IdentifiersConverter = identifiersConverter;
            this.ModelConverter = modelConverter;
        }

        /// <inheritdoc />
        public IConverter<string, string> IdentifiersConverter { get; }

        /// <inheritdoc />
        public IConverter<CharacterDataContract, Character> ModelConverter { get; }

        /// <inheritdoc />
        public IParameterizedBuilder ServiceLocation
        {
            get
            {
                return ApiMessageBuilder.Init().Version(ApiVersion.V2).OnEndpoint("characters");
            }
        }
    }
}
