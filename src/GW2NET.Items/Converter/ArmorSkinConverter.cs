// <copyright file="ArmorSkinConverter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GW2NET.Items.Converter
{
    using System;

    using GW2NET.Common;
    using GW2NET.Items.ApiModels;
    using GW2NET.Items.Armors;
    using GW2NET.Skins;

    public partial class ArmorSkinConverter
    {
        private readonly IConverter<string, WeightClass> weightClassConverter;

        /// <summary>Initializes a new instance of the <see cref="ArmorSkinConverter" /> class.</summary>
        /// <param name="converterFactory"></param>
        /// <param name="weightClassConverter">The converter for <see cref="WeightClass" />.</param>
        /// <exception cref="ArgumentNullException">Thrown when the weight converter is null.</exception>
        public ArmorSkinConverter(ITypeConverterFactory<SkinDataModel, ArmorSkin> converterFactory, IConverter<string, WeightClass> weightClassConverter)
            : this(converterFactory)
        {
            if (weightClassConverter == null)
            {
                throw new ArgumentNullException(nameof(weightClassConverter));
            }

            this.weightClassConverter = weightClassConverter;
        }

        partial void Merge(ArmorSkin entity, SkinDataModel dataContract, object state)
        {
            var details = dataContract.Details;
            if (details == null)
            {
                return;
            }

            if (details.WeightClass != null)
            {
                entity.WeightClass = this.weightClassConverter.Convert(details.WeightClass, details);
            }
        }
    }
}