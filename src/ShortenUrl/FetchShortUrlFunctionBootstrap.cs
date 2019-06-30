using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShortenUrl
{
    public class FetchShortUrlFunctionBootstrap : ApiFunctionBootstrapBase<FetchShortUrlHandler>
    {
        public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest apigProxyEvent, ILambdaContext context)
        {
            return await Handle(apigProxyEvent);
        }
    }
}
