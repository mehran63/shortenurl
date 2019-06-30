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
    public class ToShortUrlRepositoryTests
    {
        [Fact]
        public async Task FetchExistingShortUrlKey()
        {
            //arrange
            var longUrl = "a long url";
            var shortUrlKey = "a short url key";
            var dynamoDBContextMock = new Mock<IDynamoDBContext>();
            dynamoDBContextMock
                .Setup(m => m.LoadAsync<ToShortUrl>(longUrl, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ToShortUrl { ShortUrlKey = shortUrlKey, LongUrl = longUrl });

            var sut = new ToShortUrlRepository(dynamoDBContextMock.Object);

            //act
            var actual = await sut.FetchShortUrlKey(longUrl);

            //assert
            actual.Should().Be(shortUrlKey);
        }

        [Fact]
        public async Task FetchNonExistingLongUrl()
        {
            //arrange
            var longUrl = "a long url";
            var dynamoDBContextMock = new Mock<IDynamoDBContext>();
            dynamoDBContextMock
                .Setup(m => m.LoadAsync<ToShortUrl>(longUrl, It.IsAny<CancellationToken>()))
                .ReturnsAsync(null as ToShortUrl);

            var sut = new ToShortUrlRepository(dynamoDBContextMock.Object);

            //act
            var actual = await sut.FetchShortUrlKey(longUrl);

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

            var sut = new ToShortUrlRepository(dynamoDBContextMock.Object);

            //act
            await sut.Store(longUrl, shortUrlKey);

            //assert
            dynamoDBContextMock
               .Verify(
                   m => m.SaveAsync(
                       It.Is<ToShortUrl>(u => u.ShortUrlKey == shortUrlKey && u.LongUrl == longUrl),
                       It.IsAny<CancellationToken>()),
                   Times.Once); ;
        }
    }
}
