using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShortenUrl.DataModel
{
    [DynamoDBTable("ToShortUrl")]
    public class ToShortUrl
    {
        [DynamoDBHashKey]
        public string LongUrl { get; set; }

        public string ShortUrlKey { get; set; }

        public string ExpireOn { get; set; }
    }
}
