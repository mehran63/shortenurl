using Amazon.Lambda.APIGatewayEvents;
using System.Threading.Tasks;

namespace ShortenUrl
{
    public interface IHttpRequestHandler
    {
        Task<APIGatewayProxyResponse> Handle(APIGatewayProxyRequest request);
    }
}