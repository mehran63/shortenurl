using FluentAssertions;
using Moq;
using ShortenUrl.BusinessLogic;
using ShortenUrl.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ShortenUrlTests.BusinessLogic.ShortUrlManagerTests
{
    public class ShortUrlManagerHandlesNewUrlSuccessfully : IAsyncLifetime
    {
        private readonly string longUrl = "a long url";
        private readonly string shortUrlKey = "a key";
        private Mock<IToShortUrlRepository> toShortUrlRepositoryMock;
        private Mock<IFromShortUrlRepository> fromShortUrlRepositoryMock;
        private string actulaShortUrlKey;
        private ShortUrlManager sut;

        public ShortUrlManagerHandlesNewUrlSuccessfully()
        {
        }

        public async Task InitializeAsync()
        {
            sut = GivenANewLongUrl();

            actulaShortUrlKey = await WhenShortUrlRequested();
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        public ShortUrlManager GivenANewLongUrl()
        {
            toShortUrlRepositoryMock = new Mock<IToShortUrlRepository>();
            toShortUrlRepositoryMock
                .Setup(m => m.FetchShortUrlKey(longUrl))
                .ReturnsAsync(null as string);

            fromShortUrlRepositoryMock = new Mock<IFromShortUrlRepository>();

            var shortUrlGeneratorMock = new Mock<IShortUrlGenerator>();
            shortUrlGeneratorMock
                .Setup(m => m.GenerateShortUrlKey(longUrl))
                .Returns(shortUrlKey);

            return new ShortUrlManager(
                toShortUrlRepositoryMock.Object,
                fromShortUrlRepositoryMock.Object,
                shortUrlGeneratorMock.Object);
        }

        public async Task<string> WhenShortUrlRequested()
        {
            return await sut.GetShortUrlKey(longUrl);
        }

        [Fact]
        public void ThenTheCreatedShortUrlReturned()
        {
            actulaShortUrlKey.Should().Be(shortUrlKey);
        }

        [Fact]
        public void ThenTheEntryStoredInToShortUrlRepository()
        {
            toShortUrlRepositoryMock
                .Verify(m => m.Store(longUrl, shortUrlKey));
        }

        [Fact]
        public void ThenTheEntryStoredInFromShortUrlRepository()
        {
            fromShortUrlRepositoryMock
                .Verify(m => m.Store(longUrl, shortUrlKey));
        }

    }
}
