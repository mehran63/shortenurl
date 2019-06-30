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
    public class ToShortUrlRepository : IToShortUrlRepository
    {
        private readonly IDynamoDBContext context;

        public ToShortUrlRepository(IDynamoDBContext dynamoDBContext)
        {
            this.context = dynamoDBContext;
        }

        public async Task<string> FetchShortUrlKey(string longUrl)
        {
            var record = await context.LoadAsync<ToShortUrl>(longUrl);

            return record?.ShortUrlKey;
        }

        public async Task Store(string longUrl, string shortUrlKey, DateTime expireOn)
        {
            int epochSeconds = AWSSDKUtils.ConvertToUnixEpochSeconds(expireOn);
            var record = new ToShortUrl
            {
                LongUrl = longUrl,
                ShortUrlKey = shortUrlKey,
                ExpireOn = epochSeconds.ToString()
            };
            await context.SaveAsync(record);
        }

        public async Task UpdateAsync(string longUrl, DateTime expireOn)
        {
            var record = await context.LoadAsync<ToShortUrl>(longUrl);
            int epochSeconds = AWSSDKUtils.ConvertToUnixEpochSeconds(expireOn);
            record.ExpireOn = epochSeconds.ToString();
            await context.SaveAsync(record);
        }
    }
}
