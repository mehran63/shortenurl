using ShortenUrl.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShortenUrl.BusinessLogic
{
    public class ShortUrlGenerator : IShortUrlGenerator
    {
        private readonly IFromShortUrlRepository fromShortUrlRepository;
        private readonly IRandomStringGenerator randomStringGenerator;

        public ShortUrlGenerator(
            IFromShortUrlRepository fromShortUrlRepository,
            IRandomStringGenerator randomStringGenerator)
        {
            this.fromShortUrlRepository = fromShortUrlRepository;
            this.randomStringGenerator = randomStringGenerator;
        }

        public async Task<string> GenerateShortUrlKey(string longUrl)
        {
            //TODO: considerable potential of improvement around this core logic

            string newKey;
            bool keyFound;
            do
            {
                newKey = randomStringGenerator.GetNext();
                var foundLongUrl = await fromShortUrlRepository.FetchLongUrl(newKey);
                keyFound = !string.IsNullOrEmpty(foundLongUrl);
            } while (keyFound);

            return newKey;
        }
    }
}
