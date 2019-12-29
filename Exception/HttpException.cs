

namespace Sarkaran.Http.Client
{
    using System;
    using System.Net;
    using System.Net.Http;

    public class APIException : Exception
    {
        public HttpStatusCode StatusCode { get;}
        public IResponse Response { get; }
        public HttpResponseMessage HttpResponseMessage { get; set; } 

        public APIException(IResponse response, string message, Exception innerException = null)
            : base(message, innerException)
        {
            this.Response = response;
            this.HttpResponseMessage = response.Message;
            this.StatusCode = response.Message.StatusCode;
        }
    }
}
