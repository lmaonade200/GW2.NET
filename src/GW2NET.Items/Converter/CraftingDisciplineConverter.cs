// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CraftingDisciplineConverter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary>
//   Converts objects of type <see cref="string" /> to objects of type <see cref="CraftingDisciplines" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace GW2NET.Items.Converter
{
    using System;
    using System.Diagnostics;

    using GW2NET.Common;
    using GW2NET.Recipes;

    /// <summary>Converts objects of type <see cref="string"/> to objects of type <see cref="CraftingDisciplines"/>.</summary>
    public sealed class CraftingDisciplineConverter : IConverter<string, CraftingDisciplines>
    {
        /// <inheritdoc />
        public CraftingDisciplines Convert(string value, object state)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            CraftingDisciplines result;
            if (Enum.TryParse(value, true, out result))
            {
                return result;
            }

            Debug.Assert(false, "Unknown CraftingDisciplines: " + value);
            return default(CraftingDisciplines);
        }
    }
}