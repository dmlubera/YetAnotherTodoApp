using FluentAssertions;
using System;
using Xunit;
using YetAnotherTodoApp.Domain.Exceptions;
using YetAnotherTodoApp.Domain.ValueObjects;

namespace YetAnotherTodoApp.Domain.Tests.Unit.ValueObjects
{
    public class FinishDateTests
    {
        [Fact]
        public void Create_WithEarlierDateThanToday_ShouldThrowAnException()
        {
            var date = DateTime.UtcNow.AddDays(-1).Date;
            var expectedException = new DateCannotBeEarlierThanTodayDateException(date);

            var exception = Assert.Throws<DateCannotBeEarlierThanTodayDateException>(() => FinishDate.Create(date));

            exception.Code.Should().Be(expectedException.Code);
            exception.Message.Should().Be(expectedException.Message);
        }
    }
}