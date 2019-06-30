using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using ShortenUrl.DataModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShortenUrl.Repository
{
    public class FromShortUrlRepository : IFromShortUrlRepository
    {
        private readonly IDynamoDBContext context;

        public FromShortUrlRepository(IDynamoDBContext dynamoDBContext)
        {
            this.context = dynamoDBContext;
        }

        public async Task<string> FetchLongUrl(string shortUrlKey)
        {
            //var context = new DynamoDBContext(amazonDynamoDB);

            var record = await context.LoadAsync<FromShortUrl>(shortUrlKey);

            return record?.LongUrl;
        }

        public async Task Store(string longUrl, string shortUrlKey)
        {
            var record = new FromShortUrl
            {
                LongUrl = longUrl,
                ShortUrlKey = shortUrlKey
            }
            ;
            await context.SaveAsync(record);
        }
    }
}
