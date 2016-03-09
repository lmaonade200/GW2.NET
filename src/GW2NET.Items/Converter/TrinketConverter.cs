// <copyright file="TrinketConverter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GW2NET.Items.Converter
{
    using System;
    using System.Collections.Generic;

    using GW2NET.Common;
    using GW2NET.Items.ApiModels;
    using GW2NET.Items.Common;
    using GW2NET.Items.Trinkets;

    public partial class TrinketConverter
    {
        private readonly IConverter<InfixUpgradeDataModel, InfixUpgrade> infixUpgradeConverter;

        private readonly IConverter<IEnumerable<InfusionSlotDataModel>, IEnumerable<InfusionSlot>> infusionSlotCollectionConverter;

        /// <summary>Initializes a new instance of the <see cref="TrinketConverter"/> class.</summary>
        /// <param name="converterFactory"></param>
        /// <param name="infusionSlotCollectionConverter">The converter for <see cref="ICollection{InfusionSlot}"/>.</param>
        /// <param name="infixUpgradeConverter">The converter for <see cref="InfixUpgrade"/>.</param>
        public TrinketConverter(
            ITypeConverterFactory<ItemDataModel, Trinket> converterFactory,
            IConverter<IEnumerable<InfusionSlotDataModel>, IEnumerable<InfusionSlot>> infusionSlotCollectionConverter,
            IConverter<InfixUpgradeDataModel, InfixUpgrade> infixUpgradeConverter)
            : this(converterFactory)
        {
            if (infusionSlotCollectionConverter == null)
            {
                throw new ArgumentNullException(nameof(infusionSlotCollectionConverter));
            }

            if (infixUpgradeConverter == null)
            {
                throw new ArgumentNullException(nameof(infixUpgradeConverter));
            }

            this.infusionSlotCollectionConverter = infusionSlotCollectionConverter;
            this.infixUpgradeConverter = infixUpgradeConverter;
        }

        partial void Merge(Trinket entity, ItemDataModel dataModel, object state)
        {
            var details = dataModel.Details;
            if (details == null)
            {
                return;
            }

            var infusionSlots = details.InfusionSlots;
            if (infusionSlots != null)
            {
                entity.InfusionSlots = this.infusionSlotCollectionConverter.Convert(infusionSlots, dataModel);
            }

            var infixUpgrade = details.InfixUpgrade;
            if (infixUpgrade != null)
            {
                entity.InfixUpgrade = this.infixUpgradeConverter.Convert(infixUpgrade, dataModel);
            }

            details.SuffixItemId = details.SuffixItemId;

            int secondarySuffixItemId;
            if (int.TryParse(details.SecondarySuffixItemId, out secondarySuffixItemId))
            {
                entity.SecondarySuffixItemId = secondarySuffixItemId;
            }
        }
    }
}