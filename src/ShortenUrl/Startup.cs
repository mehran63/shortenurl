using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShortenUrl.BusinessLogic;
using ShortenUrl.Repository;
using ShortenUrl.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ShortenUrl
{
    public class Startup
    {
        public static ServiceProvider ServiceProvider { get; set; }

        static Startup()
        {
            ServiceProvider = BuildServiceProvider();
        }

        private static ServiceProvider BuildServiceProvider()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            //var configuration = GetConfiguration();

            return serviceCollection.BuildServiceProvider();
        }

        private static void ConfigureServices(ServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ShortenUrlHandler>();
            serviceCollection.AddTransient<FetchShortUrlHandler>();
            serviceCollection.AddTransient<IToShortUrlRepository, ToShortUrlRepository>();
            serviceCollection.AddTransient<IFromShortUrlRepository, FromShortUrlRepository>();
            serviceCollection.AddTransient<IShortUrlGenerator, ShortUrlGenerator>();
            serviceCollection.AddTransient<IShortUrlManager, ShortUrlManager>();
            serviceCollection.AddTransient<IRandomStringGenerator, RandomStringGenerator>();
            serviceCollection.AddTransient<IDateTimeProvider, DateTimeProvider>();
            serviceCollection.AddTransient<ILongUrlValidator, LongUrlValidator>();
            serviceCollection.AddTransient<IDynamoDBContext>(BuildDynamoDBContext);

            serviceCollection.AddSingleton<IAmazonDynamoDB, AmazonDynamoDBClient>();
            //TODO: Improve by reading from environment variables or Parameter Store
            serviceCollection.AddSingleton(new ShortUrlManagerSettings()
            {
                ToShortUrlTtlDays = 1,
                FromShortUrlTtlDays = 7
            });
        }

        private static IDynamoDBContext BuildDynamoDBContext(IServiceProvider serviceProvider)
        {
            var amazonDynamoDB = serviceProvider.GetService<IAmazonDynamoDB>();
            return new DynamoDBContext(amazonDynamoDB);
        }

        //public static IConfiguration GetConfiguration()
        //{
        //    return new ConfigurationBuilder()
        //        .SetBasePath(Directory.GetCurrentDirectory())
        //        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        //        .Build();
        //}
    }
}
