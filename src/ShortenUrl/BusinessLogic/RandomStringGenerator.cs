using System;
using System.Collections.Generic;
using System.Text;

namespace ShortenUrl.BusinessLogic
{
    public class RandomStringGenerator : IRandomStringGenerator
    {
        public string GetNext()
        {
            return new Random().Next().ToString();
        }
    }
}
