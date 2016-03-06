// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WeaponConverter.cs" company="GW2.NET Coding Team">
//   This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>
// <summary>
//   Converts objects of type <see cref="DetailsDTO" /> to objects of type <see cref="Weapon" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GW2NET.Items.Converter
{
    using System;
    using System.Collections.Generic;

    using GW2NET.Common;
    using GW2NET.Items.ApiModels;

    public partial class WeaponConverter
    {
        private readonly IConverter<string, DamageType> damageTypeConverter;

        private readonly IConverter<InfixUpgradeDataModel, InfixUpgrade> infixUpgradeConverter;

        private readonly IConverter<ICollection<InfusionSlotDataModel>, ICollection<InfusionSlot>>
            infusionSlotCollectionConverter;

        /// <summary>Initializes a new instance of the <see cref="WeaponConverter" /> class.</summary>
        /// <param name="converterFactory"></param>
        /// <param name="damageTypeConverter">The converter for <see cref="DamageType" />.</param>
        /// <param name="infusionSlotCollectionConverter">The converter for <see cref="ICollection{InfusionSlot}" />.</param>
        /// <param name="infixUpgradeConverter">The converter for <see cref="InfixUpgrade" />.</param>
        public WeaponConverter(
            ITypeConverterFactory<ItemDataModel, Weapon> converterFactory,
            IConverter<string, DamageType> damageTypeConverter,
            IConverter<ICollection<InfusionSlotDataModel>, ICollection<InfusionSlot>> infusionSlotCollectionConverter,
            IConverter<InfixUpgradeDataModel, InfixUpgrade> infixUpgradeConverter)
            : this(converterFactory)
        {
            if (damageTypeConverter == null)
            {
                throw new ArgumentNullException("damageTypeConverter");
            }

            if (infixUpgradeConverter == null)
            {
                throw new ArgumentNullException("infixUpgradeConverter");
            }

            if (infusionSlotCollectionConverter == null)
            {
                throw new ArgumentNullException("infusionSlotCollectionConverter");
            }

            this.damageTypeConverter = damageTypeConverter;
            this.infixUpgradeConverter = infixUpgradeConverter;
            this.infusionSlotCollectionConverter = infusionSlotCollectionConverter;
        }

        partial void Merge(Weapon entity, ItemDataModel dataModel, object state)
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

            entity.DamageType = this.damageTypeConverter.Convert(details.DamageType, details);

            if (details.MinimumPower.HasValue)
            {
                entity.MinimumPower = details.MinimumPower.Value;
            }

            if (details.MaximumPower.HasValue)
            {
                entity.MaximumPower = details.MaximumPower.Value;
            }

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