using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
namespace ShortenUrl
{
  public  class ApiFunctionBootstrapBase<THandler>
        where THandler: IHttpRequestHandler
    {
        protected async Task<APIGatewayProxyResponse> Handle(APIGatewayProxyRequest request)
        {
            using (var serviceProviderScope = Startup.ServiceProvider.CreateScope())
            {
               return await serviceProviderScope.ServiceProvider.GetService<THandler>().Handle(request);
            }
        }
    }
}
