// <copyright file="RenderRepository.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Rendering
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    using GW2NET.Common;

    /// <summary>Provides the default implementation of the render service.</summary>
    public class RenderRepository : IRenderRepository
    {
        private readonly HttpClient client;

        /// <summary>Initializes a new instance of the <see cref="RenderRepository"/> class.</summary>
        /// <param name="client">The client used to make requests.</param>
        /// <exception cref="ArgumentNullException">The value of <paramref name="client"/> is a null reference.</exception>
        public RenderRepository(HttpClient client)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            this.client = client;
        }

        /// <inheritdoc />
        public Task<byte[]> GetImageAsync(IRenderable file, string imageFormat)
        {
            return this.GetImageAsync(file, imageFormat, CancellationToken.None);
        }

        /// <inheritdoc />
        public async Task<byte[]> GetImageAsync(IRenderable file, string imageFormat, CancellationToken cancellationToken)
        {
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, $"/file/{file.FileSignature}/{file.FileId}.{imageFormat}");

            return await (await this.client.SendAsync(message, cancellationToken)).Content.ReadAsByteArrayAsync();
        }
    }
}