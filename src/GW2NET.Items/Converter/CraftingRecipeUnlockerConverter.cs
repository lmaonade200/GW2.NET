// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CraftingRecipeUnlockerConverter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary>
//   Converts objects of type <see cref="DetailsDTO" /> to objects of type <see cref="CraftingRecipeUnlocker" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GW2NET.Items.Converter
{
    using GW2NET.Items.ApiModels;
    using GW2NET.Items.Consumables;

    public partial class CraftingRecipeUnlockerConverter
    {
        partial void Merge(CraftingRecipeUnlocker entity, ItemDataModel dataModel, object state)
        {
            var details = dataModel.Details;
            if (details == null)
            {
                return;
            }

            if (details.RecipeId.HasValue)
            {
                entity.RecipeId = details.RecipeId.Value;
            }
        }
    }
}