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

    public class HttpResponseConverter
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

        public async Task<IEnumerable<TOutput>> ConvertCollectionAsync<TInput, TOutput>(HttpResponseMessage responseMessage, IConverter<TInput, TOutput> innerConverter)
        {
            IEnumerable<TInput> response = await this.GetContentAsync<IEnumerable<TInput>>(responseMessage);
            ApiMetadata metadata = this.GetMetadata(responseMessage);

            ConcurrentBag<TOutput> items = new ConcurrentBag<TOutput>();

            // Conversion is currently done, so the old converter
            // infrastructure can be kept intact for now.
            // This should be changed later down the road.
            Response<TInput> responseState = new Response<TInput>
            {
                Culture = metadata.ContentLanguage,
                Date = metadata.RequestDate
            };

            Parallel.ForEach(response, item =>
            {
                items.Add(innerConverter.Convert(item, responseState));
            });

            return items;
        }

        public async Task<TOutput> ConvertElementAsync<TInput, TOutput>(HttpResponseMessage responseMessage, IConverter<TInput, TOutput> innerConverter)
        {
            TInput response = await this.GetContentAsync<TInput>(responseMessage);
            ApiMetadata metadata = this.GetMetadata(responseMessage);

            // Conversion is currently done, so the old converter
            // infrastructure can be kept intact for now.
            // This should be changed later down the road.
            Response<TInput> responseState = new Response<TInput>
            {
                Culture = metadata.ContentLanguage,
                Date = metadata.RequestDate
            };

            return innerConverter.Convert(response, responseState);
        }

        private async Task<TResult> DeserializeAsync<TResult>(HttpContent content, ISerializerFactory serializer, IConverter<Stream, Stream> compressionConverter)
        {
            byte[] buffer = new byte[4096];

            Stream contentStream = new MemoryStream(buffer, true);

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
