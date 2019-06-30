using Amazon.DynamoDBv2.DataModel;
using FluentAssertions;
using Moq;
using ShortenUrl.DataModel;
using ShortenUrl.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ShortenUrlTests.Repository
{
    public class FromShortUrlRepositoryTests
    {
        [Fact]
        public async Task FetchExistingLongUrl()
        {
            //arrange
            var longUrl = "a long url";
            var shortUrlKey = "a short url key";
            var dynamoDBContextMock = new Mock<IDynamoDBContext>();
            dynamoDBContextMock
                .Setup(m => m.LoadAsync<FromShortUrl>(shortUrlKey, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FromShortUrl { ShortUrlKey = shortUrlKey, LongUrl = longUrl });

            var sut = new FromShortUrlRepository(dynamoDBContextMock.Object);

            //act
            var actual = await sut.FetchLongUrl(shortUrlKey);

            //assert
            actual.Should().Be(longUrl);
        }

        [Fact]
        public async Task FetchNonExistingLongUrl()
        {
            //arrange
            var shortUrlKey = "a short url key";
            var dynamoDBContextMock = new Mock<IDynamoDBContext>();
            dynamoDBContextMock
                .Setup(m => m.LoadAsync<FromShortUrl>(shortUrlKey, It.IsAny<CancellationToken>()))
                .ReturnsAsync(null as FromShortUrl);

            var sut = new FromShortUrlRepository(dynamoDBContextMock.Object);

            //act
            var actual = await sut.FetchLongUrl(shortUrlKey);

            //assert
            actual.Should().BeNull();
        }

        [Fact]
        public async Task StoreAnEntry()
        {
            //arrange
            var longUrl = "a long url";
            var shortUrlKey = "a short url key";
            var dynamoDBContextMock = new Mock<IDynamoDBContext>();

            var sut = new FromShortUrlRepository(dynamoDBContextMock.Object);

            //act
            await sut.Store(longUrl, shortUrlKey);

            //assert
            dynamoDBContextMock
               .Verify(
                   m => m.SaveAsync(
                       It.Is<FromShortUrl>(u => u.ShortUrlKey == shortUrlKey && u.LongUrl == longUrl),
                       It.IsAny<CancellationToken>()),
                   Times.Once); ;
        }
    }
}
