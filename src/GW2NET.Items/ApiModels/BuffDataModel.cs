// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BuffDataModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary>
//   Defines the BuffDTO type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#pragma warning disable 1591
namespace GW2NET.Items.ApiModels
{
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;

    [DataContract]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "http://wiki.guildwars2.com/wiki/API:2/items")]
    public sealed class BuffDataModel
    {
        [DataMember(Name = "description", Order = 1)]
        public string Description { get; set; }

        [DataMember(Name = "skill_id", Order = 0)]
        public int? SkillId { get; set; }
    }
}