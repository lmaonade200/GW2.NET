// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SkinConverter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary>
//   Defines the SkinConverter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GW2NET.Items.Converter
{
    using System;
    using System.Collections.Generic;

    using GW2NET.Common;
    using GW2NET.Common.Converters;
    using GW2NET.Items;
    using GW2NET.Items.ApiModels;
    using GW2NET.Skins;

    public partial class SkinConverter
    {
        private readonly IConverter<ICollection<string>, ItemRestrictions> itemRestrictionsConverter;

        private readonly IConverter<ICollection<string>, SkinFlags> skinFlagsConverter;

        /// <summary>Initializes a new instance of the <see cref="SkinConverter"/> class.</summary>
        /// <param name="converterFactory"></param>
        /// <param name="itemRestrictionsConverter">The item restrictions converter.</param>
        /// <param name="skinFlagsConverter">The skin flags converter.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public SkinConverter(
            ITypeConverterFactory<SkinDataModel, Skin> converterFactory,
            IConverter<ICollection<string>, ItemRestrictions> itemRestrictionsConverter,
            IConverter<ICollection<string>, SkinFlags> skinFlagsConverter)
            : this(converterFactory)
        {
            if (itemRestrictionsConverter == null)
            {
                throw new ArgumentNullException(nameof(itemRestrictionsConverter));
            }

            if (skinFlagsConverter == null)
            {
                throw new ArgumentNullException(nameof(skinFlagsConverter));
            }

            this.itemRestrictionsConverter = itemRestrictionsConverter;
            this.skinFlagsConverter = skinFlagsConverter;
        }

        partial void Merge(Skin entity, SkinDataModel dataContract, object state)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state), "Precondition: state is IResponse");
            }

            ApiMetadata response = state as ApiMetadata;
            if (response == null)
            {
                throw new ArgumentException("Could not cast to ApiMetadata", nameof(state));
            }

            entity.Culture = response.ContentLanguage;
            entity.SkinId = dataContract.Id;
            entity.Name = dataContract.Name;

            if (dataContract.Flags != null)
            {
                entity.Flags = this.skinFlagsConverter.Convert(dataContract.Flags, state);
            }

            if (dataContract.Restrictions != null)
            {
                entity.Restrictions = this.itemRestrictionsConverter.Convert(dataContract.Restrictions, state);
            }

            // Process the URI. Note since the V2 api the URI doesn't have to be built by hand anymore.
            // It is stored as a a string in the response.
            // Question: Shouled we split the URI for user convenience or not??
            // TODO: yes we should split the URI. Not for convencience, but because 'Skin' implements 'IRenderable'
            entity.IconFileUrl = new Uri(dataContract.IconUrl, UriKind.Absolute);
        }
    }
}