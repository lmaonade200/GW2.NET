// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UpgradeComponentFlagConverter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary>
//   Converts objects of type <see cref="string" /> to objects of type <see cref="UpgradeComponentFlags" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GW2NET.Items.Converter
{
    using System;
    using System.Diagnostics;

    using GW2NET.Common;
    using GW2NET.Items.UpgradeComponents;

    /// <summary>Converts objects of type <see cref="string" /> to objects of type <see cref="UpgradeComponentFlags" />.</summary>
    public sealed class UpgradeComponentFlagConverter : IConverter<string, UpgradeComponentFlags>
    {
        /// <summary>
        ///     Converts the given object of type <see cref="string" /> to an object of type
        ///     <see cref="UpgradeComponentFlags" />.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="state"></param>
        /// <returns>The converted value.</returns>
        public UpgradeComponentFlags Convert(string value, object state)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            UpgradeComponentFlags result;
            if (Enum.TryParse(value, out result))
            {
                return result;
            }

            Debug.Assert(false, "Unknown UpgradeComponentFlags: " + value);
            return default(UpgradeComponentFlags);
        }
    }
}