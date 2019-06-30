using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Util;
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

        public async Task Store(string longUrl, string shortUrlKey, DateTime expireOn)
        {
            int epochSeconds = AWSSDKUtils.ConvertToUnixEpochSeconds(expireOn);
            var record = new FromShortUrl
            {
                LongUrl = longUrl,
                ShortUrlKey = shortUrlKey,
                ExpireOn = epochSeconds.ToString()
            };
            await context.SaveAsync(record);
        }

        public async Task Update(string shortUrlKey, DateTime expireOn)
        {
            var record = await context.LoadAsync<FromShortUrl>(shortUrlKey);
            int epochSeconds = AWSSDKUtils.ConvertToUnixEpochSeconds(expireOn);
            record.ExpireOn = epochSeconds.ToString();
            await context.SaveAsync(record);
        }
    }
}
