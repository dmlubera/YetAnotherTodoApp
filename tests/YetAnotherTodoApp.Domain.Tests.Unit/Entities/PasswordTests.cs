using FluentAssertions;
using Xunit;
using YetAnotherTodoApp.Domain.Exceptions;
using YetAnotherTodoApp.Domain.ValueObjects;

namespace YetAnotherTodoApp.Domain.Tests.Unit.Entities
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
            var exception = Record.Exception(() => Password.Create(hash, "test"));

            exception.Should().NotBeNull();
            exception.Should().BeOfType<InvalidPasswordHashException>();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Create_WithInvalidSalt_ShouldThrowAnException(string salt)
        {
            var exception = Record.Exception(() => Password.Create("test", salt));

            exception.Should().NotBeNull();
            exception.Should().BeOfType<InvalidPasswordSaltException>();
        }
    }
}
