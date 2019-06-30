using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using ShortenUrl.BusinessLogic;
using ShortenUrl.Repository;

namespace ShortenUrl
{
    public class FetchShortUrlHandler : IHttpRequestHandler
    {
        private readonly IShortUrlManager shortUrlManager;

        public FetchShortUrlHandler(
            IShortUrlManager shortUrlManager)
        {
            this.shortUrlManager = shortUrlManager;
        }
        public async Task<APIGatewayProxyResponse> Handle(APIGatewayProxyRequest request)
        {
            var shortUrlKey = request.Path.TrimEnd('/').Split('/').LastOrDefault();

            if (string.IsNullOrEmpty(shortUrlKey)
                || shortUrlKey.Length > 10)
            {
                return new APIGatewayProxyResponse
                {
                    Body = "No short url key is specified or it is longer than 10 characters",
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }

            var longURl = await shortUrlManager.GetLongUrl(shortUrlKey);

            if (string.IsNullOrEmpty(longURl))
            {
                return new APIGatewayProxyResponse { StatusCode = (int)HttpStatusCode.NotFound };
            }

            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.PermanentRedirect,
                Headers = new Dictionary<string, string> { { "location", longURl } }
            };
        }
    }
}
