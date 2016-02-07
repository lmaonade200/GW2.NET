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
            var contentStream = await this.GetContent(responseMessage, state);

            IEnumerable<TInput> response = this.serializerFactory.GetSerializer<IEnumerable<TInput>>().Deserialize(contentStream);

            ConcurrentBag<TOutput> items = new ConcurrentBag<TOutput>();

            //ToDo: Implement proper
            var responseState = new Response<TInput>
            {
                Culture = CultureInfo.CurrentCulture,
                Date = new DateTimeOffset(DateTime.Now)
            };

            Parallel.ForEach(response, item =>
            {
                items.Add(innerConverter.Convert(item, responseState));
            });

            return items;
        }

        public async Task<TOutput> ConvertElementAsync<TInput, TOutput>(HttpResponseMessage responseMessage, IConverter<TInput, TOutput> innerConverter, CancellationToken cancellationToken, object state = null)
        {
            var contentStream = await this.GetContent(responseMessage, state);

            TInput response = this.serializerFactory.GetSerializer<TInput>().Deserialize(contentStream);

            //ToDo: Implement proper
            var responseState = new Response<TInput>
            {
                Culture = CultureInfo.CurrentCulture,
                Date = new DateTimeOffset(DateTime.Now)
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

                var encoding = message.Headers.SingleOrDefault(h => h.Key == "Content-Encoding").Value;
                if (encoding != null)
                {
                    if (encoding.FirstOrDefault().Equals("gzip", StringComparison.OrdinalIgnoreCase))
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
    }
}
