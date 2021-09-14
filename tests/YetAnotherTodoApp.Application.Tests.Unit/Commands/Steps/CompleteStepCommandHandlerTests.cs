using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Logging;
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
    public class CompleteStepCommandHandlerTests
    {
        private readonly Mock<IStepRepository> _repositoryMock;
        private readonly Mock<ILogger<CompleteStepCommandHandler>> _loggerMock;
        private readonly CompleteStepCommandHandler _handler;

        public CompleteStepCommandHandlerTests()
        {
            _repositoryMock = new Mock<IStepRepository>();
            _loggerMock = new Mock<ILogger<CompleteStepCommandHandler>>();
            _handler = new CompleteStepCommandHandler(_repositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task WhenStepExists_ShouldUpdateStepAndSaveChangesToDatabase()
        {
            var commandFixture = CreateCommandFixture();
            var stepFixture = StepFixture.Create();
            _repositoryMock.Setup(x => x.GetForUserAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(stepFixture);

            await _handler.HandleAsync(commandFixture);

            stepFixture.IsFinished.Should().BeTrue();
            _repositoryMock.Verify(x => x.SaveChangesAsync());
        }

        [Fact]
        public async Task WhenStepDoesNotExist_ShouldThrowCustomException()
        {
            var commandFixture = CreateCommandFixture();
            var expectedException = new StepWithGivenIdDoesNotExistException(commandFixture.StepId);

            var exception = await Assert.ThrowsAsync<StepWithGivenIdDoesNotExistException>(() => _handler.HandleAsync(commandFixture));

            exception.Should().NotBeNull();
            exception.Code.Should().Be(expectedException.Code);
            exception.Message.Should().Be(expectedException.Message);
        }

        private CompleteStepCommand CreateCommandFixture()
            => new Faker<CompleteStepCommand>()
                .CustomInstantiator(x => Activator.CreateInstance(typeof(CompleteStepCommand), nonPublic: true) as CompleteStepCommand)
                .RuleFor(x => x.StepId, f => f.Random.Guid())
                .RuleFor(x => x.UserId, f => f.Random.Guid())
                .Generate();
    }
}