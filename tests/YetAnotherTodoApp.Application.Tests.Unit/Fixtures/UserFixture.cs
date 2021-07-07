using Bogus;
using System;
using YetAnotherTodoApp.Domain.Entities;
using YetAnotherTodoApp.Domain.ValueObjects;

namespace YetAnotherTodoApp.Application.Tests.Unit.Fixtures
{
    public static class UserFixture
    {
        public static User Create()
            => new Faker<User>()
                .CustomInstantiator(x => Activator.CreateInstance(typeof(User), nonPublic: true) as User)
                .RuleFor(x => x.Name, x => CustomizedNameFaker().Generate())
                .RuleFor(x => x.Username, x => CustomizedUsernameFaker().Generate())
                .RuleFor(x => x.Email, x => CustomizedEmailFaker().Generate())
                .RuleFor(x => x.Password, x => CustomizedPasswordFaker().Generate())
                .Generate();

        private static Faker<Username> CustomizedUsernameFaker()
            => new Faker<Username>()
                .CustomInstantiator(x => Activator.CreateInstance(typeof(Username), nonPublic: true) as Username)
                .RuleFor(x => x.Value, x => x.Person.UserName);

        private static Faker<Email> CustomizedEmailFaker()
            => new Faker<Email>()
                .CustomInstantiator(x => Activator.CreateInstance(typeof(Email), nonPublic: true) as Email)
                .RuleFor(x => x.Value, x => x.Person.Email);

        private static Faker<Name> CustomizedNameFaker()
            => new Faker<Name>()
                .CustomInstantiator(x => Activator.CreateInstance(typeof(Name), nonPublic: true) as Name)
                .RuleFor(x => x.FirstName, x => x.Person.FirstName)
                .RuleFor(x => x.LastName, x => x.Person.LastName);

        private static Faker<Password> CustomizedPasswordFaker()
            => new Faker<Password>()
                .CustomInstantiator(x => Activator.CreateInstance(typeof(Password), nonPublic: true) as Password)
                .RuleFor(x => x.Hash, x => x.Random.Utf16String(32))
                .RuleFor(x => x.Salt, x => x.Random.Utf16String(32));
    }
}