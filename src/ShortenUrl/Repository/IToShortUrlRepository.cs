using System.Threading.Tasks;

namespace ShortenUrl.Repository
{
    public interface IToShortUrlRepository
    {
        Task<string> FetchShortUrlKey(string longUrl);

        Task Store(string longUrl,string shortUrlKey);
    }
}