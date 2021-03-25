using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Application.Commands.Handlers;
using YetAnotherTodoApp.Application.Commands.Models;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Application.Helpers;
using YetAnotherTodoApp.Domain.Entities;
using YetAnotherTodoApp.Domain.Repositories;
using YetAnotherTodoApp.Domain.ValueObjects;

namespace YetAnotherTodoApp.Application.Tests.Unit.Commands
{
    public class SignInCommandHandlerTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IJwtHelper> _jwtHelperMock;
        private readonly Mock<IEncrypter> _encrypterMock;
        private readonly Mock<IMemoryCache> _memoryCacheMock;
        private readonly SignInCommandHandler _handler;

        public SignInCommandHandlerTests()
        {
            _fixture = new Fixture();
            _userRepositoryMock = new Mock<IUserRepository>();
            _jwtHelperMock = new Mock<IJwtHelper>();
            _encrypterMock = new Mock<IEncrypter>();
            _memoryCacheMock = new Mock<IMemoryCache>();
            _handler = new SignInCommandHandler(_userRepositoryMock.Object, _jwtHelperMock.Object, _encrypterMock.Object, _memoryCacheMock.Object);
        }

        [Fact]
        public async Task HandleAsync_WhenUserWithGivenUsernameDoesNotExist_ThenShouldThrowAnException()
        {
            var command = CreateCommandFixture();
            _userRepositoryMock.Setup(x => x.GetByEmailAsync(command.Email))
                .ReturnsAsync(() => null);

            var exception = await Record.ExceptionAsync(async () => await _handler.HandleAsync(command));

            exception.Should().NotBeNull();
            exception.Should().BeOfType<InvalidCredentialsException>();
        }

        private SignInCommand CreateCommandFixture()
            => _fixture.Build<SignInCommand>().Create();
    }
}
