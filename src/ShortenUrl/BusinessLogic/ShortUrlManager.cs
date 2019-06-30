using ShortenUrl.Repository;
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

        public ShortUrlManager(
            IToShortUrlRepository toShortUrlRepository,
            IFromShortUrlRepository fromShortUrlRepository,
            IShortUrlGenerator shortUrlGenerator)
        {
            this.toShortUrlRepository = toShortUrlRepository;
            this.fromShortUrlRepository = fromShortUrlRepository;
            this.shortUrlGenerator = shortUrlGenerator;
        }

        public async Task<string> GetShortUrlKey(string longUrl)
        {
            var shortUrl = await toShortUrlRepository.FetchShortUrlKey(longUrl);

            if (string.IsNullOrEmpty(shortUrl))
            {
                return await Create(longUrl);
            }

            return shortUrl;
        }

        private async Task<string> Create(string longUrl)
        {
            var shortUrl = shortUrlGenerator.GenerateShortUrlKey(longUrl);

            //Technical Dept: use transaction to store these two entries
            var streTask1 = toShortUrlRepository.Store(longUrl, shortUrl);
            var streTask2 = fromShortUrlRepository.Store(longUrl, shortUrl);

            //Instead of two awaits, Task.WaitAll could be used.
            await streTask1;
            await streTask2;

            return shortUrl;
        }
    }
}
