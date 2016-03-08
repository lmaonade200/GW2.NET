// <copyright file="RewardsConverter.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Achievements.Converter
{
    using GW2NET.Achievements.ApiModels;
    using GW2NET.Common;

    /// <summary>Converts a <see cref="AchievementRewardDataModel"/> into the corresponding <see cref="Reward"/> object.</summary>
    public class RewardsConverter : IConverter<AchievementRewardDataModel, Reward>
    {
        /// <inheritdoc />
        public Reward Convert(AchievementRewardDataModel value, object state = null)
        {
            switch (value.Type)
            {
                case "Item":
                    return new ItemReward { Count = value.Count, Id = value.Id };
                case "Mastery":
                    return new MasteryReward { Region = value.Region };
                default:
                    throw new SerializationException($"The type '{value.Type}' could not be converted into a reward object.");
            }
        }
    }
}