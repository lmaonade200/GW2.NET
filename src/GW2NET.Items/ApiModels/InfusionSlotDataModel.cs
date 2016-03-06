// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InfusionSlotDataModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary>
//   Defines the InfusionSlotDTO type.
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
    public sealed class InfusionSlotDataModel

    {
        [DataMember(Name = "flags", Order = 0)]
        public ICollection<string> Flags { get; set; }

        [DataMember(Name = "item_id", Order = 1)]
        public int? ItemId { get; set; }
    }
}