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
using YetAnotherTodoApp.Domain.Enums;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Tests.Unit.Commands.Todos
{
    public class UpdateTodoStatusCommandHandlerTests
    {
        private readonly Mock<ITodoRepository> _repositoryMock;
        private readonly Mock<ILogger<UpdateTodoStatusCommandHandler>> _loggerMock;
        private readonly UpdateTodoStatusCommandHandler _handler;

        public UpdateTodoStatusCommandHandlerTests()
        {
            _repositoryMock = new Mock<ITodoRepository>();
            _loggerMock = new Mock<ILogger<UpdateTodoStatusCommandHandler>>();
            _handler = new UpdateTodoStatusCommandHandler(_repositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task WhenValidData_ThenShouldUpdateStatusAndSaveChangesToDatabase()
        {
            var commandFixture = CreateCommandFixture();
            var todoFixture = TodoFixture.Create();
            _repositoryMock.Setup(x => x.GetForUserAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(todoFixture);

            await _handler.HandleAsync(commandFixture);

            todoFixture.Status.Should().Be(commandFixture.Status);
            _repositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Todo>()));
        }

        [Fact]
        public async Task WhenTodoDoesNotExist_ThenShouldThrowCustomException()
        {
            var commandFixture = CreateCommandFixture();
            var expectedException = new TodoWithGivenIdDoesNotExistException(commandFixture.TodoId);
            _repositoryMock.Setup(x => x.GetForUserAsync(It.IsAny<Guid>(), It.IsAny<Guid>()));

            var exception = await Assert.ThrowsAsync<TodoWithGivenIdDoesNotExistException>(() => _handler.HandleAsync(commandFixture));

            exception.Should().NotBeNull();
            exception.Code.Should().Be(expectedException.Code);
            exception.Message.Should().Be(expectedException.Message);
        }


        private UpdateTodoStatusCommand CreateCommandFixture()
            => new UpdateTodoStatusCommand(Guid.NewGuid(), Guid.NewGuid(), TodoStatus.Done);
    }
}