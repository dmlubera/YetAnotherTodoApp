using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Application.Commands.Handlers.Todos;
using YetAnotherTodoApp.Application.Commands.Models.Todos;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Application.Tests.Unit.Fixtures;
using YetAnotherTodoApp.Domain.Enums;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Tests.Unit.Commands.Todos
{
    public class UpdateTodoPriorityCommandHandlerTests
    {
        private readonly Mock<ITodoRepository> _repositoryMock;
        private readonly UpdateTodoPriorityCommandHandler _handler;

        public UpdateTodoPriorityCommandHandlerTests()
        {
            _repositoryMock = new Mock<ITodoRepository>();
            _handler = new UpdateTodoPriorityCommandHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task WhenValidData_ThenShouldUpdatePriorityAndSaveChanges()
        {
            var commandFixture = CreateCommandFixture();
            var todoFixture = TodoFixture.Create();
            _repositoryMock.Setup(x => x.GetForUserAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(todoFixture);

            await _handler.HandleAsync(commandFixture);

            todoFixture.Priority.Should().Be(commandFixture.Priority);
            _repositoryMock.Verify(x => x.SaveChangesAsync());
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


        private UpdateTodoPriorityCommand CreateCommandFixture()
            => new UpdateTodoPriorityCommand(Guid.NewGuid(), Guid.NewGuid(), TodoPriority.High);
    }
}