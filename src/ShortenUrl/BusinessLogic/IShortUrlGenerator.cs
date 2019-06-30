namespace ShortenUrl.BusinessLogic
{
    public interface IShortUrlGenerator
    {
        string GenerateShortUrlKey(string longUrl);
    }
}