using Bogus;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Application.Cache;
using YetAnotherTodoApp.Application.Commands.Handlers.TodoLists;
using YetAnotherTodoApp.Application.Commands.Models.TodoLists;
using YetAnotherTodoApp.Application.Tests.Unit.Fixtures;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Tests.Unit.Commands.TodoLists
{
    public class AddTodoListCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _repositoryMock;
        private readonly Mock<ICache> _cacheMock;
        private readonly AddTodoListCommandHandler _handler;

        public AddTodoListCommandHandlerTests()
        {
            _repositoryMock = new Mock<IUserRepository>();
            _cacheMock = new Mock<ICache>();
            _handler = new AddTodoListCommandHandler(_repositoryMock.Object, _cacheMock.Object);
        }

        [Fact]
        public async Task WhenValidData_ShouldAddTodoListAndInvokeSaveChangesAsyncAndSetIdentifierToCache()
        {
            var commandFixutre = CreateCommandFixture();
            var userFixture = UserFixture.Create();
            _repositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(userFixture);

            await _handler.HandleAsync(commandFixutre);

            userFixture.TodoLists
                .FirstOrDefault(x => x.Title.Value == commandFixutre.Title)
                .Should().NotBeNull();
            _repositoryMock.Verify(x => x.SaveChangesAsync());
            _cacheMock.Verify(x => x.Set(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<TimeSpan>()));
        }

        private AddTodoListCommand CreateCommandFixture()
            => new Faker<AddTodoListCommand>()
                .CustomInstantiator(x => Activator.CreateInstance(typeof(AddTodoListCommand), nonPublic: true) as AddTodoListCommand)
                .RuleFor(x => x.Title, f => f.Random.String())
                .RuleFor(x => x.UserId, f => f.Random.Guid())
                .RuleFor(x => x.CacheTokenId, f => f.Random.Guid())
                .Generate();
    }
}