using Bogus;
using FluentAssertions;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Application.Cache;
using YetAnotherTodoApp.Application.Commands.Handlers.Steps;
using YetAnotherTodoApp.Application.Commands.Models.Steps;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Application.Tests.Unit.Fixtures;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Tests.Unit.Commands.Steps
{
    public class AddStepCommandHandlerTests
    {
        private readonly Mock<ITodoRepository> _repositoryMock;
        private readonly Mock<ICache> _cacheMock;
        private readonly AddStepCommandHandler _handler;

        public AddStepCommandHandlerTests()
        {
            _repositoryMock = new Mock<ITodoRepository>();
            _cacheMock = new Mock<ICache>();
            _handler = new AddStepCommandHandler(_repositoryMock.Object, _cacheMock.Object);
        }

        [Fact]
        public async Task WithValidData_ShouldAddStepToTodoAndInvokeSaveChangesAsyncAndSetIdentifierToCache()
        {
            var commandFixture = CreateCommandFixture();
            var todoFixture = TodoFixture.Create();
            _repositoryMock.Setup(x => x.GetForUserAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(todoFixture);

            await _handler.HandleAsync(commandFixture);

            todoFixture.Steps.Count.Should().BeGreaterThan(0);
            var step = todoFixture.Steps.FirstOrDefault();
            step.Title.Value.Should().Be(commandFixture.Title);
            step.Description.Should().Be(commandFixture.Description);
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

        private AddStepCommand CreateCommandFixture()
            => new Faker<AddStepCommand>()
                .CustomInstantiator(x => Activator.CreateInstance(typeof(AddStepCommand), nonPublic: true) as AddStepCommand)
                .RuleFor(x => x.CacheTokenId, f => f.Random.Guid())
                .RuleFor(x => x.UserId, f => f.Random.Guid())
                .RuleFor(x => x.TodoId, f => f.Random.Guid())
                .RuleFor(x => x.Title, f => f.Random.String2(8))
                .RuleFor(x => x.Description, f => f.Random.Words())
                .Generate();
    }
}