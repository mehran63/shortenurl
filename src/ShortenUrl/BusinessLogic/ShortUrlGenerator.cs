using System;
using System.Collections.Generic;
using System.Text;

namespace ShortenUrl.BusinessLogic
{
    public class ShortUrlGenerator : IShortUrlGenerator
    {
        public string GenerateShortUrlKey(string longUrl)
        {
            //TODO: huge potential of improvement around this core logic
            return new Random().Next().ToString();
        }
    }
}
