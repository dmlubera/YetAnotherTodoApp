﻿using AutoFixture;
using FluentAssertions;
using Moq;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Application.Commands.Handlers;
using YetAnotherTodoApp.Application.Commands.Models;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Application.Helpers;
using YetAnotherTodoApp.Domain.Entities;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Tests.Unit.Commands
{
    public class RegisterUserCommandHandlerTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IEncrypter> _encrypterMock;
        private readonly RegisterUserCommandHandler _handler;

        public RegisterUserCommandHandlerTests()
        {
            _fixture = new Fixture();
            _userRepositoryMock = new Mock<IUserRepository>();
            _encrypterMock = new Mock<IEncrypter>();
            _handler = new RegisterUserCommandHandler(_userRepositoryMock.Object, _encrypterMock.Object);
        }

        [Fact]
        public async Task HandleAsync_WhenEmailAlreadyExists_ThenShouldThrownAnException()
        {
            var command = CreateCommandFixture();
            _userRepositoryMock.Setup(x => x.CheckIfEmailIsInUseAsync(command.Email))
                .ReturnsAsync(true);

            var exception = await Record.ExceptionAsync(async () => await _handler.HandleAsync(command));

            exception.Should().NotBeNull();
            exception.Should().BeOfType<EmailInUseException>();
        }

        [Fact]
        public async Task HandleAsync_WhenUsernameAlreadyExists_ThenShouldThrownAnException()
        {
            var command = CreateCommandFixture();
            _userRepositoryMock.Setup(x => x.CheckIfEmailIsInUseAsync(command.Email))
                .ReturnsAsync(false);
            _userRepositoryMock.Setup(x => x.CheckIfUsernameIsInUseAsync(command.Username))
                .ReturnsAsync(true);

            var exception = await Record.ExceptionAsync(async () => await _handler.HandleAsync(command));

            exception.Should().NotBeNull();
            exception.Should().BeOfType<UsernameInUseException>();
        }

        [Fact]
        public async Task HandleAsync_WhenGivenValidData_ThenShouldRegisterUser()
        {
            var command = new RegisterUserCommand("validUsername", "validEmail@test.com", "secretPassword");
            var passwordHash = _fixture.Create<string>();
            var passwordSalt = _fixture.Create<string>();
            _userRepositoryMock.Setup(x => x.CheckIfEmailIsInUseAsync(command.Email))
                .ReturnsAsync(false);
            _userRepositoryMock.Setup(x => x.CheckIfUsernameIsInUseAsync(command.Username))
                .ReturnsAsync(false);
            _encrypterMock.Setup(x => x.GetSalt())
                .Returns(passwordSalt);
            _encrypterMock.Setup(x => x.GetHash(command.Password, passwordSalt))
                .Returns(passwordHash);

            await _handler.HandleAsync(command);

            _userRepositoryMock.Verify(x => x.AddAsync(It.Is<User>(x =>
               x.Username.Value == command.Username &&
               x.Email.Value == command.Email &&
               x.Password.Hash == passwordHash &&
               x.Password.Salt == passwordSalt)), Times.Once);
        }

        private RegisterUserCommand CreateCommandFixture()
            => _fixture.Build<RegisterUserCommand>().Create();
    }
}
