﻿
namespace Sarkaran.Http.Client.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Net.Http.Headers;
    using System.Text;

    /// <summary>Constructs HTTP request bodies.</summary>
    internal class BodyBuilder : IBodyBuilder
    { 
        /// <summary>The underlying request.</summary>
        private readonly IRequest Request;


        /// <summary>Construct an instance.</summary>
        /// <param name="request">The underlying request.</param>
        public BodyBuilder(IRequest request)
        {
            this.Request = request;
        }

        /// <summary>Get a form URL-encoded body.</summary>
        /// <param name="arguments">An anonymous object containing the property names and values to set.</param>
        /// <example><code>client.WithArguments(new { id = 14, name = "Joe" })</code></example>
        public HttpContent FormUrlEncoded(object arguments)
        {
            return this.FormUrlEncoded(arguments.GetKeyValueArguments());
        }

        /// <summary>Get a form URL-encoded body.</summary>
        /// <param name="arguments">An anonymous object containing the property names and values to set.</param>
        public HttpContent FormUrlEncoded(IDictionary<string, string> arguments)
        {
            return new FormUrlEncodedContent(
                from pair in arguments
                where pair.Value != null || this.Request.Options.IgnoreNullArguments != true
                select new KeyValuePair<string, string>(pair.Key, pair.Value)
            );
        }

        /// <summary>Get a form URL-encoded body.</summary>
        /// <param name="arguments">An anonymous object containing the property names and values to set.</param>
        /// <example><code>client.WithArguments(new[] { new KeyValuePair&lt;string, string&gt;("genre", "drama"), new KeyValuePair&lt;string, int&gt;("genre", "comedy") })</code></example>
        public HttpContent FormUrlEncoded(IEnumerable<KeyValuePair<string, object>> arguments)
        {
            // bypass FormUrlEncodedContent, which restricts the body length to the maximum size of a URL. That's not applicable for a URL-encoded body.
            IEnumerable<string> pairs =
            (
                from pair in arguments
                where pair.Value != null || this.Request.Options.IgnoreNullArguments != true
                select $"{WebUtility.UrlEncode(pair.Key)}={WebUtility.UrlEncode(pair.Value?.ToString())}"
            );
            return new StringContent(string.Join("&", pairs), Encoding.UTF8, "application/x-www-form-urlencoded");
        }

        /// <summary>Get the serialized model body.</summary>
        /// <param name="body">The value to serialize into the HTTP body content.</param>
        /// <param name="contentType">The request body format (or <c>null</c> to use the first supported Content-Type in the client's formatter).</param>
        /// <returns>Returns the request builder for chaining.</returns>
        /// <exception cref="InvalidOperationException">No MediaTypeFormatters are available on the API client for this content type.</exception>
        public HttpContent Model<T>(T body, MediaTypeHeaderValue contentType = null)
        {
            MediaTypeFormatter formatter = Factory.GetFormatter(this.Request.Formatters, contentType);
            string mediaType = contentType?.MediaType;
            return new ObjectContent<T>(body, formatter, mediaType);
        }
        /// <summary>Get a serialised model body.</summary>
        /// <param name="body">The value to serialize into the HTTP body content.</param>
        /// <param name="formatter">The media type formatter with which to format the request body format.</param>
        /// <param name="mediaType">The HTTP media type (or <c>null</c> for the <paramref name="formatter"/>'s default).</param>
        /// <returns>Returns the request builder for chaining.</returns>
        public HttpContent Model<T>(T body, MediaTypeFormatter formatter, string mediaType = null)
        {
            return new ObjectContent<T>(body, formatter, mediaType);
        }
    }
}
