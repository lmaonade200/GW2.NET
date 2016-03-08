// <copyright file="BitsConverter.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Achievements.Converter
{
    using System.Collections.Generic;

    using GW2NET.Common;

    /// <summary>Converts a <see cref="KeyValuePair{TKey,TValue}"/> into the corresponding <see cref="AchievementBit"/> object.</summary>
    public class BitsConverter : IConverter<KeyValuePair<string, object>, AchievementBit>
    {
        /// <inheritdoc />
        public AchievementBit Convert(KeyValuePair<string, object> value, object state = null)
        {
            switch (value.Key)
            {
                case "Text":
                    return new TextBit { Text = value.Value.ToString() };
                case "Item":
                    return new ItemBit { Id = System.Convert.ToInt32(value.Value) };
                case "Minipet":
                    return new MinipetBit { Id = System.Convert.ToInt32(value.Value) };
                case "Skin": 
                    return new SkinBit { Id = System.Convert.ToInt32(value.Value) };
                default:
                    throw new SerializationException($"The type '{value.Key}' could not be converted into a achivement-bit object.");
            }
        }
    }
}