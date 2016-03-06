// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UpgradeComponentFlagCollectionConverter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary>
//   Converts objects of type <see cref="T:ICollection{string}" /> to objects of type <see cref="UpgradeComponentFlags" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GW2NET.Items.Converter
{
    using System;
    using System.Collections.Generic;

    using GW2NET.Common;
    using GW2NET.Items.UpgradeComponents;

    /// <summary>
    ///     Converts objects of type <see cref="T:ICollection{string}" /> to objects of type
    ///     <see cref="UpgradeComponentFlags" />.
    /// </summary>
    public sealed class UpgradeComponentFlagCollectionConverter : IConverter<ICollection<string>, UpgradeComponentFlags>
    {
        private readonly IConverter<string, UpgradeComponentFlags> upgradeComponentFlagConverter;

        /// <summary>Initializes a new instance of the <see cref="UpgradeComponentFlagCollectionConverter" /> class.</summary>
        /// <param name="upgradeComponentFlagConverter">The converter for <see cref="UpgradeComponentFlags" />.</param>
        public UpgradeComponentFlagCollectionConverter(
            IConverter<string, UpgradeComponentFlags> upgradeComponentFlagConverter)
        {
            if (upgradeComponentFlagConverter == null)
            {
                throw new ArgumentNullException(nameof(upgradeComponentFlagConverter));
            }

            this.upgradeComponentFlagConverter = upgradeComponentFlagConverter;
        }

        /// <summary>
        ///     Converts the given object of type <see cref="T:ICollection{string}" /> to an object of type
        ///     <see cref="UpgradeComponentFlags" />.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="state"></param>
        /// <returns>The converted value.</returns>
        public UpgradeComponentFlags Convert(ICollection<string> value, object state)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var result = default(UpgradeComponentFlags);
            foreach (var s in value)
            {
                result |= this.upgradeComponentFlagConverter.Convert(s, state);
            }

            return result;
        }
    }
}