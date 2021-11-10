using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Application.Commands.Handlers.TodoTasks;
using YetAnotherTodoApp.Application.Commands.Models.TodoTasks;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Application.Tests.Unit.Fixtures;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Tests.Unit.Commands.TodoTasks
{
    public class DeleteTodoTaskCommandHandlerTests
    {
        private readonly Mock<ITodoRepository> _repositoryMock;
        private readonly Mock<ILogger<DeleteTodoTaskCommandHandler>> _loggerMock;
        private readonly DeleteTodoTaskCommandHandler _handler;

        public DeleteTodoTaskCommandHandlerTests()
        {
            _repositoryMock = new Mock<ITodoRepository>();
            _loggerMock = new Mock<ILogger<DeleteTodoTaskCommandHandler>>();
            _handler = new DeleteTodoTaskCommandHandler(_repositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task WhenTodoExists_ShouldDeleteTaskAndSaveChangesToDatabase()
        {
            var commandFixture = CreateCommandFixture();
            var todoTaskFixture = TodoTaskFixture.Create();
            var todoFixture = TodoFixture.Create();
            todoFixture.AddTasks(new[] { todoTaskFixture });
            commandFixture.TaskId = todoTaskFixture.Id;

            _repositoryMock.Setup(x => x.GetForUserAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(todoFixture);

            await _handler.HandleAsync(commandFixture);

            todoFixture.Tasks.Count.Should().Be(0);
            _repositoryMock.Verify(x => x.SaveChangesAsync());
        }

        [Fact]
        public async Task WhenTodoDoesNotExist_ShouldThrowCustomException()
        {
            var commandFixture = CreateCommandFixture();
            var expectedException = new TodoWithGivenIdDoesNotExistException(commandFixture.TodoId);
            _repositoryMock.Setup(x => x.GetForUserAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(() => null);

            var exception = await Assert.ThrowsAsync<TodoWithGivenIdDoesNotExistException>(() => _handler.HandleAsync(commandFixture));

            exception.Should().NotBeNull();
            exception.Code.Should().Be(expectedException.Code);
            exception.Message.Should().Be(expectedException.Message);
        }

        private DeleteTodoTaskCommand CreateCommandFixture()
            => new Faker<DeleteTodoTaskCommand>()
                .CustomInstantiator(x => Activator.CreateInstance(typeof(DeleteTodoTaskCommand), nonPublic: true) as DeleteTodoTaskCommand)
                .RuleFor(x => x.UserId, f => f.Random.Guid())
                .RuleFor(x => x.TodoId, f => f.Random.Guid())
                .RuleFor(x => x.TaskId, f => f.Random.Guid())
                .Generate();
    }
}