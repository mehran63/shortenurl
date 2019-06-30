using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;
using ShortenUrl.BusinessLogic;

namespace ShortenUrl
{
    public class ShortenUrlHandler : IHttpRequestHandler
    {
        private readonly IShortUrlManager shortUrlManager;

        public ShortenUrlHandler(
            IShortUrlManager shortUrlManager)
        {
            this.shortUrlManager = shortUrlManager;
        }

        public async Task<APIGatewayProxyResponse> Handle(APIGatewayProxyRequest request)
        {
            var longUrl = request.Body;

            if (string.IsNullOrEmpty(longUrl))
            {
                return new APIGatewayProxyResponse
                {
                    Body = "No body!",
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }

            if (longUrl.Length > 3000)
            {
                return new APIGatewayProxyResponse
                {
                    Body = "Maximum valid body length is 3000!",
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }

            var shortUrlKey = await shortUrlManager.GetShortUrlKey(longUrl);

            return new APIGatewayProxyResponse
            {
                Body = shortUrlKey,
                StatusCode = (int)HttpStatusCode.OK,
                Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
            };
        }
    }
}
