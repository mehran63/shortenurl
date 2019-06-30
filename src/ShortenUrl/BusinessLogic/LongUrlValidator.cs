using System;
using System.Collections.Generic;
using System.Text;

namespace ShortenUrl.BusinessLogic
{
    public class LongUrlValidator : ILongUrlValidator
    {
        public bool Validate(string longUrl, out string error)
        {
            error = string.Empty;

            if (string.IsNullOrEmpty(longUrl))
            {
                error = "No URL!";
            }
            else if (longUrl.Length > 3000)
            {
                //TODO: make max length configurable
                error = "Maximum valid URL length is 3000 bytes!";
            }
            else if (!longUrl.StartsWith("http://") && !longUrl.StartsWith("https://"))
            {
                error = "URL should start with http:// or https://!";
            }

            return string.IsNullOrEmpty(error);
        }
    }
}
