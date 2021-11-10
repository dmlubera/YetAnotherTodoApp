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
    public class UpdateTodoTaskCommandHandlerTests
    {
        private readonly Mock<ITodoTaskRepository> _repositoryMock;
        private readonly Mock<ILogger<UpdateTodoTaskCommandHandler>> _loggerMock;
        private readonly UpdateTodoTaskCommandHandler _handler;


        public UpdateTodoTaskCommandHandlerTests()
        {
            _repositoryMock = new Mock<ITodoTaskRepository>();
            _loggerMock = new Mock<ILogger<UpdateTodoTaskCommandHandler>>();
            _handler = new UpdateTodoTaskCommandHandler(_repositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task WhenValidData_ShouldUpdateTodoTaskAndSaveChangesToDatabase()
        {
            var commandFixture = CreateCommandFixture();
            var todoTaskFixture = TodoTaskFixture.Create();
            _repositoryMock.Setup(x => x.GetForUserAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(todoTaskFixture);

            await _handler.HandleAsync(commandFixture);

            todoTaskFixture.Title.Value.Should().Be(commandFixture.Title);
            todoTaskFixture.Description.Should().Be(commandFixture.Description);
            _repositoryMock.Verify(x => x.SaveChangesAsync());
        }

        [Fact]
        public async Task WhenTodoTaskDoesNotExist_ShouldThrowCustomException()
        {
            var commandFixture = CreateCommandFixture();
            var expectedException = new TodoTaskWithGivenIdDoesNotExistException(commandFixture.TaskId);
            _repositoryMock.Setup(x => x.GetForUserAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(() => null);

            var exception = await Assert.ThrowsAsync<TodoTaskWithGivenIdDoesNotExistException>(() => _handler.HandleAsync(commandFixture));

            exception.Should().NotBeNull();
            exception.Code.Should().Be(expectedException.Code);
            exception.Message.Should().Be(expectedException.Message);
        }

        private UpdateTodoTaskCommand CreateCommandFixture()
            => new Faker<UpdateTodoTaskCommand>()
                .CustomInstantiator(x => Activator.CreateInstance(typeof(UpdateTodoTaskCommand), nonPublic: true) as UpdateTodoTaskCommand)
                .RuleFor(x => x.UserId, f => f.Random.Guid())
                .RuleFor(x => x.TaskId, f => f.Random.Guid())
                .RuleFor(x => x.Title, f => f.Random.String2(8))
                .RuleFor(x => x.Description, f => f.Random.Words())
                .Generate();
    }
}