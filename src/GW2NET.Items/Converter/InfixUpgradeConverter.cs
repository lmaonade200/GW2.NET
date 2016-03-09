// <copyright file="InfixUpgradeConverter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GW2NET.Items.Converter
{
    using System;
    using System.Collections.Generic;

    using GW2NET.Common;
    using GW2NET.Items.ApiModels;
    using GW2NET.Items.Common;

    /// <summary>Converts objects of type <see cref="InfixUpgradeDataModel" /> to objects of type <see cref="InfixUpgrade" />.</summary>
    public sealed class InfixUpgradeConverter : IConverter<InfixUpgradeDataModel, InfixUpgrade>
    {
        private readonly IConverter<IEnumerable<AttributeDataModel>, IEnumerable<CombatAttribute>>
            combatAttributeCollectionConverter;

        private readonly IConverter<BuffDataModel, CombatBuff> combatBuffConverter;

        /// <summary>Initializes a new instance of the <see cref="InfixUpgradeConverter" /> class.</summary>
        /// <param name="combatAttributeCollectionConverter">The converter for <see cref="ICollection{CombatAttribute}" />.</param>
        /// <param name="combatBuffConverter">The converter for <see cref="CombatBuff" />.</param>
        public InfixUpgradeConverter(
            IConverter<IEnumerable<AttributeDataModel>, IEnumerable<CombatAttribute>> combatAttributeCollectionConverter,
            IConverter<BuffDataModel, CombatBuff> combatBuffConverter)
        {
            if (combatAttributeCollectionConverter == null)
            {
                throw new ArgumentNullException(nameof(combatAttributeCollectionConverter));
            }

            if (combatBuffConverter == null)
            {
                throw new ArgumentNullException(nameof(combatBuffConverter));
            }

            this.combatAttributeCollectionConverter = combatAttributeCollectionConverter;
            this.combatBuffConverter = combatBuffConverter;
        }

        /// <summary>
        ///     Converts the given object of type <see cref="InfixUpgradeDataModel" /> to an object of type
        ///     <see cref="InfixUpgrade" />.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="state"></param>
        /// <returns>The converted value.</returns>
        public InfixUpgrade Convert(InfixUpgradeDataModel value, object state)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var infixUpgrade = new InfixUpgrade();
            var buff = value.Buff;
            if (buff != null)
            {
                infixUpgrade.Buff = this.combatBuffConverter.Convert(buff, value);
            }

            var attributes = value.Attributes;
            if (attributes != null)
            {
                infixUpgrade.Attributes = this.combatAttributeCollectionConverter.Convert(attributes, value);
            }

            return infixUpgrade;
        }
    }
}