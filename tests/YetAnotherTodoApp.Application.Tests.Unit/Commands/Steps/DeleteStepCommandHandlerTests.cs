using Bogus;
using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Application.Commands.Handlers.Steps;
using YetAnotherTodoApp.Application.Commands.Models.Steps;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Application.Tests.Unit.Fixtures;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Tests.Unit.Commands.Steps
{
    public class DeleteStepCommandHandlerTests
    {
        private readonly Mock<ITodoRepository> _repositoryMock;
        private readonly DeleteStepCommandHandler _handler;

        public DeleteStepCommandHandlerTests()
        {
            _repositoryMock = new Mock<ITodoRepository>();
            _handler = new DeleteStepCommandHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task WhenTodoExists_ShouldDeleteStepAndSaveChangesToDatabase()
        {
            var commandFixture = CreateCommandFixture();
            var stepFixture = StepFixture.Create();
            var todoFixture = TodoFixture.Create();
            todoFixture.AddSteps(new[] { stepFixture });
            commandFixture.StepId = stepFixture.Id;

            _repositoryMock.Setup(x => x.GetForUserAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(todoFixture);

            await _handler.HandleAsync(commandFixture);

            todoFixture.Steps.Count.Should().Be(0);
            _repositoryMock.Verify(x => x.SaveChangesAsync());
        }

        [Fact]
        public async Task WhenTodoDoesNotExist_ShouldThrowCustomException()
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

        private DeleteStepCommand CreateCommandFixture()
            => new Faker<DeleteStepCommand>()
                .CustomInstantiator(x => Activator.CreateInstance(typeof(DeleteStepCommand), nonPublic: true) as DeleteStepCommand)
                .RuleFor(x => x.UserId, f => f.Random.Guid())
                .RuleFor(x => x.TodoId, f => f.Random.Guid())
                .RuleFor(x => x.StepId, f => f.Random.Guid())
                .Generate();
    }
}