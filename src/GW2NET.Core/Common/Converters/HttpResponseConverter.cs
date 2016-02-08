namespace GW2NET.Common.Converters
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    using GW2NET.Common.Serializers;

    public class HttpResponseConverter
    {
        private readonly ISerializerFactory serializerFactory;

        private readonly IConverter<Stream, Stream> gzipInflator;

        public HttpResponseConverter(ISerializerFactory serializerFactory, IConverter<Stream, Stream> gzipInflator)
        {
            this.serializerFactory = serializerFactory;
            this.gzipInflator = gzipInflator;
        }

        public async Task<IEnumerable<TOutput>> ConvertCollectionAsync<TInput, TOutput>(HttpResponseMessage responseMessage, IConverter<TInput, TOutput> innerConverter, CancellationToken cancellationToken, object state = null)
        {
            Stream contentStream = await this.GetContent(responseMessage, state);
            ApiMetadata metadata = this.GetMetadata(responseMessage);

            IEnumerable<TInput> response = this.serializerFactory.GetSerializer<IEnumerable<TInput>>().Deserialize(contentStream);

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

        public async Task<TOutput> ConvertElementAsync<TInput, TOutput>(HttpResponseMessage responseMessage, IConverter<TInput, TOutput> innerConverter, CancellationToken cancellationToken, object state = null)
        {
            Stream contentStream = await this.GetContent(responseMessage, state);
            ApiMetadata metadata = this.GetMetadata(responseMessage);

            TInput response = this.serializerFactory.GetSerializer<TInput>().Deserialize(contentStream);

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

        private async Task<Stream> GetContent(HttpResponseMessage message, object state = null)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            using (HttpContent content = message.Content)
            {
                byte[] buffer = new byte[4096];

                Stream contentStream = new MemoryStream(buffer, true);

                await content.CopyToAsync(contentStream);

                if (contentStream.Length == 0)
                {
                    throw new ServiceException("Could not read content stream.");
                }

                ICollection<string> contentEncoding = content.Headers.ContentEncoding;
                if (contentEncoding != null && contentEncoding.Count > 0)
                {
                    if (contentEncoding.FirstOrDefault().Equals("gzip", StringComparison.OrdinalIgnoreCase))
                    {
                        Stream uncompressed = this.gzipInflator.Convert(contentStream, state);
                        if (uncompressed == null)
                        {
                            throw new InvalidOperationException("Could not read stream.");
                        }

                        contentStream = uncompressed;
                    }
                }

                await contentStream.FlushAsync();
                contentStream.Position = 0;
                return contentStream;
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
