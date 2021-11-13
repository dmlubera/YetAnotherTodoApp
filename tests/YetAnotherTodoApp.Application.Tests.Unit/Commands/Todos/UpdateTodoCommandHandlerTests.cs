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
    public class UpdateTodoCommandHandlerTests
    {
        private readonly Mock<ITodoRepository> _repositoryMock;
        private readonly Mock<ILogger<UpdateTodoCommandHandler>> _loggerMock;
        private readonly UpdateTodoCommandHandler _handler;

        public UpdateTodoCommandHandlerTests()
        {
            _repositoryMock = new Mock<ITodoRepository>();
            _loggerMock = new Mock<ILogger<UpdateTodoCommandHandler>>();
            _handler = new UpdateTodoCommandHandler(_repositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task WhenValidaData_ThenShouldUpdateTodoAndSaveChangesToDatabase()
        {
            var commandFixture = CreateCommandFixture();
            var todoFixture = TodoFixture.Create();
            _repositoryMock.Setup(x => x.GetForUserAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(todoFixture);

            await _handler.HandleAsync(commandFixture);

            todoFixture.Title.Value.Should().Be(commandFixture.Title);
            todoFixture.FinishDate.Value.Should().Be(commandFixture.FinishDate.Date);
            todoFixture.Description.Should().Be(commandFixture.Description);
            _repositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Todo>()));
        }

        [Fact]
        public async Task WhenTodoDoesNotExist_ThenShouldThrowCustomException()
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

        private UpdateTodoCommand CreateCommandFixture()
            => new Faker<UpdateTodoCommand>()
                .CustomInstantiator(x => Activator.CreateInstance(typeof(UpdateTodoCommand), nonPublic: true) as UpdateTodoCommand)
                .RuleFor(x => x.UserId, f => f.Random.Guid())
                .RuleFor(x => x.TodoId, f => f.Random.Guid())
                .RuleFor(x => x.Title, f => f.Random.String2(8))
                .RuleFor(x => x.Description, f => f.Random.Words(5))
                .RuleFor(x => x.FinishDate, f => f.Date.Future())
                .Generate();
    }
}