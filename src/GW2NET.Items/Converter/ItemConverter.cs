// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemConverter.cs" company="GW2.NET Coding Team">
//   This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>
// <summary>
//   Converts objects of type <see cref="ItemDTO" /> to objects of type <see cref="Item" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace GW2NET.Items.Converter
{
    using System;
    using System.Collections.Generic;

    using GW2NET.Common;
    using GW2NET.Common.Converters;
    using GW2NET.Items.ApiModels;

    /// <summary>Converts objects of type <see cref="ItemDataModel"/> to objects of type <see cref="Item"/>.</summary>
    public partial class ItemConverter
    {
        private readonly IConverter<ICollection<string>, GameTypes> gameTypesConverter;

        private readonly IConverter<ICollection<string>, ItemFlags> itemFlagsConverter;

        private readonly IConverter<string, ItemRarity> itemRarityConverter;

        private readonly IConverter<ICollection<string>, ItemRestrictions> itemRestrictionsConverter;

        /// <summary>Initializes a new instance of the <see cref="ItemConverter"/> class.</summary>
        /// <param name="converterFactory"></param>
        /// <param name="itemRarityConverter">The converter for <see cref="ItemRarity"/>.</param>
        /// <param name="gameTypesConverter">The converter for <see cref="GameTypes"/>.</param>
        /// <param name="itemFlagsConverter">The converter for <see cref="ItemFlags"/>.</param>
        /// <param name="itemRestrictionsConverter">The converter for <see cref="ItemRestrictions"/>.</param>
        public ItemConverter(
            ITypeConverterFactory<ItemDataModel, Item> converterFactory,
            IConverter<string, ItemRarity> itemRarityConverter,
            IConverter<ICollection<string>, GameTypes> gameTypesConverter,
            IConverter<ICollection<string>, ItemFlags> itemFlagsConverter,
            IConverter<ICollection<string>, ItemRestrictions> itemRestrictionsConverter)
            : this(converterFactory)
        {
            if (itemRarityConverter == null)
            {
                throw new ArgumentNullException(nameof(itemRarityConverter));
            }

            if (gameTypesConverter == null)
            {
                throw new ArgumentNullException(nameof(gameTypesConverter));
            }

            if (itemFlagsConverter == null)
            {
                throw new ArgumentNullException(nameof(itemFlagsConverter));
            }

            if (itemRestrictionsConverter == null)
            {
                throw new ArgumentNullException(nameof(itemRestrictionsConverter));
            }

            this.itemRarityConverter = itemRarityConverter;
            this.gameTypesConverter = gameTypesConverter;
            this.itemFlagsConverter = itemFlagsConverter;
            this.itemRestrictionsConverter = itemRestrictionsConverter;
        }

        partial void Merge(Item entity, ItemDataModel dataModel, object state)
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
            entity.ItemId = dataModel.Id;
            entity.ChatLink = dataModel.ChatLink;
            entity.Name = dataModel.Name;
            entity.Description = dataModel.Description;
            entity.Level = dataModel.Level;
            entity.Rarity = this.itemRarityConverter.Convert(dataModel.Rarity, dataModel);
            entity.VendorValue = dataModel.VendorValue;
            ICollection<string> gameTypes = dataModel.GameTypes;
            if (gameTypes != null)
            {
                entity.GameTypes = this.gameTypesConverter.Convert(gameTypes, dataModel);
            }

            ICollection<string> flags = dataModel.Flags;
            if (flags != null)
            {
                entity.Flags = this.itemFlagsConverter.Convert(flags, dataModel);
            }

            ICollection<string> restrictions = dataModel.Restrictions;
            if (restrictions != null)
            {
                entity.Restrictions = this.itemRestrictionsConverter.Convert(restrictions, dataModel);
            }

            // Set the icon file identifier and signature
            Uri icon;
            if (Uri.TryCreate(dataModel.Icon, UriKind.Absolute, out icon))
            {
                // Set the icon file URL
                entity.IconFileUrl = icon;

                // Split the path into segments
                // Format: /file/{signature}/{identifier}.{extension}
                string[] segments = icon.LocalPath.Split('.')[0].Split('/');

                // Set the icon file signature
                if (segments.Length >= 3 && segments[2] != null)
                {
                    entity.IconFileSignature = segments[2];
                }

                // Set the icon file identifier
                int iconFileId;
                if (segments.Length >= 4 && int.TryParse(segments[3], out iconFileId))
                {
                    entity.IconFileId = iconFileId;
                }
            }
        }
    }
}