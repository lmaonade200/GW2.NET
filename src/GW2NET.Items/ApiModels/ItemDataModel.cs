﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemDataModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary>
//   Defines the ItemDTO type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#pragma warning disable 1591
namespace GW2NET.Items.ApiModels
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;

    [DataContract]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "http://wiki.guildwars2.com/wiki/API:2/items")]
    public sealed class ItemDataModel
    {
        [DataMember(Order = 5, Name = "default_skin")]
        public string DefaultSkin { get; set; }

        [DataMember(Order = 1, Name = "description")]
        public string Description { get; set; }

        [DataMember(Order = 13, Name = "details")]
        public DetailsDataModel Details { get; set; }

        [DataMember(Order = 8, Name = "flags")]
        public ICollection<string> Flags { get; set; }

        [DataMember(Order = 7, Name = "game_types")]
        public ICollection<string> GameTypes { get; set; }

        [DataMember(Order = 12, Name = "icon")]
        public string Icon { get; set; }

        [DataMember(Order = 10, Name = "id")]
        public int Id { get; set; }

        [DataMember(Order = 11, Name = "chat_link")]
        public string ChatLink { get; set; }

        [DataMember(Order = 3, Name = "level")]
        public int Level { get; set; }

        [DataMember(Order = 0, Name = "name")]
        public string Name { get; set; }

        [DataMember(Order = 4, Name = "rarity")]
        public string Rarity { get; set; }

        [DataMember(Order = 9, Name = "restrictions")]
        public ICollection<string> Restrictions { get; set; }

        [DataMember(Order = 2, Name = "type")]
        public string Type { get; set; }

        [DataMember(Order = 6, Name = "vendor_value")]
        public int VendorValue { get; set; }
    }
}