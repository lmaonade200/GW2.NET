// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArmorConverter.cs" company="GW2.NET Coding Team">
//   This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>
// <summary>
//   Converts objects of type <see cref="DetailsDTO" /> to objects of type <see cref="Armor" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace GW2NET.Items.Converter
{
    using System;
    using System.Collections.Generic;

    using GW2NET.Common;
    using GW2NET.Items.ApiModels;

    public partial class ArmorConverter
    {
        private readonly IConverter<InfixUpgradeDataModel, InfixUpgrade> infixUpgradeConverter;

        private readonly IConverter<ICollection<InfusionSlotDataModel>, ICollection<InfusionSlot>> infusionSlotCollectionConverter;

        private readonly IConverter<string, WeightClass> weightClassConverter;

        /// <summary>Initializes a new instance of the <see cref="ArmorConverter"/> class.</summary>
        /// <param name="converterFactory"></param>
        /// <param name="weightClassConverter">The converter for <see cref="WeightClass"/>.</param>
        /// <param name="infusionSlotCollectionConverter">The converter for <see cref="ICollection{InfusionSlot}"/>.</param>
        /// <param name="infixUpgradeConverter">The converter for <see cref="InfixUpgrade"/>.</param>
        public ArmorConverter(
            ITypeConverterFactory<ItemDataModel, Armor> converterFactory,
            IConverter<string, WeightClass> weightClassConverter,
            IConverter<ICollection<InfusionSlotDataModel>, ICollection<InfusionSlot>> infusionSlotCollectionConverter,
            IConverter<InfixUpgradeDataModel, InfixUpgrade> infixUpgradeConverter)
            : this(converterFactory)
        {
            if (weightClassConverter == null)
            {
                throw new ArgumentNullException("weightClassConverter");
            }

            if (infusionSlotCollectionConverter == null)
            {
                throw new ArgumentNullException("infusionSlotCollectionConverter");
            }

            if (infixUpgradeConverter == null)
            {
                throw new ArgumentNullException("infixUpgradeConverter");
            }

            this.weightClassConverter = weightClassConverter;
            this.infusionSlotCollectionConverter = infusionSlotCollectionConverter;
            this.infixUpgradeConverter = infixUpgradeConverter;
        }

        partial void Merge(Armor entity, ItemDataModel dataModel, object state)
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

            entity.WeightClass = this.weightClassConverter.Convert(details.WeightClass, details);
            if (details.Defense.HasValue)
            {
                entity.Defense = details.Defense.Value;
            }

            var infusionSlots = details.InfusionSlots;
            if (infusionSlots != null)
            {
                entity.InfusionSlots = this.infusionSlotCollectionConverter.Convert(infusionSlots, details);
            }

            var infixUpgrade = details.InfixUpgrade;
            if (infixUpgrade != null)
            {
                entity.InfixUpgrade = this.infixUpgradeConverter.Convert(infixUpgrade, details);
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