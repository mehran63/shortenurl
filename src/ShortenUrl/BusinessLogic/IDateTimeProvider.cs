using System;

namespace ShortenUrl.BusinessLogic
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }
}