﻿using FluentAssertions;
using Xunit;
using YetAnotherTodoApp.Domain.Exceptions;
using YetAnotherTodoApp.Domain.ValueObjects;

namespace YetAnotherTodoApp.Domain.Tests.Unit.Entities
{
    public class UsernameTests
    {
        [Theory]
        [InlineData("testusername")]
        [InlineData("1test23")]
        [InlineData("test_test")]
        [InlineData("test_test_test")]
        public void Create_WithValidUsername_ShouldReturnUsername(string username)
        {
            var result = Username.Create(username);

            result.Should().NotBeNull();
            result.Value.Should().Be(username);
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("test")]
        [InlineData("_test")]
        [InlineData(".test")]
        [InlineData("test.")]
        [InlineData("test_")]
        [InlineData("test.test")]
        [InlineData("test$test")]
        public void Create_WithInvalidUsername_ShouldThrowAnException(string username)
        {
            var exception = Record.Exception(() => Username.Create(username));

            exception.Should().NotBeNull();
            exception.Should().BeOfType<InvalidUsernameException>();
        }
    }
}
