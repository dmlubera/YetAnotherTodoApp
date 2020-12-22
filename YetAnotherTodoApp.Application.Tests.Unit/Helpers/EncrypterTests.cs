using FluentAssertions;
using System;
using Xunit;
using YetAnotherTodoApp.Application.Helpers;

namespace YetAnotherTodoApp.Application.Tests.Unit.Helpers
{
    public class EncrypterTests
    {
        private readonly IEncrypter _encrypter = new Encrypter();

        [Fact]
        public void GetHash_WhenValidValueAndSalt_ThenReturnHash()
        {
            var result = _encrypter.GetHash("test", _encrypter.GetSalt());

            result.Should().NotBeNullOrEmpty(result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void GetHash_WhenInvalidValue_ThenThrowAnException(string password)
        {
            var exception = Record.Exception(() => _encrypter.GetHash(password, "test"));

            exception.Should().NotBeNull();
            exception.Should().BeOfType<ArgumentException>();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void GetHash_WhenInvalidSalt_ThenThrowAnException(string salt)
        {
            var exception = Record.Exception(() => _encrypter.GetHash("test", salt));

            exception.Should().NotBeNull();
            exception.Should().BeOfType<ArgumentException>();
        }
    }
}
