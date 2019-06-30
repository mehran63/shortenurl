using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using ShortenUrl.Repository;

namespace ShortenUrl
{
    public class FetchShortUrlHandler : IHttpRequestHandler
    {
        private readonly IFromShortUrlRepository fromShortUrlRepository;

        public FetchShortUrlHandler(
            IFromShortUrlRepository fromShortUrlRepository)
        {
            this.fromShortUrlRepository = fromShortUrlRepository;
        }
        public async Task<APIGatewayProxyResponse> Handle(APIGatewayProxyRequest request)
        {
            var shortUrlKey = request.Path.TrimEnd('/').Split('/').LastOrDefault();

            if (string.IsNullOrEmpty(shortUrlKey))
            {
                return new APIGatewayProxyResponse { StatusCode = (int)HttpStatusCode.BadRequest };
            }

            var longURl = await fromShortUrlRepository.FetchLongUrl(shortUrlKey);

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
