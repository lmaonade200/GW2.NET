// <copyright file="DailyAchivementConverter.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Achievements.Converter
{
    using GW2NET.Achievements.ApiModels;
    using GW2NET.Common;

    /// <summary>Converts a <see cref="DailyAchievementDataModel"/> into the corresponding <see cref="DailyAchievement"/>.</summary>
    public class DailyAchivementConverter : IConverter<DailyAchievementDataModel, DailyAchievement>
    {
        /// <inheritdoc />
        public DailyAchievement Convert(DailyAchievementDataModel value, object state = null)
        {
            return new DailyAchievement
            {
                Id = value.Id,
                MaximumLevel = value.Level.Max,
                MinimumLevel = value.Level.Min
            };
        }
    }
}