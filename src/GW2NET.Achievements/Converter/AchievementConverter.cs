// <copyright file="AchievementConverter.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Achievements.Converter
{
    using System;
    using System.Collections.Generic;

    using GW2NET.Achievements.ApiModels;
    using GW2NET.Common;

    /// <summary>Converts an <see cref="AchievementDataModel"/> into the corresponding <see cref="Achievement"/>.</summary>
    public class AchievementConverter : IConverter<AchievementDataModel, Achievement>
    {
        private readonly IConverter<string[], AchievementFlags> flagsConverter;

        private readonly IConverter<IEnumerable<TierDataModel>, IEnumerable<Tier>> tiersConverter;

        private readonly IConverter<IEnumerable<AchievementRewardDataModel>, IEnumerable<Reward>> rewardsConverter;

        private readonly IConverter<IEnumerable<KeyValuePair<string, object>>, IEnumerable<AchievementBit>> bitsConverter;

        /// <summary>Initializes a new instance of the <see cref="AchievementConverter"/> class.</summary>
        /// <param name="flagsConverter"></param>
        /// <param name="tiersConverter"></param>
        /// <param name="rewardsConverter"></param>
        /// <param name="bitsConverter"></param>
        public AchievementConverter(IConverter<string[], AchievementFlags> flagsConverter, IConverter<IEnumerable<TierDataModel>, IEnumerable<Tier>> tiersConverter, IConverter<IEnumerable<AchievementRewardDataModel>, IEnumerable<Reward>> rewardsConverter, IConverter<IEnumerable<KeyValuePair<string, object>>, IEnumerable<AchievementBit>> bitsConverter)
        {
            if (flagsConverter == null)
            {
                throw new ArgumentNullException(nameof(flagsConverter));
            }

            if (tiersConverter == null)
            {
                throw new ArgumentNullException(nameof(tiersConverter));
            }

            if (rewardsConverter == null)
            {
                throw new ArgumentNullException(nameof(rewardsConverter));
            }

            if (bitsConverter == null)
            {
                throw new ArgumentNullException(nameof(bitsConverter));
            }

            this.flagsConverter = flagsConverter;
            this.tiersConverter = tiersConverter;
            this.rewardsConverter = rewardsConverter;
            this.bitsConverter = bitsConverter;
        }

        /// <inheritdoc />
        public Achievement Convert(AchievementDataModel value, object state)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            Achievement achievement;
            switch (value.Type)
            {
                case "ItemSet":
                    achievement = new ItemSetAchievement();
                    break;
                case "Default":
                    achievement = new Achievement();
                    break;
                default:
                    throw new SerializationException($"The type '{value.Type}' could not be converted into a achivement object.");
            }

            achievement.Id = value.Id;
            achievement.Icon = string.IsNullOrEmpty(value.Icon) ? null : value.Icon;
            achievement.Name = value.Name;
            achievement.Description = value.Description;
            achievement.Requirement = value.Requirement;
            achievement.Flags = this.flagsConverter.Convert(value.Flags);
            achievement.Tiers = this.tiersConverter.Convert(value.Tiers);
            achievement.Rewards = this.rewardsConverter.Convert(value.Rewards);
            achievement.Bits = this.bitsConverter.Convert(value.Bits);
            achievement.PointCap = value.Point_Cap;

            return achievement;
        }
    }
}
