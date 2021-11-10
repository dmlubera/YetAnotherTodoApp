using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Application.Cache;
using YetAnotherTodoApp.Application.Commands.Handlers.TodoTasks;
using YetAnotherTodoApp.Application.Commands.Models.TodoTasks;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Application.Tests.Unit.Fixtures;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Tests.Unit.Commands.TodoTasks
{
    public class AddTodoTaskCommandHandlerTests
    {
        private readonly Mock<ITodoRepository> _repositoryMock;
        private readonly Mock<ICache> _cacheMock;
        private readonly Mock<ILogger<AddTodoTaskCommandHandler>> _loggerMock;
        private readonly AddTodoTaskCommandHandler _handler;

        public AddTodoTaskCommandHandlerTests()
        {
            _repositoryMock = new Mock<ITodoRepository>();
            _cacheMock = new Mock<ICache>();
            _loggerMock = new Mock<ILogger<AddTodoTaskCommandHandler>>();
            _handler = new AddTodoTaskCommandHandler(_repositoryMock.Object, _cacheMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task WithValidData_ShouldAddTaskToTodoAndInvokeSaveChangesAsyncAndSetIdentifierToCache()
        {
            var commandFixture = CreateCommandFixture();
            var todoFixture = TodoFixture.Create();
            _repositoryMock.Setup(x => x.GetForUserAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(todoFixture);

            await _handler.HandleAsync(commandFixture);

            todoFixture.Tasks.Count.Should().BeGreaterThan(0);
            var task = todoFixture.Tasks.FirstOrDefault();
            task.Title.Value.Should().Be(commandFixture.Title);
            task.Description.Should().Be(commandFixture.Description);
            _repositoryMock.Verify(x => x.SaveChangesAsync());
            _cacheMock.Verify(x => x.Set(commandFixture.CacheTokenId.ToString(), It.IsAny<Guid>(), It.IsAny<TimeSpan>()));
        }

        [Fact]
        public async Task WhenSpecifiedTodoDoesNotExist_ShouldThrowCustomException()
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

        private AddTodoTaskCommand CreateCommandFixture()
            => new Faker<AddTodoTaskCommand>()
                .CustomInstantiator(x => Activator.CreateInstance(typeof(AddTodoTaskCommand), nonPublic: true) as AddTodoTaskCommand)
                .RuleFor(x => x.CacheTokenId, f => f.Random.Guid())
                .RuleFor(x => x.UserId, f => f.Random.Guid())
                .RuleFor(x => x.TodoId, f => f.Random.Guid())
                .RuleFor(x => x.Title, f => f.Random.String2(8))
                .RuleFor(x => x.Description, f => f.Random.Words())
                .Generate();
    }
}