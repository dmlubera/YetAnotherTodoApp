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
    public class CompleteTodoTaskCommandHandlerTests
    {
        private readonly Mock<ITodoTaskRepository> _repositoryMock;
        private readonly Mock<ILogger<CompleteTodoTaskCommandHandler>> _loggerMock;
        private readonly CompleteTodoTaskCommandHandler _handler;

        public CompleteTodoTaskCommandHandlerTests()
        {
            _repositoryMock = new Mock<ITodoTaskRepository>();
            _loggerMock = new Mock<ILogger<CompleteTodoTaskCommandHandler>>();
            _handler = new CompleteTodoTaskCommandHandler(_repositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task WhenTaskExists_ShouldUpdateTaskndSaveChangesToDatabase()
        {
            var commandFixture = CreateCommandFixture();
            var todoTaskFixture = TodoTaskFixture.Create();
            _repositoryMock.Setup(x => x.GetForUserAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(todoTaskFixture);

            await _handler.HandleAsync(commandFixture);

            todoTaskFixture.IsFinished.Should().BeTrue();
            _repositoryMock.Verify(x => x.SaveChangesAsync());
        }

        [Fact]
        public async Task WhenTaskDoesNotExist_ShouldThrowCustomException()
        {
            var commandFixture = CreateCommandFixture();
            var expectedException = new TodoTaskWithGivenIdDoesNotExistException(commandFixture.TaskId);

            var exception = await Assert.ThrowsAsync<TodoTaskWithGivenIdDoesNotExistException>(() => _handler.HandleAsync(commandFixture));

            exception.Should().NotBeNull();
            exception.Code.Should().Be(expectedException.Code);
            exception.Message.Should().Be(expectedException.Message);
        }

        private CompleteTodoTaskCommand CreateCommandFixture()
            => new Faker<CompleteTodoTaskCommand>()
                .CustomInstantiator(x => Activator.CreateInstance(typeof(CompleteTodoTaskCommand), nonPublic: true) as CompleteTodoTaskCommand)
                .RuleFor(x => x.TaskId, f => f.Random.Guid())
                .RuleFor(x => x.UserId, f => f.Random.Guid())
                .Generate();
    }
}