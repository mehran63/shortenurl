namespace ShortenUrl.BusinessLogic
{
    public interface ILongUrlValidator
    {
        bool Validate(string longUrl, out string error);
    }
}