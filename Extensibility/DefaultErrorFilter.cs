
namespace Sarkaran.Http.Client.Extensibility
{
    public class DefaultErrorFilter : IHttpFilter
    {
        public void OnRequest(IRequest request)
        {
            // Do Nothing
        }

        public void OnResponse(IResponse response, bool httpErrorAsException)
        {
            if (httpErrorAsException && !response.Message.IsSuccessStatusCode)
            {
                var responseMessage = response.Message.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                throw new APIException(response, $"The API query failed with status code {response.Message.StatusCode}: {response.Message.ReasonPhrase}. Orginal Error - { responseMessage}" );

               
            }
        }
    }
}
