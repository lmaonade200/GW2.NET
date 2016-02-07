namespace GW2NET.Common.Converters
{
    using System;
    using System.Collections.Generic;
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

        public async Task<TOutput> ConvertAsync<TInput, TOutput>(HttpResponseMessage responseMessage, IConverter<TInput, TOutput> innerConverter, CancellationToken cancellationToken, object state = null)
        {
            if (responseMessage == null)
            {
                throw new ArgumentNullException(nameof(responseMessage));
            }

            using (HttpContent content = responseMessage.Content)
            {
                Stream contentStream = await content.ReadAsStreamAsync();
                if (contentStream == null)
                {
                    throw new ServiceException("The content stream is null.");
                }

                // ToDo: Needs to be done better!
                IEnumerable<string> encoding;
                if (responseMessage.Headers.TryGetValues("Content-Encoding", out encoding))
                {
                    List<string> encodingList = encoding.ToList();

                    if (encodingList.FirstOrDefault() != null)
                    {
                        bool compressed = encodingList.First().Equals("gzip", StringComparison.OrdinalIgnoreCase);
                        if (compressed)
                        {
                            Stream uncompressed = this.gzipInflator.Convert(contentStream, state);
                            if (uncompressed != null)
                            {
                                contentStream = uncompressed;
                            }
                        }
                    }
                }

                TInput responseContent = this.serializerFactory.GetSerializer<TInput>().Deserialize(contentStream);

                return innerConverter.Convert(responseContent, state);
            }
        }
    }
}
