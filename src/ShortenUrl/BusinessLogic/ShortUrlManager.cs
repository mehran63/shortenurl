using ShortenUrl.Repository;
using ShortenUrl.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShortenUrl.BusinessLogic
{
    public class ShortUrlManager : IShortUrlManager
    {
        private readonly IToShortUrlRepository toShortUrlRepository;
        private readonly IFromShortUrlRepository fromShortUrlRepository;
        private readonly IShortUrlGenerator shortUrlGenerator;
        private readonly ShortUrlManagerSettings shortUrlManagerSettings;
        private readonly IDateTimeProvider dateTimeProvider;

        private DateTime toShortUrlExpireOn => dateTimeProvider.Now.AddDays(shortUrlManagerSettings.ToShortUrlTtlDays);
        private DateTime fromShortUrlExpireOn => dateTimeProvider.Now.AddDays(shortUrlManagerSettings.FromShortUrlTtlDays);

        public ShortUrlManager(
            IToShortUrlRepository toShortUrlRepository,
            IFromShortUrlRepository fromShortUrlRepository,
            IShortUrlGenerator shortUrlGenerator,
            ShortUrlManagerSettings shortUrlManagerSettings,
            IDateTimeProvider dateTimeProvider)
        {
            this.toShortUrlRepository = toShortUrlRepository;
            this.fromShortUrlRepository = fromShortUrlRepository;
            this.shortUrlGenerator = shortUrlGenerator;
            this.shortUrlManagerSettings = shortUrlManagerSettings;
            this.dateTimeProvider = dateTimeProvider;
        }

        public async Task<string> GetLongUrl(string shortUrlKey)
        {
            var shortUrl = await fromShortUrlRepository.FetchLongUrl(shortUrlKey);
            await fromShortUrlRepository.Update(shortUrlKey, toShortUrlExpireOn);
            return shortUrl;
        }

        public async Task<string> GetShortUrlKey(string longUrl)
        {
            var shortUrl = await toShortUrlRepository.FetchShortUrlKey(longUrl);

            if (string.IsNullOrEmpty(shortUrl))
            {
                return await Create(longUrl);
            }
            else
            {
                await toShortUrlRepository.UpdateAsync(longUrl, toShortUrlExpireOn);
                return shortUrl;
            }
        }

        private async Task<string> Create(string longUrl)
        {
            var shortUrl = await shortUrlGenerator.GenerateShortUrlKey(longUrl);

            //Technical Dept: use transaction to store these two entries
            var streTask1 = toShortUrlRepository.Store(longUrl, shortUrl, toShortUrlExpireOn);
            var streTask2 = fromShortUrlRepository.Store(longUrl, shortUrl, fromShortUrlExpireOn);

            //Instead of two awaits, Task.WaitAll could be used.
            await streTask1;
            await streTask2;

            return shortUrl;
        }
    }
}
