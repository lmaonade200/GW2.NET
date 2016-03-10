namespace GW2NET.Achievements.ApiModels
{
    using System.Collections.Generic;

    public class DailyAchievementsDataModel
    {
        public IEnumerable<DailyAchievementDataModel> Pve { get; set; }

        public IEnumerable<DailyAchievementDataModel> Pvp { get; set; }

        public IEnumerable<DailyAchievementDataModel> Wvw { get; set; }

        public IEnumerable<DailyAchievementDataModel> Special { get; set; }
    }
}