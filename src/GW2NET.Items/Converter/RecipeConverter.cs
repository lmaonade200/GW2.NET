// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecipeConverter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary>
//   Converts objects of type <see cref="RecipeDTO" /> to objects of type <see cref="Recipe" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace GW2NET.Items.Converter
{
    using System;
    using System.Collections.Generic;

    using GW2NET.Common;
    using GW2NET.Common.Converters;
    using GW2NET.Items.ApiModels;
    using GW2NET.Recipes;

    public partial class RecipeConverter
    {
        private readonly IConverter<ICollection<string>, CraftingDisciplines> craftingDisciplineCollectionConverter;

        private readonly IConverter<ICollection<IngredientDataModel>, ICollection<ItemQuantity>> ingredientsCollectionConverter;

        private readonly IConverter<ICollection<string>, RecipeFlags> recipeFlagCollectionConverter;

        /// <summary>Initializes a new instance of the <see cref="RecipeConverter"/> class.</summary>
        /// <param name="converterFactory"></param>
        /// <param name="craftingDisciplineCollectionConverter">The converter for <see cref="CraftingDisciplines"/>.</param>
        /// <param name="recipeFlagCollectionConverter">The converter for <see cref="RecipeFlags"/>.</param>
        /// <param name="ingredientsCollectionConverter">The converter for <see cref="T:ICollection{ItemQuantity}"/>.</param>
        public RecipeConverter(
            ITypeConverterFactory<RecipeDataModel, Recipe> converterFactory,
            IConverter<ICollection<string>, CraftingDisciplines> craftingDisciplineCollectionConverter,
            IConverter<ICollection<string>, RecipeFlags> recipeFlagCollectionConverter,
            IConverter<ICollection<IngredientDataModel>, ICollection<ItemQuantity>> ingredientsCollectionConverter)
            : this(converterFactory)
        {
            if (craftingDisciplineCollectionConverter == null)
            {
                throw new ArgumentNullException(nameof(craftingDisciplineCollectionConverter));
            }

            if (recipeFlagCollectionConverter == null)
            {
                throw new ArgumentNullException(nameof(recipeFlagCollectionConverter));
            }

            if (ingredientsCollectionConverter == null)
            {
                throw new ArgumentNullException(nameof(ingredientsCollectionConverter));
            }

            this.craftingDisciplineCollectionConverter = craftingDisciplineCollectionConverter;
            this.recipeFlagCollectionConverter = recipeFlagCollectionConverter;
            this.ingredientsCollectionConverter = ingredientsCollectionConverter;
        }

        partial void Merge(Recipe entity, RecipeDataModel dataModel, object state)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state), "Precondition: state is IResponse");
            }

            var response = state as ApiMetadata;
            if (response == null)
            {
                throw new ArgumentException("Could not cast to ApiMetadata", nameof(state));
            }

            entity.Culture = response.ContentLanguage;
            entity.RecipeId = dataModel.Id;
            entity.ChatLink = dataModel.ChatLink;
            entity.OutputItemId = dataModel.OutputItemId;
            entity.OutputItemCount = dataModel.OutputItemCount;
            entity.MinimumRating = dataModel.MinRating;
            entity.TimeToCraft = TimeSpan.FromMilliseconds(dataModel.TimeToCraftMs);

            if (dataModel.Disciplines != null)
            {
                entity.CraftingDisciplines = this.craftingDisciplineCollectionConverter.Convert(dataModel.Disciplines, dataModel);
            }

            if (dataModel.Flags != null)
            {
                entity.Flags = this.recipeFlagCollectionConverter.Convert(dataModel.Flags, dataModel);
            }

            if (dataModel.Ingredients != null)
            {
                entity.Ingredients = this.ingredientsCollectionConverter.Convert(dataModel.Ingredients, dataModel);
            }
        }
    }
}