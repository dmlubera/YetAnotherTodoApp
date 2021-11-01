using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Application.Cache;
using YetAnotherTodoApp.Application.Commands.Handlers.Todos;
using YetAnotherTodoApp.Application.Commands.Models.Todos;
using YetAnotherTodoApp.Application.Tests.Unit.Fixtures;
using YetAnotherTodoApp.Domain.Entities;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Tests.Unit.Commands.Todos
{
    public class AddTodoCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _repositoryMock;
        private readonly Mock<ICache> _cacheMock;
        private readonly Mock<ILogger<AddTodoCommandHandler>> _loggerMock;
        private readonly AddTodoCommandHandler _handler;

        public AddTodoCommandHandlerTests()
        {
            _repositoryMock = new Mock<IUserRepository>();
            _cacheMock = new Mock<ICache>();
            _loggerMock = new Mock<ILogger<AddTodoCommandHandler>>();
            _handler = new AddTodoCommandHandler(_repositoryMock.Object, _cacheMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task WithoutSpecifiedProject_ShouldAddTodoToInboxAndInvokeSaveChangesAsyncAndSetIdentifierToCache()
        {
            var commandFixture = CreateCommandFixture();
            var userFixture = UserFixture.Create();
            _repositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(userFixture);

            await _handler.HandleAsync(commandFixture);

            userFixture.TodoLists
                .FirstOrDefault()
                .Todos
                .FirstOrDefault(x => x.Title == commandFixture.Title)
                .Should().NotBeNull();
            _cacheMock.Verify(x => x.Set(commandFixture.CacheTokenId.ToString(), It.IsAny<Guid>(), It.IsAny<TimeSpan>()));
        }

        [Fact]
        public async Task WithSpecifiedProjectWhichDoesNotExistYet_ShouldCreateTodoListThenAddTodoAndInvokeSaveChangesAsyncAndSetIdentifierToCache()
        {
            var commandFixture = CreateCommandFixture();
            commandFixture.Project = "Test";
            var userFixture = UserFixture.Create();
            _repositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(userFixture);

            await _handler.HandleAsync(commandFixture);

            userFixture.TodoLists.Count.Should().Be(2);
            userFixture.TodoLists
                .FirstOrDefault(x => x.Title == commandFixture.Project)
                .Todos
                .FirstOrDefault(x => x.Title == commandFixture.Title)
                .Should().NotBeNull();
            _cacheMock.Verify(x => x.Set(commandFixture.CacheTokenId.ToString(), It.IsAny<Guid>(), It.IsAny<TimeSpan>()));
        }

        [Fact]
        public async Task WithSpecifiedProjectWhichExistYet_ShouldAddTodoAndInvokeSaveChangesAsyncAndSetIdentifierToCache()
        {
            var commandFixture = CreateCommandFixture();
            var todoList = new TodoList("Test");
            commandFixture.Project = todoList.Title;
            var userFixture = UserFixture.Create();
            userFixture.AddTodoList(todoList);
            _repositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(userFixture);

            await _handler.HandleAsync(commandFixture);

            userFixture.TodoLists.Count.Should().Be(2);
            userFixture.TodoLists
                .FirstOrDefault(x => x.Title == commandFixture.Project)
                .Todos
                .FirstOrDefault(x => x.Title == commandFixture.Title)
                .Should().NotBeNull();
            _cacheMock.Verify(x => x.Set(commandFixture.CacheTokenId.ToString(), It.IsAny<Guid>(), It.IsAny<TimeSpan>()));
        }

        private AddTodoCommand CreateCommandFixture()
            => new Faker<AddTodoCommand>()
                .CustomInstantiator(x => Activator.CreateInstance(typeof(AddTodoCommand), nonPublic: true) as AddTodoCommand)
                .RuleFor(x => x.UserId, f => f.Random.Guid())
                .RuleFor(x => x.CacheTokenId, f => f.Random.Guid())
                .RuleFor(x => x.Title, f => f.Random.String2(8))
                .RuleFor(x => x.FinishDate, f => f.Date.Future())
                .Generate();
    }
}