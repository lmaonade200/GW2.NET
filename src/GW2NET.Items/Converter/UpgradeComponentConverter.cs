// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UpgradeComponentConverter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary>
//   Converts objects of type <see cref="DetailsDTO" /> to objects of type <see cref="UpgradeComponent" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace GW2NET.Items.Converter
{
    using System;
    using System.Collections.Generic;

    using GW2NET.Common;
    using GW2NET.Items.ApiModels;
    using GW2NET.Items.Common;
    using GW2NET.Items.UpgradeComponents;

    public partial class UpgradeComponentConverter
    {
        private readonly IConverter<InfixUpgradeDataModel, InfixUpgrade> infixUpgradeConverter;

        private readonly IConverter<ICollection<string>, InfusionSlotFlags> infusionSlotFlagCollectionConverter;

        private readonly IConverter<ICollection<string>, UpgradeComponentFlags> upgradeComponentFlagCollectionConverter;

        /// <summary>Initializes a new instance of the <see cref="UpgradeComponentConverter"/> class.</summary>
        /// <param name="converterFactory"></param>
        /// <param name="upgradeComponentFlagCollectionConverter">The converter for <see cref="UpgradeComponentFlags"/>.</param>
        /// <param name="infusionSlotFlagCollectionConverter">The converter for <see cref="ICollection{InfusionSlotFlags}"/>.</param>
        /// <param name="infixUpgradeConverter">The converter for <see cref="InfixUpgrade"/>.</param>
        public UpgradeComponentConverter(
            ITypeConverterFactory<ItemDataModel, UpgradeComponent> converterFactory,
            IConverter<ICollection<string>, UpgradeComponentFlags> upgradeComponentFlagCollectionConverter,
            IConverter<ICollection<string>, InfusionSlotFlags> infusionSlotFlagCollectionConverter,
            IConverter<InfixUpgradeDataModel, InfixUpgrade> infixUpgradeConverter)
            : this(converterFactory)
        {
            if (upgradeComponentFlagCollectionConverter == null)
            {
                throw new ArgumentNullException(nameof(upgradeComponentFlagCollectionConverter));
            }

            if (infusionSlotFlagCollectionConverter == null)
            {
                throw new ArgumentNullException(nameof(infusionSlotFlagCollectionConverter));
            }

            if (infixUpgradeConverter == null)
            {
                throw new ArgumentNullException(nameof(infixUpgradeConverter));
            }

            this.upgradeComponentFlagCollectionConverter = upgradeComponentFlagCollectionConverter;
            this.infusionSlotFlagCollectionConverter = infusionSlotFlagCollectionConverter;
            this.infixUpgradeConverter = infixUpgradeConverter;
        }

        partial void Merge(UpgradeComponent entity, ItemDataModel dataModel, object state)
        {
            var details = dataModel.Details;
            if (details == null)
            {
                return;
            }

            var flags = details.Flags;
            if (flags != null)
            {
                entity.UpgradeComponentFlags = this.upgradeComponentFlagCollectionConverter.Convert(flags, details);
            }

            var infusionUpgradeFlags = details.InfusionUpgradeFlags;
            if (infusionUpgradeFlags != null)
            {
                entity.InfusionUpgradeFlags = this.infusionSlotFlagCollectionConverter.Convert(infusionUpgradeFlags, details);
            }

            entity.Suffix = details.Suffix;

            var infixUpgrade = details.InfixUpgrade;
            if (infixUpgrade != null)
            {
                entity.InfixUpgrade = this.infixUpgradeConverter.Convert(infixUpgrade, details);
            }

            entity.Bonuses = details.Bonuses;
        }
    }
}