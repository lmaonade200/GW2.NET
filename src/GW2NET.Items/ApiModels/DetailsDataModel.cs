// <copyright file="DetailsDataModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

#pragma warning disable 1591
namespace GW2NET.Items.ApiModels
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;

    [DataContract]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "http://wiki.guildwars2.com/wiki/API:2/items")]
    public sealed partial class DetailsDataModel
    {
        /// <summary>Gets or sets the type.</summary>
        [DataMember(Name = "type", Order = 0)]
        public string Type { get; set; }
    }

   [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "http://wiki.guildwars2.com/wiki/API:2/items")]
    public sealed partial class DetailsDataModel
    {
        [DataMember(Name = "defense", Order = 2)]
        public int? Defense { get; set; }

        [DataMember(Name = "infusion_slots", Order = 3)]
        public ICollection<InfusionSlotDataModel> InfusionSlots { get; set; }

        [DataMember(Name = "secondary_suffix_item_id", Order = 6)]
        public string SecondarySuffixItemId { get; set; }

        [DataMember(Name = "suffix_item_id", Order = 5)]
        public int? SuffixItemId { get; set; }
    }

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "http://wiki.guildwars2.com/wiki/API:2/items")]
    public sealed partial class DetailsDataModel
    {
        [DataMember(Name = "infix_upgrade", Order = 4)]
        public InfixUpgradeDataModel InfixUpgrade { get; set; }
    }

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "http://wiki.guildwars2.com/wiki/API:2/items")]
    public sealed partial class DetailsDataModel
    {
        [DataMember(Name = "weight_class", Order = 1)]
        public string WeightClass { get; set; }
    }

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "http://wiki.guildwars2.com/wiki/API:2/items")]
    public sealed partial class DetailsDataModel
    {
        [DataMember(Name = "no_sell_or_sort", Order = 0)]
        public bool? NoSellOrSort { get; set; }

        [DataMember(Name = "size", Order = 1)]
        public int? Size { get; set; }
    }

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "http://wiki.guildwars2.com/wiki/API:2/items")]
    public sealed partial class DetailsDataModel
    {
        [DataMember(Name = "color_id", Order = 2)]
        public int? ColorId { get; set; }

        [DataMember(Name = "description", Order = 3)]
        public string Description { get; set; }

        [DataMember(Name = "duration_ms", Order = 2)]
        public double? Duration { get; set; }

        [DataMember(Name = "recipe_id", Order = 2)]
        public int? RecipeId { get; set; }

        [DataMember(Name = "unlock_type", Order = 1)]
        public string UnlockType { get; set; }
    }

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "http://wiki.guildwars2.com/wiki/API:2/items")]
    public sealed partial class DetailsDataModel
    {
        [DataMember(Name = "charges", Order = 1)]
        public int? Charges { get; set; }
    }

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "http://wiki.guildwars2.com/wiki/API:2/items")]
    public sealed partial class DetailsDataModel
    {
        [DataMember(Name = "bonuses", Order = 3)]
        public ICollection<string> Bonuses { get; set; }

        [DataMember(Name = "flags", Order = 1)]
        public ICollection<string> Flags { get; set; }

        [DataMember(Name = "infusion_upgrade_flags", Order = 2)]
        public ICollection<string> InfusionUpgradeFlags { get; set; }

        [DataMember(Name = "suffix", Order = 5)]
        public string Suffix { get; set; }
    }

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "http://wiki.guildwars2.com/wiki/API:2/items")]
    public sealed partial class DetailsDataModel
    {
        [DataMember(Name = "damage_type", Order = 1)]
        public string DamageType { get; set; }

        [DataMember(Name = "max_power", Order = 3)]
        public int? MaximumPower { get; set; }

        [DataMember(Name = "min_power", Order = 2)]
        public int? MinimumPower { get; set; }
    }

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "http://wiki.guildwars2.com/wiki/API:2/items")]
    public sealed partial class DetailsDataModel
    {
        [DataMember(Name = "skins", Order = 1)]
        public int[] Skins { get; set; }
    }

   [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "http://wiki.guildwars2.com/wiki/API:2/items")]
    public sealed partial class DetailsDataModel
    {
        [DataMember(Name = "minipet_id", Order = 1)]
        public int MiniPetId { get; set; }
    }

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "http://wiki.guildwars2.com/wiki/API:2/items")]
    public sealed partial class DetailsDataModel
    {
        /// <summary>Gets or sets the damage class.</summary>
        [DataMember(Name = "damage_type", Order = 1)]
        public string DamageClass { get; set; }
    }
}