// <copyright file="TierCollectionConverter.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Achievements.Converter
{
    using GW2NET.Achievements.ApiModels;
    using GW2NET.Common;

    public class TierCollectionConverter : IConverter<TierDataModel, Tier>
    {
        /// <inheritdoc />
        public Tier Convert(TierDataModel value, object state = null)
        {
            return new Tier
            {
                Count = value.Count,
                Points = value.Points
            };
        }
    }
}