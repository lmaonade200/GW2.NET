// <copyright file="WeaponSkinConverter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GW2NET.Items.Converter
{
    using System;

    using GW2NET.Common;
    using GW2NET.Items.ApiModels;
    using GW2NET.Items.Weapons;
    using GW2NET.Skins;

    /// <summary>Converts objects of type <see cref="SkinDataModel" /> to objects of type <see cref="WeaponSkin" />.</summary>
    public partial class WeaponSkinConverter
    {
        private readonly IConverter<string, DamageType> damageClassConverter;

        /// <summary>Initializes a new instance of the <see cref="WeaponSkinConverter" /> class.</summary>
        /// <param name="converterFactory"></param>
        /// <param name="damageTypeConverter">The converter for <see cref="DamageType" />.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public WeaponSkinConverter(
            ITypeConverterFactory<SkinDataModel, WeaponSkin> converterFactory,
            IConverter<string, DamageType> damageTypeConverter)
            : this(converterFactory)
        {
            if (damageTypeConverter == null)
            {
                throw new ArgumentNullException(nameof(damageTypeConverter));
            }

            this.damageClassConverter = damageTypeConverter;
        }

        partial void Merge(WeaponSkin entity, SkinDataModel dataContract, object state)
        {
            var details = dataContract.Details;
            if (details == null)
            {
                return;
            }

            if (details.DamageClass != null)
            {
                entity.DamageType = this.damageClassConverter.Convert(details.DamageClass, details);
            }
        }
    }
}