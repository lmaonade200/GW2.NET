﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IApiResponse.cs" company="GW2.Net Coding Team">
//   This product is licensed under the GNU General Public License version 2 (GPLv2) as defined on the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Net.Mime;

namespace GW2DotNET.V1.Core
{
    /// <summary>
    /// Provides the base interface for HTTP responses originating from the Guild Wars 2 API.
    /// </summary>
    /// <typeparam name="TContent">The type of the response content.</typeparam>
    public interface IApiResponse<out TContent>
    {
        /// <summary>
        /// Gets a value indicating the Internet media type of the message content.
        /// </summary>
        ContentType ContentType { get; }

        /// <summary>
        /// Gets a value indicating whether the service returned a success status code.
        /// </summary>
        bool IsSuccessStatusCode { get; }

        /// <summary>
        /// Gets a value indicating whether the service returned a JSON response.
        /// </summary>
        bool IsJsonResponse { get; }

        /// <summary>
        /// Gets the error result if the request was unsuccessful.
        /// </summary>
        /// <returns>Return the error response as an instance of the <see cref="ApiException"/> class.</returns>
        ApiException DeserializeError();

        /// <summary>
        /// Gets the response content as an object of the specified type.
        /// </summary>
        /// <returns>Returns the response as an instance of the specified type.</returns>
        TContent DeserializeResponse();

        /// <summary>
        /// Throws an exception if the request did not return a success status code.
        /// </summary>
        /// <returns>Returns the current instance.</returns>
        /// <remarks>The current instance is returned to allow chaining method calls.</remarks>
        IApiResponse<TContent> EnsureSuccessStatusCode();
    }
}