using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShortenUrl.DataModel
{
    [DynamoDBTable("FromShortUrl")]
    public class FromShortUrl
    {
        [DynamoDBHashKey]
        public string ShortUrlKey { get; set; }

        public string LongUrl { get; set; }

    }
}
