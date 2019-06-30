using FluentAssertions;
using ShortenUrl.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ShortenUrlTests.BusinessLogic
{
    public class LongUrlValidatorTests
    {
        [Theory]
        [InlineData("", false, "No URL!")]
        [InlineData("file://something", false, "URL should start with http:// or https://!")]
        [InlineData("somehting", false, "URL should start with http:// or https://!")]
        [InlineData("https://something", true, "")]
        [InlineData("http://something", true, "")]
        public void TestValidate(string assumedUrl, bool expectedIsValid, string expectedErrorMessage)
        {
            //arrange
            var sut = new LongUrlValidator();

            //act
            string actualError;
            var actualIsValid = sut.Validate(assumedUrl, out actualError);

            //assert
            actualIsValid.Should().Be(expectedIsValid);
            if (expectedIsValid)
            {
                actualError.Should().BeNullOrEmpty();
            }
            else
            {
                actualError.Should().Be(expectedErrorMessage);
            }
        }
    }
}
