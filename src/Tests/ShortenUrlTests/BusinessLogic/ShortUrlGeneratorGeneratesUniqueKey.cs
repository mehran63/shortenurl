using FluentAssertions;
using Moq;
using ShortenUrl.BusinessLogic;
using ShortenUrl.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ShortenUrlTests.BusinessLogic
{
    public class ShortUrlGeneratorGeneratesUniqueKey : IAsyncLifetime
    {
        private ShortUrlGenerator sut;
        private string generatedKey;
        private const string secondRandomString = "secondRandomString";

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        public async Task InitializeAsync()
        {
            sut = GivenAnExistingKey();

            generatedKey = await WhenNewKeyGenerated();
        }

        private ShortUrlGenerator GivenAnExistingKey()
        {
            const string firstRandomString = "firstRandomString";

            var fromShortUrlRepositoryMock = new Mock<IFromShortUrlRepository>();
            fromShortUrlRepositoryMock
                .Setup(m => m.FetchLongUrl(firstRandomString))
                .ReturnsAsync("something");
            fromShortUrlRepositoryMock
                .Setup(m => m.FetchLongUrl(secondRandomString))
                .ReturnsAsync(null as string);

            var fandomStringGeneratorMock = new Mock<IRandomStringGenerator>();
            fandomStringGeneratorMock
                .SetupSequence(m => m.GetNext())
                .Returns(firstRandomString)
                .Returns(secondRandomString);

            return new ShortUrlGenerator(
                fromShortUrlRepositoryMock.Object,
                fandomStringGeneratorMock.Object);
        }

        private async Task<string> WhenNewKeyGenerated()
        {
            return await sut.GenerateShortUrlKey("a keylong url");
        }

        [Fact]
        public void ThenTheGeneratedKeyIsUniqe()
        {
            generatedKey.Should().Be(secondRandomString);
        }
    }
}
