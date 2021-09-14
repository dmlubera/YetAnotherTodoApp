using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Application.Commands.Handlers.Users;
using YetAnotherTodoApp.Application.Commands.Models.Users;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Application.Helpers;
using YetAnotherTodoApp.Application.Tests.Unit.Fixtures;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Tests.Unit.Commands.Users
{
    public class UpdatePasswordCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _repositoryMock;
        private readonly Mock<IEncrypter> _encrypterMock;
        private readonly Mock<ILogger<UpdatePasswordCommandHandler>> _loggerMock;
        private readonly UpdatePasswordCommandHandler _handler;

        public UpdatePasswordCommandHandlerTests()
        {
            _repositoryMock = new Mock<IUserRepository>();
            _encrypterMock = new Mock<IEncrypter>();
            _loggerMock = new Mock<ILogger<UpdatePasswordCommandHandler>>();
            _handler = new UpdatePasswordCommandHandler(_repositoryMock.Object, _encrypterMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task WhenValidData_ThenShouldUpdatePasswordAndSaveChanges()
        {
            var commandFixture = CreateCommandFixture();
            var userFixture = UserFixture.Create();
            var encrypter = new Encrypter();
            var salt = encrypter.GetSalt();
            var hash = encrypter.GetHash(commandFixture.Password, salt);
            _repositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(userFixture);
            _encrypterMock.Setup(x => x.GetHash(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(hash);
            _encrypterMock.Setup(x => x.GetSalt())
                .Returns(salt);

            await _handler.HandleAsync(commandFixture);

            var hashFromGivenPassword = new Encrypter().GetHash(commandFixture.Password, userFixture.Password.Salt);
            userFixture.Password.Hash.Should().Be(hashFromGivenPassword);
            _repositoryMock.Verify(x => x.SaveChangesAsync());
        }

        [Fact]
        public async Task WhenGivenPasswordIsCurrentlySetPassword_ThenShouldThrowCustomException()
        {
            var commandFixture = CreateCommandFixture();
            var userFixture = UserFixture.Create();
            var expectedException = new UpdatePasswordToAlreadyUsedValueException();
            _repositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(userFixture);
            _encrypterMock.Setup(x => x.GetHash(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(userFixture.Password.Hash);

            var exception = await Assert.ThrowsAsync<UpdatePasswordToAlreadyUsedValueException>(() => _handler.HandleAsync(commandFixture));

            exception.Should().NotBeNull();
            exception.Code.Should().Be(expectedException.Code);
            exception.Message.Should().Be(expectedException.Message);
        }

        private UpdatePasswordCommand CreateCommandFixture()
            => new Faker<UpdatePasswordCommand>()
                .CustomInstantiator(x => Activator.CreateInstance(typeof(UpdatePasswordCommand), nonPublic: true) as UpdatePasswordCommand)
                .RuleFor(x => x.UserId, f => f.Random.Guid())
                .RuleFor(x => x.Password, f => f.Internet.Password())
                .Generate();


    }
}