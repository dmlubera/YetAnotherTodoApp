using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Application.Commands.Handlers.Todos;
using YetAnotherTodoApp.Application.Commands.Models.Todos;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Application.Tests.Unit.Fixtures;
using YetAnotherTodoApp.Domain.Entities;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Tests.Unit.Commands.Todos
{
    public class DeleteTodoCommandHandlerTests
    {
        private readonly Mock<ITodoListRepository> _repositoryMock;
        private readonly Mock<ILogger<DeleteTodoCommandHandler>> _loggerMock;
        private readonly DeleteTodoCommandHandler _handler;

        public DeleteTodoCommandHandlerTests()
        {
            _repositoryMock = new Mock<ITodoListRepository>();
            _loggerMock = new Mock<ILogger<DeleteTodoCommandHandler>>();
            _handler = new DeleteTodoCommandHandler(_repositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task WhenTodoExist_ShouldDeleteTodoAndUpdateTodoList()
        {
            var todoListFixture = TodoListFixture.Create();
            var todoFixture = TodoFixture.Create();
            var commandFixture = CreateCommandFixture();
            commandFixture.TodoId = todoFixture.Id;
            todoListFixture.AddTodo(todoFixture);
            _repositoryMock
                .Setup(x => x.GetByBelongTodo(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(todoListFixture);

            await _handler.HandleAsync(commandFixture);

            todoListFixture.Todos.Should().HaveCount(0);
            _repositoryMock.Verify(x => x.UpdateAsync(It.IsAny<TodoList>()));
        }

        [Fact]
        public async Task WhenTodoDoesNotExist_ShouldThrowCustomException()
        {
            var commandFixture = CreateCommandFixture();
            var expectedException = new TodoWithGivenIdDoesNotExistException(commandFixture.TodoId);
            _repositoryMock.Setup(x => x.GetByBelongTodo(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(() => null);

            var exception = await Assert.ThrowsAsync<TodoWithGivenIdDoesNotExistException>(() => _handler.HandleAsync(commandFixture));

            exception.Should().NotBeNull();
            exception.Code.Should().Be(expectedException.Code);
            exception.Message.Should().Be(expectedException.Message);
        }

        private DeleteTodoCommand CreateCommandFixture()
            => new Faker<DeleteTodoCommand>()
                .CustomInstantiator(x => Activator.CreateInstance(typeof(DeleteTodoCommand), nonPublic: true) as DeleteTodoCommand)
                .RuleFor(x => x.UserId, f => f.Random.Guid())
                .RuleFor(x => x.TodoId, f => f.Random.Guid())
                .Generate();
    }
}