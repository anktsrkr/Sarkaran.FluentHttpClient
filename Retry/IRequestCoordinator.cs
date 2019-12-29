﻿
namespace Sarkaran.Http.Client.Retry
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    /// <summary>Defines how the HTTP client should dispatch requests and process responses at a low level, for example to handle transient failures and errors. Only one may be used by the client.</summary>
    public interface IRequestCoordinator
    {
        /// <summary>Dispatch an HTTP request.</summary>
        /// <param name="request">The request.</param>
        /// <param name="dispatcher">A method which executes the request.</param>
        /// <returns>The final HTTP response.</returns>
        Task<HttpResponseMessage> ExecuteAsync(IRequest request, Func<IRequest, Task<HttpResponseMessage>> dispatcher);
    }
}
