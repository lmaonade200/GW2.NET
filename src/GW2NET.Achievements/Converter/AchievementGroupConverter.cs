// <copyright file="AchievementGroupConverter.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Achievements.Converter
{
    using System;
    using System.Linq;

    using GW2NET.Achievements.ApiModels;
    using GW2NET.Common;

    /// <summary>Converts a <see cref="AchievementGroupDataModel"/> into the corresponding <see cref="Group"/>.</summary>
    public class AchievementGroupConverter : IConverter<AchievementGroupDataModel, Group>
    {
        /// <inheritdoc />
        public Group Convert(AchievementGroupDataModel value, object state = null)
        {
            return new Group
            {
                Id = new Guid(value.Id),
                Name = value.Id,
                Description = value.Description,
                CategorieIds = value.Categories.AsEnumerable(),
                Order = value.Order
            };
        }
    }
}
