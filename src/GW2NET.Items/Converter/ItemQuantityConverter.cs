// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemQuantityConverter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary>
//   Converts objects of type <see cref="IngredientDTO" /> to objects of type <see cref="ItemQuantity" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace GW2NET.Items.Converter
{
    using System;

    using GW2NET.Common;
    using GW2NET.Items.ApiModels;

    /// <summary>Converts objects of type <see cref="IngredientDataModel"/> to objects of type <see cref="ItemQuantity"/>.</summary>
    public sealed class ItemQuantityConverter : IConverter<IngredientDataModel, ItemQuantity>
    {
        /// <inheritdoc />
        public ItemQuantity Convert(IngredientDataModel value, object state)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var itemQuantity = new ItemQuantity
            {
                ItemId = value.ItemId
            };

            if (value.Count > 0)
            {
                itemQuantity.Count = value.Count;
            }

            return itemQuantity;
        }
    }
}