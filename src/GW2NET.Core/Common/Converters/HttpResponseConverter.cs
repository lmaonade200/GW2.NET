// <copyright file="HttpResponseConverter.cs" company="GW2.NET Coding Team">
// This product is licensed under the GNU General Public License version 2 (GPLv2). See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>

namespace GW2NET.Common.Converters
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    using GW2NET.Common.Serializers;

    public class HttpResponseConverter : IResponseConverter
    {
        private readonly ISerializerFactory serializerFactory;

        private readonly ISerializerFactory errorSerializerFactory;

        private readonly IConverter<Stream, Stream> gzipInflator;

        public HttpResponseConverter(ISerializerFactory serializerFactory, ISerializerFactory errorSerializerFactory, IConverter<Stream, Stream> gzipInflator)
        {
            this.serializerFactory = serializerFactory;
            this.gzipInflator = gzipInflator;
            this.errorSerializerFactory = errorSerializerFactory;
        }

        public async Task<IEnumerable<TOutput>> ConvertSetAsync<TInput, TOutput>(HttpResponseMessage responseMessage, IConverter<TInput, TOutput> innerConverter)
        {
            IEnumerable<TInput> response = await this.GetContentAsync<IEnumerable<TInput>>(responseMessage);
            ApiMetadata metadata = this.GetMetadata(responseMessage);

            ConcurrentBag<TOutput> items = new ConcurrentBag<TOutput>();

            Parallel.ForEach(response, item =>
             {
                 items.Add(innerConverter.Convert(item, metadata));
             });

            return items;
        }

        public async Task<TOutput> ConvertElementAsync<TInput, TOutput>(HttpResponseMessage responseMessage, IConverter<TInput, TOutput> innerConverter)
        {
            TInput response = await this.GetContentAsync<TInput>(responseMessage);
            ApiMetadata metadata = this.GetMetadata(responseMessage);

            return innerConverter.Convert(response, metadata);
        }

        private async Task<TResult> DeserializeAsync<TResult>(HttpContent content, ISerializerFactory serializer, IConverter<Stream, Stream> compressionConverter)
        {
            Stream contentStream = new MemoryStream();

            await content.CopyToAsync(contentStream);

            ICollection<string> contentEncoding = content.Headers.ContentEncoding;
            if (contentEncoding != null && contentEncoding.Count > 0)
            {
                if (contentEncoding.FirstOrDefault().Equals("gzip", StringComparison.OrdinalIgnoreCase))
                {
                    Stream uncompressed = compressionConverter.Convert(contentStream, null);
                    if (uncompressed == null)
                    {
                        throw new InvalidOperationException("Could not read stream.");
                    }

                    contentStream = uncompressed;
                }
            }

            await contentStream.FlushAsync();
            contentStream.Position = 0;
            return serializer.GetSerializer<TResult>().Deserialize(contentStream);
        }

        private async Task<TContent> GetContentAsync<TContent>(HttpResponseMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            using (HttpContent content = message.Content)
            {
                if (!message.IsSuccessStatusCode)
                {
                    MediaTypeHeaderValue contentType = content.Headers.ContentType;

                    if (contentType != null
                        && contentType.MediaType.Equals("application/json", StringComparison.OrdinalIgnoreCase))
                    {
                        // Get the response content
                        ErrorResult errorResult = await this.DeserializeAsync<ErrorResult>(content, this.errorSerializerFactory, this.gzipInflator);

                        // Get the error description, or null if none was returned
                        throw new ServiceException(errorResult?.Text);
                    }
                }

                return await this.DeserializeAsync<TContent>(content, this.serializerFactory, this.gzipInflator);
            }
        }

        private ApiMetadata GetMetadata(HttpResponseMessage message)
        {
            ApiMetadata metadata = new ApiMetadata();

            using (HttpContent content = message.Content)
            {
                metadata.ContentLanguage = new CultureInfo(content.Headers.ContentLanguage.First());
                metadata.RequestDate = message.Headers.Date ?? default(DateTimeOffset);
                metadata.ExpireDate = content.Headers.Expires ?? default(DateTimeOffset);

                string resultTotal = message.Headers.SingleOrDefault(h => string.Equals(h.Key, "X-Result-Total", StringComparison.OrdinalIgnoreCase)).Value?.FirstOrDefault();
                metadata.ResultTotal = !string.IsNullOrEmpty(resultTotal) ? Convert.ToInt32(resultTotal) : -1;

                string resultCount = message.Headers.SingleOrDefault(h => string.Equals(h.Key, "X-Result-Count", StringComparison.OrdinalIgnoreCase)).Value?.FirstOrDefault();
                metadata.ResultCount = !string.IsNullOrEmpty(resultCount) ? Convert.ToInt32(resultCount) : -1;
            }

            return metadata;
        }
    }
}