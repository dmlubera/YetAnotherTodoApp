using FluentAssertions;
using Xunit;
using YetAnotherTodoApp.Domain.Exceptions;
using YetAnotherTodoApp.Domain.ValueObjects;

namespace YetAnotherTodoApp.Domain.Tests.Unit.Entities
{
    public class TitleTests
    {
        [Fact]
        public void Create_WithValidValue_ShouldReturnTitle()
        {
            var result = Title.Create("test");

            result.Should().NotBeNull();
            result.Value.Should().Be("test");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Create_WithInvalidValue_ShouldThrowAnException(string title)
        {
            var expectedException = new InvalidTitleException(title);

            var exception = Assert.Throws<InvalidTitleException>(() => Title.Create(title));

            exception.Code.Should().Be(expectedException.Code);
            exception.Message.Should().Be(expectedException.Message);
        }
    }
}