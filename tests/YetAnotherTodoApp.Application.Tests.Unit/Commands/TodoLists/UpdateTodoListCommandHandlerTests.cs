using Bogus;
using FluentAssertions;
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
        private readonly UpdateTodoListCommandHandler _handler;

        public UpdateTodoListCommandHandlerTests()
        {
            _repositoryMock = new Mock<ITodoListRepository>();
            _handler = new UpdateTodoListCommandHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task WhenValidData_ShouldUpdateTitleAndSaveChanges()
        {
            var commandFixutre = CreateCommandFixture();
            var todoListFixture = TodoListFixture.Create();
            _repositoryMock.Setup(x => x.CheckIfUserHasGotTodoListWithGivenTitle(It.IsAny<Guid>(), It.IsAny<string>()))
                .ReturnsAsync(false);
            _repositoryMock.Setup(x => x.GetForUserAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(todoListFixture);

            await _handler.HandleAsync(commandFixutre);

            todoListFixture.Title.Value.Should().Be(commandFixutre.Title);
            _repositoryMock.Verify(x => x.SaveChangesAsync());
        }

        [Fact]
        public async Task WhenTodoListWithGivenNameAlreadyExists_ShouldThrowCustomException()
        {
            var commandFixutre = CreateCommandFixture();
            var expectedException = new TodoListWithGivenTitleAlreadyExistsException(commandFixutre.Title);
            _repositoryMock.Setup(x => x.CheckIfUserHasGotTodoListWithGivenTitle(It.IsAny<Guid>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            var exception = await Assert.ThrowsAsync<TodoListWithGivenTitleAlreadyExistsException>(async () => await _handler.HandleAsync(commandFixutre));

            exception.Should().NotBeNull();
            exception.Code.Should().Be(expectedException.Code);
            exception.Message.Should().Be(expectedException.Message);
        }


        [Fact]
        public async Task WhenTodoListWithIdDoesNotExists_ShouldThrowCustomException()
        {
            var commandFixutre = CreateCommandFixture();
            var expectedException = new TodoListWithGivenIdDoesNotExistException(commandFixutre.TodoListId);
            _repositoryMock.Setup(x => x.CheckIfUserHasGotTodoListWithGivenTitle(It.IsAny<Guid>(), It.IsAny<string>()))
                .ReturnsAsync(false);
            _repositoryMock.Setup(x => x.GetForUserAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(() => null);

            var exception = await Assert.ThrowsAsync<TodoListWithGivenIdDoesNotExistException>(async () => await _handler.HandleAsync(commandFixutre));

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