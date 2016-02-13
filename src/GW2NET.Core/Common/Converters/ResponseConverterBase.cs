namespace GW2NET.Common.Converters
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    /// <summary>Provides methods to convert an <see cref="HttpResponseMessage"/> into a suitable data object.</summary>
    public abstract class ResponseConverterBase
    {
        /// <summary>Converts an <see cref="HttpResponseMessage"/> into a set of objects of type <see cref="TOutput"/>.</summary>
        /// <typeparam name="TInput">The data contract type.</typeparam>
        /// <typeparam name="TOutput">The data object type.</typeparam>
        /// <param name="responseMessage">The response message recieved from the <see cref="HttpClient"/>.</param>
        /// <param name="innerConverter">The inner converter used to convert data contracts into data objects.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> containing objects of type <see cref="TOutput"/>.</returns>
        public abstract Task<IEnumerable<TOutput>> ConvertSetAsync<TInput, TOutput>(HttpResponseMessage responseMessage, IConverter<TInput, TOutput> innerConverter);

        /// <summary>
        /// Converts an <see cref="HttpResponseMessage"/> into a single object of type <see cref="TOutput"/>.</summary>
        /// <typeparam name="TInput">The data contract type.</typeparam>
        /// <typeparam name="TOutput">The data object type.</typeparam>
        /// <param name="responseMessage">The response message recieved from the <see cref="HttpClient"/>.</param>
        /// <param name="innerConverter">The inner converter used to convert data contracts into data objects.</param>
        /// <returns>An object of type <see cref="TOutput"/>.</returns>
        public abstract Task<TOutput> ConvertElementAsync<TInput, TOutput>(HttpResponseMessage responseMessage, IConverter<TInput, TOutput> innerConverter);
    }
}