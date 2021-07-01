using FluentAssertions;
using Xunit;
using YetAnotherTodoApp.Domain.Exceptions;
using YetAnotherTodoApp.Domain.ValueObjects;

namespace YetAnotherTodoApp.Domain.Tests.Unit.ValueObjects
{
    public class PasswordTests
    {
        [Fact]
        public void Create_WithValidHashAndSalt_ShouldReturnPassword()
        {
            var result = Password.Create("test", "test");

            result.Should().NotBeNull();
            result.Hash.Should().Be("test");
            result.Salt.Should().Be("test");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Create_WithInvalidHash_ShouldThrowAnException(string hash)
        {
            var expectedException = new InvalidPasswordHashException();

            var exception = Assert.Throws<InvalidPasswordHashException>(() => Password.Create(hash, "salt"));

            exception.Code.Should().Be(expectedException.Code);
            exception.Message.Should().Be(expectedException.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Create_WithInvalidSalt_ShouldThrowAnException(string salt)
        {
            var expectedException = new InvalidPasswordSaltException();

            var exception = Assert.Throws<InvalidPasswordSaltException>(() => Password.Create("hash", salt));

            exception.Code.Should().Be(expectedException.Code);
            exception.Message.Should().Be(expectedException.Message);
        }
    }
}