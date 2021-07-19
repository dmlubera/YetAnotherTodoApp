using Bogus;
using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Application.Commands.Handlers.Users;
using YetAnotherTodoApp.Application.Commands.Models.Users;
using YetAnotherTodoApp.Application.Tests.Unit.Fixtures;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Tests.Unit.Commands.Users
{
    public class UpdateUserInfoCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _repositoryMock;
        private readonly UpdateUserInfoCommandHandler _handler;

        public UpdateUserInfoCommandHandlerTests()
        {
            _repositoryMock = new Mock<IUserRepository>();
            _handler = new UpdateUserInfoCommandHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task WhenValidData_ThenShouldUpdateUserInfoAndSaveChanges()
        {
            var commandFixture = CreateCommandFixture();
            var userFixture = UserFixture.Create();
            _repositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(userFixture);

            await _handler.HandleAsync(commandFixture);

            userFixture.Name.FirstName.Should().Be(commandFixture.FirstName);
            userFixture.Name.LastName.Should().Be(commandFixture.LastName);
            _repositoryMock.Verify(x => x.SaveChangesAsync());
        }

        private UpdateUserInfoCommand CreateCommandFixture()
            => new Faker<UpdateUserInfoCommand>()
                .CustomInstantiator(x => Activator.CreateInstance(typeof(UpdateUserInfoCommand), nonPublic: true) as UpdateUserInfoCommand)
                .RuleFor(x => x.UserId, f => f.Random.Guid())
                .RuleFor(x => x.FirstName, f => f.Person.FirstName)
                .RuleFor(x => x.LastName, f => f.Person.LastName)
                .Generate();
    }
}