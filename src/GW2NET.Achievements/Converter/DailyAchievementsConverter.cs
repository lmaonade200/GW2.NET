// <copyright file="DailyAchievementConverter.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Achievements.Converter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using GW2NET.Achievements.ApiModels;
    using GW2NET.Common;

    /// <summary>Converts a <see cref="DailyAchievementsDataModel"/> into a set of <see cref="DailyAchievement">DailyAchievements</see>.</summary>
    public class DailyAchievementsConverter : IConverter<DailyAchievementsDataModel, IEnumerable<DailyAchievement>>
    {
        private readonly IConverter<DailyAchievementDataModel, DailyAchievement> dailiesConverter;

        /// <summary>Initializes a new instance of the <see cref="DailyAchievementsConverter"/> class.</summary>
        /// <param name="dailiesConverter">The converter used to convert a single daily achievement.</param>
        public DailyAchievementsConverter(IConverter<DailyAchievementDataModel, DailyAchievement> dailiesConverter)
        {
            if (dailiesConverter == null)
            {
                throw new ArgumentNullException(nameof(dailiesConverter));
            }

            this.dailiesConverter = dailiesConverter;
        }

        /// <inheritdoc />
        public IEnumerable<DailyAchievement> Convert(DailyAchievementsDataModel value, object state = null)
        {
            List<DailyAchievement> dailies = new List<DailyAchievement>();

            dailies.AddRange(value.Pve.Select(daily => this.dailiesConverter.Convert(daily)));
            dailies.AddRange(value.Pvp.Select(daily => this.dailiesConverter.Convert(daily)));
            dailies.AddRange(value.Wvw.Select(daily => this.dailiesConverter.Convert(daily)));
            dailies.AddRange(value.Special.Select(daily => this.dailiesConverter.Convert(daily)));

            return dailies;
        }
    }
}
