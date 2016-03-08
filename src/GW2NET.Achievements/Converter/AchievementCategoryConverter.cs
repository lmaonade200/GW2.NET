// <copyright file="AchievementCategoryConverter.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Achievements.Converter
{
    using System;
    using System.Linq;

    using GW2NET.Achievements.ApiModels;
    using GW2NET.Common;

    public class AchievementCategoryConverter : IConverter<AchievementCategoryDataModel, Category>
    {
        /// <inheritdoc />
        public Category Convert(AchievementCategoryDataModel value, object state = null)
        {
            return new Category
            {
                Id = value.Id,
                Name = value.Name,
                Description = value.Description,
                IconUrl = new Uri(value.Icon, UriKind.Absolute),
                AchievementIds = value.Achievements.AsEnumerable(),
                Order = value.Order,
            };
        }
    }
}
