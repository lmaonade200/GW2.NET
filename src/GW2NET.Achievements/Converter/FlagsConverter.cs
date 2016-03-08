// <copyright file="FlagsConverter.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Achievements.Converter
{
    using System;
    using System.Linq;

    using GW2NET.Common;

    /// <summary>Converts an array of achievement flags into the corresponding <see cref="AchievementFlags"/> enumeration.</summary>
    public class FlagsConverter : IConverter<string[], AchievementFlags>
    {
        /// <inheritdoc />
        public AchievementFlags Convert(string[] value, object state = null)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return value.Aggregate<string, AchievementFlags>(0, (current, flag) => current | flag.TryParse<AchievementFlags>());
        }
    }
}