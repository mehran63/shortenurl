using System;
using System.Threading.Tasks;

namespace ShortenUrl.Repository
{
    public interface IToShortUrlRepository
    {
        Task<string> FetchShortUrlKey(string longUrl);

        Task UpdateAsync(string longUrl, DateTime expireOn);

        Task Store(string longUrl,string shortUrlKey, DateTime expireOn);
    }
}