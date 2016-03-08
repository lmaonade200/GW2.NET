// <copyright file="AchievementDataModel.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Achievements.ApiModels
{
    using System.Collections.Generic;

    public class AchievementDataModel
    {
        public int Id { get; set; }

        public string Icon { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Requirement { get; set; }

        public string Type { get; set; }

        public string[] Flags { get; set; }

        public TierDataModel[] Tiers { get; set; }

        public AchievementRewardDataModel[] Rewards { get; set; }

        public KeyValuePair<string, object>[] Bits { get; set; }

        public int? Point_Cap { get; set; }
    }
}