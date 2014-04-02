﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecipeDetailsService.cs" company="GW2.Net Coding Team">
//   This product is licensed under the GNU General Public License version 2 (GPLv2) as defined on the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>
// <summary>
//   Provides the default implementation of the recipe details service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace GW2DotNET.V1.RecipesDetails
{
    using System;
    using System.Globalization;
    using System.Threading;
    using System.Threading.Tasks;

    using GW2DotNET.V1.Common;
    using GW2DotNET.V1.RecipesDetails.Types;

    /// <summary>Provides the default implementation of the recipe details service.</summary>
    public class RecipeDetailsService : ServiceBase, IRecipeDetailsService
    {
        /// <summary>Initializes a new instance of the <see cref="RecipeDetailsService"/> class.</summary>
        public RecipeDetailsService()
            : this(new ServiceClient(new Uri(Services.DataServiceUrl)))
        {
        }

        /// <summary>Initializes a new instance of the <see cref="RecipeDetailsService"/> class.</summary>
        /// <param name="serviceClient">The service client.</param>
        public RecipeDetailsService(IServiceClient serviceClient)
            : base(serviceClient)
        {
        }

        /// <summary>Gets a recipe and its localized details.</summary>
        /// <param name="recipeId">The recipe.</param>
        /// <returns>A recipe and its localized details.</returns>
        /// <remarks>See <a href="http://wiki.guildwars2.com/wiki/API:1/recipe_details">wiki</a> for more information.</remarks>
        public Recipe GetRecipeDetails(int recipeId)
        {
            return this.GetRecipeDetails(recipeId, ServiceBase.DefaultLanguage);
        }

        /// <summary>Gets a recipe and its localized details.</summary>
        /// <param name="recipeId">The recipe.</param>
        /// <param name="language">The language.</param>
        /// <returns>A recipe and its localized details.</returns>
        /// <remarks>See <a href="http://wiki.guildwars2.com/wiki/API:1/recipe_details">wiki</a> for more information.</remarks>
        public Recipe GetRecipeDetails(int recipeId, CultureInfo language)
        {
            var serviceRequest = new RecipeDetailsRequest { RecipeId = recipeId, Language = language };
            var result = this.Request<Recipe>(serviceRequest);

            // patch missing language information
            result.Language = language;

            return result;
        }

        /// <summary>Gets a recipe and its localized details.</summary>
        /// <param name="recipeId">The recipe.</param>
        /// <returns>A recipe and its localized details.</returns>
        /// <remarks>See <a href="http://wiki.guildwars2.com/wiki/API:1/recipe_details">wiki</a> for more information.</remarks>
        public Task<Recipe> GetRecipeDetailsAsync(int recipeId)
        {
            return this.GetRecipeDetailsAsync(recipeId, CancellationToken.None);
        }

        /// <summary>Gets a recipe and its localized details.</summary>
        /// <param name="recipeId">The recipe.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that provides cancellation support.</param>
        /// <returns>A recipe and its localized details.</returns>
        /// <remarks>See <a href="http://wiki.guildwars2.com/wiki/API:1/recipe_details">wiki</a> for more information.</remarks>
        public Task<Recipe> GetRecipeDetailsAsync(int recipeId, CancellationToken cancellationToken)
        {
            return this.GetRecipeDetailsAsync(recipeId, ServiceBase.DefaultLanguage, cancellationToken);
        }

        /// <summary>Gets a recipe and its localized details.</summary>
        /// <param name="recipeId">The recipe.</param>
        /// <param name="language">The language.</param>
        /// <returns>A recipe and its localized details.</returns>
        /// <remarks>See <a href="http://wiki.guildwars2.com/wiki/API:1/recipe_details">wiki</a> for more information.</remarks>
        public Task<Recipe> GetRecipeDetailsAsync(int recipeId, CultureInfo language)
        {
            return this.GetRecipeDetailsAsync(recipeId, language, CancellationToken.None);
        }

        /// <summary>Gets a recipe and its localized details.</summary>
        /// <param name="recipeId">The recipe.</param>
        /// <param name="language">The language.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that provides cancellation support.</param>
        /// <returns>A recipe and its localized details.</returns>
        /// <remarks>See <a href="http://wiki.guildwars2.com/wiki/API:1/recipe_details">wiki</a> for more information.</remarks>
        public Task<Recipe> GetRecipeDetailsAsync(int recipeId, CultureInfo language, CancellationToken cancellationToken)
        {
            var serviceRequest = new RecipeDetailsRequest { RecipeId = recipeId, Language = language };
            var t1 = this.RequestAsync<Recipe>(serviceRequest, cancellationToken).ContinueWith(
                task =>
                    {
                        // patch missing language information
                        task.Result.Language = language;

                        return task.Result;
                    }, 
                cancellationToken);

            return t1;
        }
    }
}