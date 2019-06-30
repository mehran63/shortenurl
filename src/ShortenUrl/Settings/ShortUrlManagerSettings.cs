using System;
using System.Collections.Generic;
using System.Text;

namespace ShortenUrl.Settings
{
   public class ShortUrlManagerSettings
    {
        public int FromShortUrlTtlDays { get; set; }
        public int ToShortUrlTtlDays { get; set; }
    }
}
