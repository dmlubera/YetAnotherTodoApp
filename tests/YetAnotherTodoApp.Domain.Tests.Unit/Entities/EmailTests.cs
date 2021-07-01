using FluentAssertions;
using Xunit;
using YetAnotherTodoApp.Domain.Exceptions;
using YetAnotherTodoApp.Domain.ValueObjects;

namespace YetAnotherTodoApp.Domain.Tests.Unit.Entities
{
    public class EmailTests
    {
        [Theory]
        [InlineData("test@test.com")]
        [InlineData("test.test@test.com")]
        [InlineData("123test.test@test.com")]
        [InlineData("test.test@test.test.com")]
        [InlineData("test.test@test-test.com")]
        [InlineData("test_test@test.com")]
        public void Create_WithValidValue_ShouldReturnAnEmail(string email)
        {
            var result = Email.Create(email);

            result.Should().NotBeNull();
            result.Value.Should().Be(email);
        }

        [Theory]
        [InlineData("")]
        [InlineData("     ")]
        [InlineData("test")]
        [InlineData("test @test.com")]
        [InlineData("test.@test.com")]
        [InlineData("  @test.com")]
        [InlineData("*^&@test.com")]
        [InlineData("12test@")]
        [InlineData("12test@test")]
        [InlineData("test@test.")]
        [InlineData("test@test..com")]
        [InlineData("test@ test. .com")]
        [InlineData(".test@test.com")]
        [InlineData("test-test@test.com")]
        [InlineData("test-.test@test.com")]
        [InlineData("test..test@test.com")]
        [InlineData("test$test@test.com")]
        [InlineData("test$test@$test.com")]
        public void Create_WithInvalidValue_ShouldThrowAnException(string email)
        {
            var expectedException = new InvalidEmailFormatException(email);

            var exception = Assert.Throws<InvalidEmailFormatException>(() => Email.Create(email));

            exception.Code.Should().Be(expectedException.Code);
            exception.Message.Should().Be(expectedException.Message);
        }
    }
}