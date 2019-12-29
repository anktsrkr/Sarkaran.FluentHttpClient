using System;

namespace Sarkaran.Http.Client
{ 
    /// <summary>Options for the fluent client.</summary>
    public class FluentClientOptions
    {
        /// <summary>Whether to ignore null arguments when the request is dispatched.</summary>
        public bool? IgnoreNullArguments { get; set; }

        /// <summary>Whether HTTP error responses (e.g. HTTP 404) should be ignored (else raised as exceptions).</summary>
        public bool? IgnoreHttpErrors { get; set; }

        /// <summary>Get the equivalent request options.</summary>
        internal RequestOptions ToRequestOptions()
        {
            return new RequestOptions
            {
                IgnoreHttpErrors = this.IgnoreHttpErrors,
                IgnoreNullArguments = this.IgnoreNullArguments
            };
        }
    }
}
