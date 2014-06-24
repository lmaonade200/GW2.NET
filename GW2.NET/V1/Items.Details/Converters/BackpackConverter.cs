﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BackpackConverter.cs" company="GW2.NET Coding Team">
//   This product is licensed under the GNU General Public License version 2 (GPLv2) as defined on the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>
// <summary>
//   Converts an instance of <see cref="Backpack" /> from its <see cref="System.String" /> representation.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace GW2DotNET.V1.Items.Details.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using GW2DotNET.Common;
    using GW2DotNET.V1.Items.Details.Contracts.Backpacks;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>Converts an object to and/or from JSON.</summary>
    public class BackpackConverter : JsonConverter
    {
        /// <summary>Backing field. Holds a dictionary of known JSON values and their corresponding type.</summary>
        private static readonly IDictionary<string, Type> KnownTypes = new Dictionary<string, Type>();

        /// <summary>Initializes static members of the <see cref="BackpackConverter" /> class.</summary>
        static BackpackConverter()
        {
            var baseType = typeof(Backpack);
            var itemTypes = baseType.Assembly.GetTypes().Where(type => type.IsSubclassOf(baseType)).AsEnumerable();
            foreach (var itemType in itemTypes)
            {
                var typeDiscriminator =
                    itemType.GetCustomAttributes(typeof(TypeDiscriminatorAttribute), false).Cast<TypeDiscriminatorAttribute>().SingleOrDefault();
                if (typeDiscriminator != null && typeDiscriminator.BaseType == baseType)
                {
                    KnownTypes.Add(typeDiscriminator.Value, itemType);
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:Newtonsoft.Json.JsonConverter"/> can write JSON.
        /// </summary>
        /// <value>
        /// <c>true</c> if this <see cref="T:Newtonsoft.Json.JsonConverter"/> can write JSON; otherwise, <c>false</c>.
        /// </value>
        public override bool CanWrite
        {
            get
            {
                return false;
            }
        }

        /// <summary>Determines whether this instance can convert the specified object type.</summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns><c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.</returns>
        public override bool CanConvert(Type objectType)
        {
            return typeof(Backpack) == objectType;
        }

        /// <summary>Reads the JSON representation of the object.</summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader"/> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var content = JObject.Load(reader);

            var detailsProperty = content.Property("back");

            var infixProperty = detailsProperty.Value.Value<JObject>().Property("infix_upgrade");

            var item = new Backpack();

            detailsProperty.Remove();

            serializer.Populate(content.CreateReader(), item);

            if (infixProperty != null)
            {
                infixProperty.Remove();
                serializer.Populate(infixProperty.Value.CreateReader(), item);
            }

            serializer.Populate(detailsProperty.Value.CreateReader(), item);

            return item;
        }

        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter"/> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new InvalidOperationException();
        }
    }
}