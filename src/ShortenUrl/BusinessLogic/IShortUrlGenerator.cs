using System.Threading.Tasks;

namespace ShortenUrl.BusinessLogic
{
    public interface IShortUrlGenerator
    {
        Task<string> GenerateShortUrlKey(string longUrl);
    }
}