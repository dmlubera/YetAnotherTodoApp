using Bogus;
using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Application.Cache;
using YetAnotherTodoApp.Application.Commands.Handlers.Auths;
using YetAnotherTodoApp.Application.Commands.Models.Auths;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Application.Helpers;
using YetAnotherTodoApp.Domain.Entities;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Tests.Unit.Commands.Auths
{
    public class SignUpCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IEncrypter> _encrypterMock;
        private readonly Mock<ICache> _memoryCacheMock;
        private readonly SignUpCommandHandler _handler;

        public SignUpCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _encrypterMock = new Mock<IEncrypter>();
            _memoryCacheMock = new Mock<ICache>();
            _handler = new SignUpCommandHandler(_userRepositoryMock.Object, _encrypterMock.Object, _memoryCacheMock.Object);
        }

        [Fact]
        public async Task HandleAsync_WhenEmailAlreadyExists_ThenShouldThrownAnException()
        {
            var commandFixture = CreateCommandFixture();
            _userRepositoryMock.Setup(x => x.CheckIfEmailIsInUseAsync(commandFixture.Email))
                .ReturnsAsync(true);

            var exception = await Record.ExceptionAsync(() => _handler.HandleAsync(commandFixture));

            exception.Should().NotBeNull();
            exception.Should().BeOfType<EmailInUseException>();
        }

        [Fact]
        public async Task HandleAsync_WhenUsernameAlreadyExists_ThenShouldThrownAnException()
        {
            var command = CreateCommandFixture();
            _userRepositoryMock.Setup(x => x.CheckIfEmailIsInUseAsync(command.Email))
                .ReturnsAsync(false);
            _userRepositoryMock.Setup(x => x.CheckIfUsernameIsInUseAsync(command.Username))
                .ReturnsAsync(true);

            var exception = await Record.ExceptionAsync(() => _handler.HandleAsync(command));

            exception.Should().NotBeNull();
            exception.Should().BeOfType<UsernameInUseException>();
        }

        [Fact]
        public async Task HandleAsync_WhenGivenValidData_ThenShouldRegisterUser()
        {
            var command = CreateCommandFixture();
            var faker = new Faker();
            var passwordHash = faker.Random.String2(8);
            var passwordSalt = faker.Random.String2(8);
            _userRepositoryMock.Setup(x => x.CheckIfEmailIsInUseAsync(command.Email))
                .ReturnsAsync(false);
            _userRepositoryMock.Setup(x => x.CheckIfUsernameIsInUseAsync(command.Username))
                .ReturnsAsync(false);
            _encrypterMock.Setup(x => x.GetSalt())
                .Returns(passwordSalt);
            _encrypterMock.Setup(x => x.GetHash(command.Password, passwordSalt))
                .Returns(passwordHash);

            await _handler.HandleAsync(command);

            _userRepositoryMock.Verify(x => x.AddAsync(It.Is<User>(x =>
               x.Username.Value == command.Username &&
               x.Email.Value == command.Email &&
               x.Password.Hash == passwordHash &&
               x.Password.Salt == passwordSalt)), Times.Once);
        }

        private SignUpCommand CreateCommandFixture()
            => new Faker<SignUpCommand>()
                .CustomInstantiator(x => Activator.CreateInstance(typeof(SignUpCommand), nonPublic: true) as SignUpCommand)
                .RuleFor(x => x.CacheTokenId, f => f.Random.Guid())
                .RuleFor(x => x.Email, f => f.Internet.Email())
                .RuleFor(x => x.Username, f => f.Random.AlphaNumeric(7))
                .RuleFor(x => x.Password, f => f.Internet.Password())
                .Generate();
    }
}