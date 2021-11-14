using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Application.Cache;
using YetAnotherTodoApp.Application.Commands.Handlers.TodoLists;
using YetAnotherTodoApp.Application.Commands.Models.TodoLists;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Application.Tests.Unit.Fixtures;
using YetAnotherTodoApp.Domain.Entities;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Tests.Unit.Commands.TodoLists
{
    public class AddTodoListCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _repositoryMock;
        private readonly Mock<ICache> _cacheMock;
        private readonly Mock<ILogger<AddTodoListCommandHandler>> _loggerMock;
        private readonly AddTodoListCommandHandler _handler;

        public AddTodoListCommandHandlerTests()
        {
            _repositoryMock = new Mock<IUserRepository>();
            _cacheMock = new Mock<ICache>();
            _loggerMock = new Mock<ILogger<AddTodoListCommandHandler>>();
            _handler = new AddTodoListCommandHandler(_repositoryMock.Object, _cacheMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task WhenValidData_ShouldAddTodoListAndInvokeUpdateAsyncAndSetIdentifierToCache()
        {
            var commandFixutre = CreateCommandFixture();
            var userFixture = UserFixture.Create();
            _repositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(userFixture);

            await _handler.HandleAsync(commandFixutre);

            userFixture.TodoLists
                .FirstOrDefault(x => x.Title == commandFixutre.Title)
                .Should().NotBeNull();
            _repositoryMock.Verify(x => x.UpdateAsync(It.IsAny<User>()));
            _cacheMock.Verify(x => x.Set(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<TimeSpan>()));
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

        private AddTodoListCommand CreateCommandFixture()
            => new Faker<AddTodoListCommand>()
                .CustomInstantiator(x => Activator.CreateInstance(typeof(AddTodoListCommand), nonPublic: true) as AddTodoListCommand)
                .RuleFor(x => x.Title, f => f.Random.String())
                .RuleFor(x => x.UserId, f => f.Random.Guid())
                .RuleFor(x => x.CacheTokenId, f => f.Random.Guid())
                .Generate();
    }
}