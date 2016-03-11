﻿// <copyright file="CharacterRepository.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Authenticated
{
    using System;
    using System.Net.Http;

    using GW2NET.Authenticated.ApiModels;
    using GW2NET.Caching;
    using GW2NET.Characters;
    using GW2NET.Common;
    using GW2NET.Common.Converters;
    using GW2NET.Common.Messages;

    /// <summary>Represents a repository that retrieves data from the /v2/characters interface.</summary>
    public sealed class CharacterRepository : CachedRepository<string, Character>, IDiscoverable<string>, ICachedRepository<string, CharacterDataModel, Character>
    {
        /// <summary>Initializes a new instance of the <see cref="CharacterRepository"/> class.</summary>
        /// <param name="httpClient">The <see cref="HttpClient"/> used to make connections with the ArenaNet servers.</param>
        /// <param name="responseConverter">The <see cref="IResponseConverter"/> used to convert <see cref="HttpResponseMessage"/> into objects.</param>
        /// <param name="cache">The <see cref="ICache{TKey, TValue}"/> used to cache api responses.</param>
        /// <param name="identifiersConverter">A converter used to convert identifiers.</param>
        /// <param name="modelConverter">A converter used to convert data contracts into objects.</param>
        /// <exception cref="ArgumentNullException">Thrown when either parameter is null.</exception>
        public CharacterRepository(HttpClient httpClient, IResponseConverter responseConverter, ICache<string, Character> cache, IConverter<string, string> identifiersConverter, IConverter<CharacterDataModel, Character> modelConverter)
            : base(httpClient, responseConverter, cache)
        {
            if (httpClient == null)
            {
                throw new ArgumentNullException(nameof(httpClient));
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
        public IConverter<CharacterDataModel, Character> ModelConverter { get; }

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