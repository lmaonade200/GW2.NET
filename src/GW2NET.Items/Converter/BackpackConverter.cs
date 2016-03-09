// <copyright file="BackpackConverter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GW2NET.Items.Converter
{
    using System;
    using System.Collections.Generic;

    using GW2NET.Common;
    using GW2NET.Items.ApiModels;
    using GW2NET.Items.Backpacks;
    using GW2NET.Items.Common;

    public partial class BackpackConverter
    {
        private readonly IConverter<InfixUpgradeDataModel, InfixUpgrade> infixUpgradeConverter;

        private readonly IConverter<IEnumerable<InfusionSlotDataModel>, IEnumerable<InfusionSlot>> infusionSlotCollectionConverter;

        /// <summary>Initializes a new instance of the <see cref="BackpackConverter"/> class.</summary>
        /// <param name="infusionSlotCollectionConverter">The converter for <see cref="ICollection{InfusionSlot}"/>.</param>
        /// <param name="infixUpgradeConverter">The converter for <see cref="InfixUpgrade"/>.</param>
        public BackpackConverter(IConverter<IEnumerable<InfusionSlotDataModel>, IEnumerable<InfusionSlot>> infusionSlotCollectionConverter, IConverter<InfixUpgradeDataModel, InfixUpgrade> infixUpgradeConverter)
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

        partial void Merge(Backpack entity, ItemDataModel dataModel, object state)
        {
            int defaultSkinId;
            if (int.TryParse(dataModel.DefaultSkin, out defaultSkinId))
            {
                entity.DefaultSkinId = defaultSkinId;
            }

            var details = dataModel.Details;
            if (details == null)
            {
                return;
            }

            var infusionSlots = details.InfusionSlots;
            if (infusionSlots != null)
            {
                entity.InfusionSlots = this.infusionSlotCollectionConverter.Convert(infusionSlots, state);
            }

            var infixUpgrade = details.InfixUpgrade;
            if (infixUpgrade != null)
            {
                entity.InfixUpgrade = this.infixUpgradeConverter.Convert(infixUpgrade, state);
            }

            entity.SuffixItemId = details.SuffixItemId;

            int secondarySuffixItemId;
            if (int.TryParse(details.SecondarySuffixItemId, out secondarySuffixItemId))
            {
                entity.SecondarySuffixItemId = secondarySuffixItemId;
            }
        }
    }
}