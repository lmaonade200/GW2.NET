// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InfusionSlotFlagConverter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary>
//   Converts objects of type <see cref="string" /> to objects of type <see cref="InfusionSlotFlags" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GW2NET.Items.Converter
{
    using System;
    using System.Diagnostics;

    using GW2NET.Common;
    using GW2NET.Items.Common;

    /// <summary>Converts objects of type <see cref="string" /> to objects of type <see cref="InfusionSlotFlags" />.</summary>
    public sealed class InfusionSlotFlagConverter : IConverter<string, InfusionSlotFlags>
    {
        /// <summary>Converts the given object of type <see cref="string" /> to an object of type <see cref="InfusionSlotFlags" />.</summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="state"></param>
        /// <returns>The converted value.</returns>
        public InfusionSlotFlags Convert(string value, object state)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            InfusionSlotFlags result;
            if (Enum.TryParse(value, true, out result))
            {
                return result;
            }

            Debug.Assert(false, "Unknown InfusionSlotFlags: " + value);
            return default(InfusionSlotFlags);
        }
    }
}