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
        private readonly ILongUrlValidator longUrlValidator;

        public ShortenUrlHandler(
            IShortUrlManager shortUrlManager,
            ILongUrlValidator longUrlValidator)
        {
            this.shortUrlManager = shortUrlManager;
            this.longUrlValidator = longUrlValidator;
        }

        public async Task<APIGatewayProxyResponse> Handle(APIGatewayProxyRequest request)
        {
            var longUrl = request.Body;

            if(!longUrlValidator.Validate(longUrl, out string error))
            {
                return new APIGatewayProxyResponse
                {
                    Body = "Request body is invalid, "+ error,
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }

            var shortUrlKey = await shortUrlManager.GetShortUrlKey(longUrl);

            return new APIGatewayProxyResponse
            {
                Body = shortUrlKey,
                StatusCode = (int)HttpStatusCode.OK,
                Headers = new Dictionary<string, string> {
                    { "Content-Type", "text/plain" },
                    { "Access-Control-Allow-Origin", "*" },
                    { "Access-Control-Allow-Headers", "*"},
                    { "Access-Control-Allow-Methods", "POST"}
                }
            };
        }
    }
}
