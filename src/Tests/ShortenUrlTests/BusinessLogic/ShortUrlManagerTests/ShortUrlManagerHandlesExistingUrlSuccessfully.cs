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
    public class ShortUrlManagerHandlesExistingUrlSuccessfully : IAsyncLifetime
    {
        private const string longUrl = "a long url";
        private const string shortUrlKey = "a key";
        private readonly ShortUrlManagerSettings shortUrlManagerSettings = new ShortUrlManagerSettings()
        {
            ToShortUrlTtlDays = 1,
            FromShortUrlTtlDays = 7
        };
        private Mock<IToShortUrlRepository> toShortUrlRepositoryMock;
        private Mock<IFromShortUrlRepository> fromShortUrlRepositoryMock;
        private Mock<IShortUrlGenerator> shortUrlGeneratorMock;
        private string actulaShortUrlKey;
        private ShortUrlManager sut;

        public ShortUrlManagerHandlesExistingUrlSuccessfully()
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
                .ReturnsAsync(shortUrlKey);

            fromShortUrlRepositoryMock = new Mock<IFromShortUrlRepository>();

            shortUrlGeneratorMock = new Mock<IShortUrlGenerator>();
            var dateTimeProviderMock = new Mock<IDateTimeProvider>();

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
        public void ThenExistingShortUrlReturned()
        {
            actulaShortUrlKey.Should().Be(shortUrlKey);
        }

        [Fact]
        public void ThenNothingStoredInToShortUrlRepository()
        {
            toShortUrlRepositoryMock.Verify(
                m => m.Store(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()),
                Times.Never);
        }

        [Fact]
        public void ThenNothingStoredInFromShortUrlRepository()
        {
            fromShortUrlRepositoryMock.Verify(
               m => m.Store(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()),
               Times.Never);
        }

        [Fact]
        public void ThenNoNewKeyIsGenerated()
        {
            shortUrlGeneratorMock.Verify(
               m => m.GenerateShortUrlKey(It.IsAny<string>()),
               Times.Never);
        }

    }
}
