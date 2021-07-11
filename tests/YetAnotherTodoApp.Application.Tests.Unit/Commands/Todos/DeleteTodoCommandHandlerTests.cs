using Bogus;
using FluentAssertions;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Application.Commands.Handlers.Todos;
using YetAnotherTodoApp.Application.Commands.Models.Todos;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Application.Tests.Unit.Fixtures;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Tests.Unit.Commands.Todos
{
    public class DeleteTodoCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _repositoryMock;
        private readonly DeleteTodoCommandHandler _handler;

        public DeleteTodoCommandHandlerTests()
        {
            _repositoryMock = new Mock<IUserRepository>();
            _handler = new DeleteTodoCommandHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task WhenTodoExist_ShouldDeleteTodoAndSaveChanges()
        {
            var todoFixture = TodoFixture.Create();
            var commandFixture = CreateCommandFixture();
            commandFixture.TodoId = todoFixture.Id;
            var userFixture = UserFixture.Create();
            userFixture.TodoLists.FirstOrDefault().AddTodo(todoFixture);
            _repositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(userFixture);

            await _handler.HandleAsync(commandFixture);

            userFixture.TodoLists
                .SelectMany(x => x.Todos)
                .FirstOrDefault(x => x.Id == todoFixture.Id)
                .Should().BeNull();
            _repositoryMock.Verify(x => x.SaveChangesAsync());
        }

        [Fact]
        public async Task WhenTodoDoesNotExist_ShouldThrowCustomException()
        {
            var commandFixture = CreateCommandFixture();
            var userFixture = UserFixture.Create();
            var expectedException = new TodoWithGivenIdDoesNotExistException(commandFixture.TodoId);
            _repositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(userFixture);

            var exception = await Assert.ThrowsAsync<TodoWithGivenIdDoesNotExistException>(async () => await _handler.HandleAsync(commandFixture));

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