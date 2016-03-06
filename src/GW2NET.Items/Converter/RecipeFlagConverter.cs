// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecipeFlagConverter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary>
//   Converts objects of type <see cref="string" /> to objects of type <see cref="RecipeFlags" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace GW2NET.Items.Converter
{
    using System;
    using System.Diagnostics;

    using GW2NET.Common;
    using GW2NET.Recipes;

    /// <summary>Converts objects of type <see cref="string"/> to objects of type <see cref="RecipeFlags"/>.</summary>
    public sealed class RecipeFlagConverter : IConverter<string, RecipeFlags>
    {
        /// <inheritdoc />
        public RecipeFlags Convert(string value, object state)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            RecipeFlags result;
            if (Enum.TryParse(value, true, out result))
            {
                return result;
            }

            Debug.Assert(false, "Unknown RecipeFlags: " + value);
            return default(RecipeFlags);
        }
    }
}