// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemFlagConverter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary>
//   Converts objects of type <see cref="string" /> to objects of type <see cref="ItemFlags" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GW2NET.Items.Converter
{
    using System;
    using System.Diagnostics;

    using GW2NET.Common;

    /// <summary>Converts objects of type <see cref="string" /> to objects of type <see cref="ItemFlags" />.</summary>
    public sealed class ItemFlagConverter : IConverter<string, ItemFlags>
    {
        /// <summary>Converts the given object of type <see cref="string" /> to an object of type <see cref="ItemFlags" />.</summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="state"></param>
        /// <returns>The converted value.</returns>
        public ItemFlags Convert(string value, object state)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            ItemFlags result;
            if (Enum.TryParse(value, true, out result))
            {
                return result;
            }

            Debug.Assert(false, "Unknown ItemFlags: " + value);
            return default(ItemFlags);
        }
    }
}