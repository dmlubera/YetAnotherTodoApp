using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Application.Commands.Handlers.TodoLists;
using YetAnotherTodoApp.Application.Commands.Models.TodoLists;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Application.Tests.Unit.Fixtures;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Tests.Unit.Commands.TodoLists
{
    public class UpdateTodoListCommandHandlerTests
    {
        private readonly Mock<ITodoListRepository> _repositoryMock;
        private readonly Mock<ILogger<UpdateTodoListCommandHandler>> _loggerMock;
        private readonly UpdateTodoListCommandHandler _handler;

        public UpdateTodoListCommandHandlerTests()
        {
            _repositoryMock = new Mock<ITodoListRepository>();
            _loggerMock = new Mock<ILogger<UpdateTodoListCommandHandler>>();
            _handler = new UpdateTodoListCommandHandler(_repositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task WhenValidData_ShouldUpdateTitleAndSaveChanges()
        {
            var commandFixture = CreateCommandFixture();
            var todoListFixture = TodoListFixture.Create();
            _repositoryMock.Setup(x => x.CheckIfUserHasGotTodoListWithGivenTitle(It.IsAny<Guid>(), It.IsAny<string>()))
                .ReturnsAsync(false);
            _repositoryMock.Setup(x => x.GetForUserAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(todoListFixture);

            await _handler.HandleAsync(commandFixture);

            todoListFixture.Title.Value.Should().Be(commandFixture.Title);
            _repositoryMock.Verify(x => x.SaveChangesAsync());
        }

        [Fact]
        public async Task WhenTodoListWithGivenNameAlreadyExists_ShouldThrowCustomException()
        {
            var commandFixture = CreateCommandFixture();
            var expectedException = new TodoListWithGivenTitleAlreadyExistsException(commandFixture.Title);
            _repositoryMock.Setup(x => x.CheckIfUserHasGotTodoListWithGivenTitle(It.IsAny<Guid>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            var exception = await Assert.ThrowsAsync<TodoListWithGivenTitleAlreadyExistsException>(() => _handler.HandleAsync(commandFixture));

            exception.Should().NotBeNull();
            exception.Code.Should().Be(expectedException.Code);
            exception.Message.Should().Be(expectedException.Message);
        }


        [Fact]
        public async Task WhenTodoListWithIdDoesNotExists_ShouldThrowCustomException()
        {
            var commandFixture = CreateCommandFixture();
            var expectedException = new TodoListWithGivenIdDoesNotExistException(commandFixture.TodoListId);
            _repositoryMock.Setup(x => x.CheckIfUserHasGotTodoListWithGivenTitle(It.IsAny<Guid>(), It.IsAny<string>()))
                .ReturnsAsync(false);
            _repositoryMock.Setup(x => x.GetForUserAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(() => null);

            var exception = await Assert.ThrowsAsync<TodoListWithGivenIdDoesNotExistException>(() => _handler.HandleAsync(commandFixture));

            exception.Should().NotBeNull();
            exception.Code.Should().Be(expectedException.Code);
            exception.Message.Should().Be(expectedException.Message);
        }

        private UpdateTodoListCommand CreateCommandFixture()
            => new Faker<UpdateTodoListCommand>()
                .CustomInstantiator(x => Activator.CreateInstance(typeof(UpdateTodoListCommand), nonPublic: true) as UpdateTodoListCommand)
                .RuleFor(x => x.UserId, f => f.Random.Guid())
                .RuleFor(x => x.TodoListId, f => f.Random.Guid())
                .RuleFor(x => x.Title, f => f.Random.String())
                .Generate();
            }
}