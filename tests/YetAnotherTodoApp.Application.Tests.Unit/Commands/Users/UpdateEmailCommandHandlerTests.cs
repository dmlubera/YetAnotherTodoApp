using Bogus;
using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Application.Commands.Handlers.Users;
using YetAnotherTodoApp.Application.Commands.Models.Users;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Application.Tests.Unit.Fixtures;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Tests.Unit.Commands.Users
{
    public class UpdateEmailCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _repositoryMock;
        private readonly UpdateEmailCommandHandler _handler;

        public UpdateEmailCommandHandlerTests()
        {
            _repositoryMock = new Mock<IUserRepository>();
            _handler = new UpdateEmailCommandHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task WhenValidData_ThenShouldUpdateEmailAndSaveChanges()
        {
            var commandFixture = CreateCommandFixture();
            var userFixture = UserFixture.Create();
            _repositoryMock.Setup(x => x.CheckIfEmailIsInUseAsync(It.IsAny<string>()))
                .ReturnsAsync(false);
            _repositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(userFixture);

            await _handler.HandleAsync(commandFixture);

            userFixture.Email.Value.Should().Be(commandFixture.Email);
            _repositoryMock.Verify(x => x.SaveChangesAsync());
        }

        [Fact]
        public async Task WhenGivenEmailIsInUse_ThenShouldThrowCustomException()
        {
            var userFixture = UserFixture.Create();
            var commandFixture = CreateCommandFixture();
            var expectedException = new EmailInUseException(commandFixture.Email);
            _repositoryMock.Setup(x => x.CheckIfEmailIsInUseAsync(It.IsAny<string>()))
                .ReturnsAsync(true);
            _repositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(userFixture);

            var exception = await Assert.ThrowsAsync<EmailInUseException>(async () => await _handler.HandleAsync(commandFixture));

            exception.Should().NotBeNull();
            exception.Code.Should().Be(expectedException.Code);
            exception.Message.Should().Be(expectedException.Message);
        }

        private UpdateEmailCommand CreateCommandFixture()
            => new Faker<UpdateEmailCommand>()
                .CustomInstantiator(x => Activator.CreateInstance(typeof(UpdateEmailCommand), nonPublic: true) as UpdateEmailCommand)
                .RuleFor(x => x.UserId, f => f.Random.Guid())
                .RuleFor(x => x.Email, f => f.Person.Email)
                .Generate();
    }
}