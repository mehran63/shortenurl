using System;
using System.Collections.Generic;
using System.Text;

namespace ShortenUrl.BusinessLogic
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.Now;
    }
}
