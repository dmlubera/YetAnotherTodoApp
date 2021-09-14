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
    public class UpdateStepCommandHandlerTests
    {
        private readonly Mock<IStepRepository> _repositoryMock;
        private readonly Mock<ILogger<UpdateStepCommandHandler>> _loggerMock;
        private readonly UpdateStepCommandHandler _handler;
            

        public UpdateStepCommandHandlerTests()
        {
            _repositoryMock = new Mock<IStepRepository>();
            _loggerMock = new Mock<ILogger<UpdateStepCommandHandler>>();
            _handler = new UpdateStepCommandHandler(_repositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task WhenValidData_ShouldUpdateStepAndSaveChangesToDatabase()
        {
            var commandFixture = CreateCommandFixture();
            var stepFixture = StepFixture.Create();
            _repositoryMock.Setup(x => x.GetForUserAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(stepFixture);

            await _handler.HandleAsync(commandFixture);

            stepFixture.Title.Value.Should().Be(commandFixture.Title);
            stepFixture.Description.Should().Be(commandFixture.Description);
            _repositoryMock.Verify(x => x.SaveChangesAsync());
        }

        [Fact]
        public async Task WhenStepDoesNotExist_ShouldThrowCustomException()
        {
            var commandFixture = CreateCommandFixture();
            var expectedException = new StepWithGivenIdDoesNotExistException(commandFixture.StepId);
            _repositoryMock.Setup(x => x.GetForUserAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(() => null);

            var exception = await Assert.ThrowsAsync<StepWithGivenIdDoesNotExistException>(() => _handler.HandleAsync(commandFixture));

            exception.Should().NotBeNull();
            exception.Code.Should().Be(expectedException.Code);
            exception.Message.Should().Be(expectedException.Message);
        }

        private UpdateStepCommand CreateCommandFixture()
            => new Faker<UpdateStepCommand>()
                .CustomInstantiator(x => Activator.CreateInstance(typeof(UpdateStepCommand), nonPublic: true) as UpdateStepCommand)
                .RuleFor(x => x.UserId, f => f.Random.Guid())
                .RuleFor(x => x.StepId, f => f.Random.Guid())
                .RuleFor(x => x.Title, f => f.Random.String2(8))
                .RuleFor(x => x.Description, f => f.Random.Words())
                .Generate();
    }
}