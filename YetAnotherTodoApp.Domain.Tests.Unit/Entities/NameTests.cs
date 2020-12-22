using FluentAssertions;
using Xunit;
using YetAnotherTodoApp.Domain.Exceptions;
using YetAnotherTodoApp.Domain.ValueObjects;

namespace YetAnotherTodoApp.Domain.Tests.Unit.Entities
{
    public class NameTests
    {

        [Theory]
        [InlineData("test", "test")]
        [InlineData("te", "test")]
        [InlineData("test", "te")]

        public void CreateName_WithValidFirstNameAndLastName_ShouldReturnName(string firstName, string lastName)
        {
            var result = Name.Create(firstName, lastName);

            result.Should().NotBeNull();
            result.FirstName.Should().Be(firstName);
            result.LastName.Should().Be(lastName);
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("t")]
        [InlineData("_test")]
        [InlineData("te$st")]
        [InlineData("1test")]
        public void Create_WithInvalidFirstName_ShouldThrowAnException(string firstName)
        {
            var exception = Record.Exception(() => Name.Create(firstName, "test"));

            exception.Should().NotBeNull();
            exception.Should().BeOfType<InvalidFirstNameException>();
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("t")]
        [InlineData("_test")]
        [InlineData("te$st")]
        [InlineData("1test")]
        public void Create_WithInvalidLastName_ShouldThrowAnException(string lastName)
        {
            var exception = Record.Exception(() => Name.Create("test", lastName));

            exception.Should().NotBeNull();
            exception.Should().BeOfType<InvalidLastNameException>();
        }
    }
}
