using System;
using System.Threading.Tasks;

namespace ShortenUrl.Repository
{
    public interface IFromShortUrlRepository
    {
        Task<string> FetchLongUrl(string shortUrlKey);

        Task Update(string shortUrlKey, DateTime expireOn);

        Task Store(string longUrl, string shortUrlKey, DateTime expireOn);
    }
}