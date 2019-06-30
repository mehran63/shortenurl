using System.Threading.Tasks;

namespace ShortenUrl.BusinessLogic
{
    public interface IShortUrlManager
    {
        Task<string> GetShortUrlKey(string longUrl);

        Task<string> GetLongUrl(string shortUrlKey);
    }
}