// <copyright file="BuildConverterTests.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Miscellaneous
{
    using System;

    using GW2NET.Builds;
    using GW2NET.Common.Converters;
    using GW2NET.Miscellaneous.ApiModels;
    using GW2NET.Miscellaneous.Converter;

    using Xunit;

    public class BuildConverterTests
    {
        private readonly BuildConverter converter = new BuildConverter();

        [Theory]
        [InlineData(0, "Tue, 26 May 2015 18:46:01 GMT")]
        [InlineData(1, "Tue, 26 May 2015 18:46:01 GMT")]
        [InlineData(-1, "Tue, 26 May 2015 18:46:01 GMT")]
        [InlineData(-1, "Tue, 26 May 2015 18:46:01 GMT")]
        [InlineData(10000, "Tue, 26 May 2015 18:46:01 GMT")]
        [InlineData(100000, "Tue, 26 May 2015 18:46:01 GMT")]
        public void CanConvert(int buildId, DateTime date)
        {
            BuildDataModel value = new BuildDataModel { BuildId = buildId };
            ApiMetadata state = new ApiMetadata
            {
                RequestDate = date
            };
            Build result = this.converter.Convert(value, state);
            Assert.NotNull(result);
            Assert.Equal(buildId, result.BuildId);
            Assert.Equal(date, result.Timestamp);
        }

        [Fact]
        public void ValueNull()
        {
            Assert.Throws<ArgumentNullException>(() => this.converter.Convert(null, null));
        }

        [Fact]
        public void StateNull()
        {
            BuildDataModel value = new BuildDataModel { BuildId = 0 };
            Assert.Throws<ArgumentNullException>(() => this.converter.Convert(value, null));
        }

        [Fact]
        public void CannotConvert()
        {
            BuildDataModel value = new BuildDataModel { BuildId = 0 };
            Assert.Throws<ArgumentException>(() => this.converter.Convert(value, new object()));
        }
    }
}
