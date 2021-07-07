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
using YetAnotherTodoApp.Domain.ValueObjects;

namespace YetAnotherTodoApp.Application.Tests.Unit.Commands
{
    public class SignInCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IJwtHelper> _jwtHelperMock;
        private readonly Mock<IEncrypter> _encrypterMock;
        private readonly Mock<ICache> _memoryCacheMock;
        private readonly SignInCommandHandler _handler;

        public SignInCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _jwtHelperMock = new Mock<IJwtHelper>();
            _encrypterMock = new Mock<IEncrypter>();
            _memoryCacheMock = new Mock<ICache>();
            _handler = new SignInCommandHandler(_userRepositoryMock.Object, _jwtHelperMock.Object, _encrypterMock.Object, _memoryCacheMock.Object);
        }

        [Fact]
        public async Task HandleAsync_WhenUserWithGivenUsernameDoesNotExist_ThenShouldThrowAnException()
        {
            var command = CustomizedCommandFaker().Generate();
            _userRepositoryMock.Setup(x => x.GetByEmailAsync(command.Email))
                .ReturnsAsync(() => null);

            var exception = await Record.ExceptionAsync(async () => await _handler.HandleAsync(command));

            exception.Should().NotBeNull();
            exception.Should().BeOfType<InvalidCredentialsException>();
        }

        [Fact]
        public async Task HandleAsync_WhenGivenPasswordIsNotValid_ThenShouldThrowAnException()
        {
            var command = CustomizedCommandFaker().Generate();
            var user = CustomizedUserFaker().Generate();
            _userRepositoryMock.Setup(x => x.GetByEmailAsync(command.Email))
                .ReturnsAsync(user);
            _encrypterMock.Setup(x => x.GetHash(command.Password, user.Password.Salt))
                .Returns(CustomizedPasswordFaker().Generate().Hash);

            var exception = await Record.ExceptionAsync(async () => await _handler.HandleAsync(command));

            exception.Should().NotBeNull();
            exception.Should().BeOfType<InvalidCredentialsException>();
        }

        private Faker<SignInCommand> CustomizedCommandFaker()
            => new Faker<SignInCommand>()
                .CustomInstantiator(x => Activator.CreateInstance(typeof(SignInCommand), nonPublic: true) as SignInCommand)
                .RuleFor(x => x.Email, x => x.Person.Email)
                .RuleFor(x => x.Password, x => x.Internet.Password(8));

        private Faker<User> CustomizedUserFaker()
            => new Faker<User>()
                .CustomInstantiator(x => Activator.CreateInstance(typeof(User), nonPublic: true) as User)
                .RuleFor(x => x.Name, x => CustomizedNameFaker().Generate())
                .RuleFor(x => x.Username, x => CustomizedUsernameFaker().Generate())
                .RuleFor(x => x.Email, x => CustomizedEmailFaker().Generate())
                .RuleFor(x => x.Password, x => CustomizedPasswordFaker().Generate());

        private Faker<Username> CustomizedUsernameFaker()
            => new Faker<Username>()
                .CustomInstantiator(x => Activator.CreateInstance(typeof(Username), nonPublic: true) as Username)
                .RuleFor(x => x.Value, x => x.Person.UserName);

        private Faker<Email> CustomizedEmailFaker()
            => new Faker<Email>()
                .CustomInstantiator(x => Activator.CreateInstance(typeof(Email), nonPublic: true) as Email)
                .RuleFor(x => x.Value, x => x.Person.Email);

        private Faker<Name> CustomizedNameFaker()
            => new Faker<Name>()
                .CustomInstantiator(x => Activator.CreateInstance(typeof(Name), nonPublic: true) as Name)
                .RuleFor(x => x.FirstName, x => x.Person.FirstName)
                .RuleFor(x => x.LastName, x => x.Person.LastName);

        private Faker<Password> CustomizedPasswordFaker()
            => new Faker<Password>()
                .CustomInstantiator(x => Activator.CreateInstance(typeof(Password), nonPublic: true) as Password)
                .RuleFor(x => x.Hash, x => x.Random.Utf16String(32))
                .RuleFor(x => x.Salt, x => x.Random.Utf16String(32));
    }
}
