// <copyright file="RecipeRepository.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.V2.Recipes
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    using GW2NET.Caching;
    using GW2NET.Common;
    using GW2NET.Common.Converters;
    using GW2NET.Common.Messages;
    using GW2NET.Recipes;
    using GW2NET.V2.Recipes.Json;

    /// <summary>Represents a repository that retrieves data from the /v2/recipes interface. See the remarks section for important limitations regarding this implementation.</summary>
    /// <remarks>
    /// This implementation does not retrieve associated entities.
    /// <list type="bullet">
    ///     <item>
    ///         <description><see cref="Recipe"/>: <see cref="Recipe.BuildId"/> is always <c>0</c>. Retrieve the build number from the build service.</description>
    ///     </item>
    ///     <item>
    ///         <description><see cref="Recipe"/>: <see cref="Recipe.OutputItem"/> is always <c>null</c>. Use the value of <see cref="Recipe.OutputItemId"/> to retrieve the output item.</description>
    ///     </item>
    ///     <item>
    ///         <description><see cref="ItemStack"/>: <see cref="ItemStack.Item"/> is always <c>null</c>. Use the value of <see cref="ItemStack.ItemId"/> to retrieve the ingredient item.</description>
    ///     </item>
    /// </list>
    /// </remarks>
    public class RecipeRepository : CachedRepository<int, Recipe>, IRecipeRepository, IDiscoverableNew<int>, ICachedRepository<int, RecipeDTO, Recipe>, ILocalizable
    {
        /// <summary>Initializes a new instance of the <see cref="RecipeRepository"/> class.</summary>
        /// <param name="httpClient">The <see cref="HttpClient"/> used to make connections with the ArenaNet servers.</param>
        /// <param name="responseConverter">The <see cref="IResponseConverter"/> used to convert <see cref="HttpResponseMessage"/> into objects.</param>
        /// <param name="cache">The <see cref="ICache{TKey, TValue}"/> used to cache api responses.</param>
        /// <param name="identifiersConverter">A converter used to convert identifiers.</param>
        /// <param name="modelConverter">A converter used to convert data contracts into objects.</param>
        /// <exception cref="ArgumentNullException">Thrown when either parameter is null.</exception>
        public RecipeRepository(HttpClient httpClient, IResponseConverter responseConverter, ICache<int, Recipe> cache, IConverter<int, int> identifiersConverter, IConverter<RecipeDTO, Recipe> modelConverter)
            : base(httpClient, responseConverter, cache)
        {
            if (identifiersConverter == null)
            {
                throw new ArgumentNullException(nameof(identifiersConverter));
            }

            if (modelConverter == null)
            {
                throw new ArgumentNullException(nameof(modelConverter));
            }

            this.IdentifiersConverter = identifiersConverter;
            this.ModelConverter = modelConverter;
        }

        /// <inheritdoc />
        public CultureInfo Culture { get; set; }

        /// <inheritdoc />
        public IConverter<int, int> IdentifiersConverter { get; }

        /// <inheritdoc />
        public IConverter<RecipeDTO, Recipe> ModelConverter { get; }

        /// <inheritdoc />
        public IParameterizedBuilder ServiceLocation
        {
            get
            {
                return ApiMessageBuilder.Init().Version(ApiVersion.V2).OnEndpoint("recipes");
            }
        }

        /// <inheritdoc />
        public Task<IEnumerable<int>> DiscoverByInputAsync(int identifier)
        {
            return this.DiscoverByInputAsync(identifier, CancellationToken.None);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<int>> DiscoverByInputAsync(int identifier, CancellationToken cancellationToken)
        {
            IParameterizedBuilder request = ApiMessageBuilder.Init().Version(ApiVersion.V2).OnEndpoint("recipes/search").WithParameter("input", identifier.ToString());
            return await this.ResponseConverter.ConvertSetAsync(await this.Client.SendAsync(request.Build(), cancellationToken), this.IdentifiersConverter);
        }

        /// <inheritdoc />
        public Task<IEnumerable<int>> DiscoverByOutputAsync(int identifier)
        {
            return this.DiscoverByOutputAsync(identifier, CancellationToken.None);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<int>> DiscoverByOutputAsync(int identifier, CancellationToken cancellationToken)
        {
            IParameterizedBuilder request = ApiMessageBuilder.Init().Version(ApiVersion.V2).OnEndpoint("recipes/search").WithParameter("output", identifier.ToString());
            return await this.ResponseConverter.ConvertSetAsync(await this.Client.SendAsync(request.Build(), cancellationToken), this.IdentifiersConverter);
        }
    }
}