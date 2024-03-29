﻿using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Application.Cache;
using YetAnotherTodoApp.Application.Commands.Handlers.Auths;
using YetAnotherTodoApp.Application.Commands.Models.Auths;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Application.Helpers;
using YetAnotherTodoApp.Application.Tests.Unit.Fixtures;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Tests.Unit.Commands
{
    public class SignInCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IJwtHelper> _jwtHelperMock;
        private readonly Mock<IEncrypter> _encrypterMock;
        private readonly Mock<ICache> _memoryCacheMock;
        private readonly Mock<ILogger<SignInCommandHandler>> _loggerMock;
        private readonly SignInCommandHandler _handler;

        public SignInCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _jwtHelperMock = new Mock<IJwtHelper>();
            _encrypterMock = new Mock<IEncrypter>();
            _memoryCacheMock = new Mock<ICache>();
            _loggerMock = new Mock<ILogger<SignInCommandHandler>>();
            _handler = new SignInCommandHandler(_userRepositoryMock.Object, _jwtHelperMock.Object,
                _encrypterMock.Object, _memoryCacheMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task HandleAsync_WhenUserWithGivenUsernameDoesNotExist_ThenShouldThrowAnException()
        {
            var commandFixture = CreateCommandFixture();
            _userRepositoryMock.Setup(x => x.GetByEmailAsync(commandFixture.Email))
                .ReturnsAsync(() => null);

            var exception = await Record.ExceptionAsync(() => _handler.HandleAsync(commandFixture));

            exception.Should().NotBeNull();
            exception.Should().BeOfType<InvalidCredentialsException>();
        }

        [Fact]
        public async Task HandleAsync_WhenGivenPasswordIsNotValid_ThenShouldThrowAnException()
        {
            var commandFixture = CreateCommandFixture();
            var user = UserFixture.Create(); 
            _userRepositoryMock.Setup(x => x.GetByEmailAsync(commandFixture.Email))
                .ReturnsAsync(user);
            _encrypterMock.Setup(x => x.GetHash(commandFixture.Password, user.Password.Salt))
                .Returns(new Faker().Random.String2(32));

            var exception = await Record.ExceptionAsync(() => _handler.HandleAsync(commandFixture));

            exception.Should().NotBeNull();
            exception.Should().BeOfType<InvalidCredentialsException>();
        }

        private SignInCommand CreateCommandFixture()
            => new Faker<SignInCommand>()
                .CustomInstantiator(x => Activator.CreateInstance(typeof(SignInCommand), nonPublic: true) as SignInCommand)
                .RuleFor(x => x.Email, x => x.Person.Email)
                .RuleFor(x => x.Password, x => x.Internet.Password(8))
                .Generate();
    }
}