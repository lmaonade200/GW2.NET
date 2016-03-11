// <copyright file="SkinDataModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

#pragma warning disable 1591
namespace GW2NET.Items.ApiModels
{
    using System.Runtime.Serialization;

    [DataContract]
    public sealed class SkinDataModel
    {
        [DataMember(Name = "name", Order = 0)]
        public string Name { get; set; }

        [DataMember(Name = "type", Order = 1)]
        public string Type { get; set; }

        [DataMember(Name = "flags", Order = 2)]
        public string[] Flags { get; set; }

        [DataMember(Name = "restrictions", Order = 3)]
        public string[] Restrictions { get; set; }

        [DataMember(Name = "id", Order = 4)]
        public int Id { get; set; }

        [DataMember(Name = "icon", Order = 5)]
        public string IconUrl { get; set; }

        [DataMember(Name = "description", Order = 6)]
        public string Description { get; set; }

        [DataMember(Name = "details", Order = 7)]
        public DetailsDataModel Details { get; set; }
    }
}