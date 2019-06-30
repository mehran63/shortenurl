using System.Threading.Tasks;

namespace ShortenUrl.Repository
{
    public interface IFromShortUrlRepository
    {
        Task<string> FetchLongUrl(string shortUrlKey);

        Task Store(string longUrl, string shortUrlKey);
    }
}