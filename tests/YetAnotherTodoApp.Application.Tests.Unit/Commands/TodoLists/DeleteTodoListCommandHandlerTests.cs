using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Application.Commands.Handlers.TodoLists;
using YetAnotherTodoApp.Application.Commands.Models.TodoLists;
using YetAnotherTodoApp.Application.Tests.Unit.Fixtures;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Tests.Unit.Commands.TodoLists
{
    public class DeleteTodoListCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _repositoryMock; 
        private readonly Mock<ILogger<DeleteTodoListCommandHandler>> _loggerMock;
        private readonly DeleteTodoListCommandHandler _handler;

        public DeleteTodoListCommandHandlerTests()
        {
            _repositoryMock = new Mock<IUserRepository>();
            _loggerMock = new Mock<ILogger<DeleteTodoListCommandHandler>>();
            _handler = new DeleteTodoListCommandHandler(_repositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task WhenTodoListExists_ShouldDeleteTodoListAndSaveChanges()
        {
            var userFixture = UserFixture.Create();
            var todoListFixture = TodoListFixture.Create();
            userFixture.AddTodoList(todoListFixture);
            _repositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(userFixture);

            await _handler.HandleAsync(new DeleteTodoListCommand(userFixture.Id, todoListFixture.Id));

            userFixture.TodoLists
                .FirstOrDefault(x => x.Title == todoListFixture.Title)
                .Should().BeNull();
            _repositoryMock.Verify(x => x.SaveChangesAsync());
        }
    }
}