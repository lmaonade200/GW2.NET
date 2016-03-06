// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecipeFlagCollectionConverter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary>
//   Converts objects of type <see cref="T:ICollection{string}" /> to objects of type <see cref="RecipeFlags" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace GW2NET.Items.Converter
{
    using System;
    using System.Collections.Generic;

    using GW2NET.Common;
    using GW2NET.Recipes;

    /// <summary>Converts objects of type <see cref="T:ICollection{string}"/> to objects of type <see cref="RecipeFlags"/>.</summary>
    public sealed class RecipeFlagCollectionConverter : IConverter<ICollection<string>, RecipeFlags>
    {
        private readonly IConverter<string, RecipeFlags> recipeFlagConverter;

        /// <summary>Initializes a new instance of the <see cref="RecipeFlagCollectionConverter"/> class.</summary>
        /// <param name="recipeFlagConverter">The converter for <see cref="RecipeFlags"/>.</param>
        /// <exception cref="ArgumentNullException">The value of <paramref name="recipeFlagConverter"/> is a null reference.</exception>
        public RecipeFlagCollectionConverter(IConverter<string, RecipeFlags> recipeFlagConverter)
        {
            if (recipeFlagConverter == null)
            {
                throw new ArgumentNullException(nameof(recipeFlagConverter));
            }

            this.recipeFlagConverter = recipeFlagConverter;
        }

        /// <inheritdoc />
        public RecipeFlags Convert(ICollection<string> value, object state)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            RecipeFlags result = default(RecipeFlags);
            foreach (var s in value)
            {
                result |= this.recipeFlagConverter.Convert(s, state);
            }

            return result;
        }
    }
}