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
using YetAnotherTodoApp.Application.Tests.Unit.Fixtures;
using YetAnotherTodoApp.Domain.Entities;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Tests.Unit.Commands.Users
{
    public class UpdateUserInfoCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _repositoryMock;
        private readonly Mock<ILogger<UpdateUserInfoCommandHandler>> _loggerMock;
        private readonly UpdateUserInfoCommandHandler _handler;

        public UpdateUserInfoCommandHandlerTests()
        {
            _repositoryMock = new Mock<IUserRepository>();
            _loggerMock = new Mock<ILogger<UpdateUserInfoCommandHandler>>();
            _handler = new UpdateUserInfoCommandHandler(_repositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task WhenValidData_ThenShouldUpdateUserInfoAndSaveChangesToDatabase()
        {
            var commandFixture = CreateCommandFixture();
            var userFixture = UserFixture.Create();
            _repositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(userFixture);

            await _handler.HandleAsync(commandFixture);

            userFixture.Name.FirstName.Should().Be(commandFixture.FirstName);
            userFixture.Name.LastName.Should().Be(commandFixture.LastName);
            _repositoryMock.Verify(x => x.UpdateAsync(It.IsAny<User>()));
        }

        [Fact]
        public async Task WhenUserNotExist_ShouldThrowCustomExcepion()
        {
            var commandFixture = CreateCommandFixture();
            var expectedException = new UserNotExistException(commandFixture.UserId);
            _repositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(() => null);

            var exception = await Assert.ThrowsAsync<UserNotExistException>(() => _handler.HandleAsync(commandFixture));

            exception.Should().NotBeNull();
            exception.Code.Should().Be(expectedException.Code);
            exception.Message.Should().Be(expectedException.Message);
        }

        private UpdateUserInfoCommand CreateCommandFixture()
            => new Faker<UpdateUserInfoCommand>()
                .CustomInstantiator(x => Activator.CreateInstance(typeof(UpdateUserInfoCommand), nonPublic: true) as UpdateUserInfoCommand)
                .RuleFor(x => x.UserId, f => f.Random.Guid())
                .RuleFor(x => x.FirstName, f => f.Person.FirstName)
                .RuleFor(x => x.LastName, f => f.Person.LastName)
                .Generate();
    }
}