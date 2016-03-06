// <copyright file="BuildRepositoryTests.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Miscellaneous
{
    using System;
    using System.Net.Http;
    using System.Threading;

    public class BuildRepositoryTests
    {
        private readonly IBuildRepository buildRepository;

        public BuildRepositoryTests()
        {
            HttpClient client = new HttpClient(new MockBuildMessageHandler(), true)
            {
                BaseAddress = new Uri("http://test.de")
            };
            ISerializerFactory serializer = new JsonSerializerFactory();
            GzipInflator gzipInflator = new GzipInflator();

            HttpResponseConverter responseConverter = new HttpResponseConverter(serializer, serializer, gzipInflator);

            this.buildRepository = new BuildRepository(client, responseConverter, new BuildConverter());
        }

        [Fact]
        public void ConverterNull()
        {
            HttpClient client = new HttpClient(new MockBuildMessageHandler(), true)
            {
                BaseAddress = new Uri("http://test.de")
            };
            ISerializerFactory serializer = new JsonSerializerFactory();
            GzipInflator gzipInflator = new GzipInflator();

            HttpResponseConverter responseConverter = new HttpResponseConverter(serializer, serializer, gzipInflator);

            Assert.Throws<ArgumentNullException>(() => new BuildRepository(client, responseConverter, null));
        }

        [Fact]
        public async void GetBuildAsync()
        {
            Build result = await this.buildRepository.GetBuildAsync();
            Assert.NotNull(result);
            Assert.NotInRange(result.BuildId, int.MinValue, 0);
            Assert.NotStrictEqual(default(DateTimeOffset), result.Timestamp);
        }

        [Fact]
        public async void GetBuildAsyncCancellationToken()
        {
            Build result = await this.buildRepository.GetBuildAsync(CancellationToken.None);
            Assert.NotNull(result);
            Assert.NotInRange(result.BuildId, int.MinValue, 0);
            Assert.NotStrictEqual(default(DateTimeOffset), result.Timestamp);
        }
    }
}