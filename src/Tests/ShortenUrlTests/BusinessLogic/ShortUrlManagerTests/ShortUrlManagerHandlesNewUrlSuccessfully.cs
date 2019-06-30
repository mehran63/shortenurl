using FluentAssertions;
using Moq;
using ShortenUrl.BusinessLogic;
using ShortenUrl.Repository;
using ShortenUrl.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ShortenUrlTests.BusinessLogic.ShortUrlManagerTests
{
    public class ShortUrlManagerHandlesNewUrlSuccessfully : IAsyncLifetime
    {
        private const string longUrl = "a long url";
        private const string shortUrlKey = "a key";
        private readonly ShortUrlManagerSettings shortUrlManagerSettings = new ShortUrlManagerSettings
        {
            ToShortUrlTtlDays = 1,
            FromShortUrlTtlDays = 7
        };
        private readonly DateTime now = new DateTime(2019, 7, 1, 8, 13, 10);
        private readonly Mock<IDateTimeProvider> dateTimeProviderMock = new Mock<IDateTimeProvider>();
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
                .ReturnsAsync(shortUrlKey);

            dateTimeProviderMock
                .Setup(m => m.Now)
                .Returns(now);

            return new ShortUrlManager(
                toShortUrlRepositoryMock.Object,
                fromShortUrlRepositoryMock.Object,
                shortUrlGeneratorMock.Object,
                shortUrlManagerSettings,
                dateTimeProviderMock.Object);
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
            var expireOn = now.AddDays(shortUrlManagerSettings.ToShortUrlTtlDays);
            toShortUrlRepositoryMock
                .Verify(m => m.Store(longUrl, shortUrlKey, expireOn));
        }

        [Fact]
        public void ThenTheEntryStoredInFromShortUrlRepository()
        {
            var expireOn = now.AddDays(shortUrlManagerSettings.FromShortUrlTtlDays);
            fromShortUrlRepositoryMock
                .Verify(m => m.Store(longUrl, shortUrlKey, expireOn));
        }

    }
}
