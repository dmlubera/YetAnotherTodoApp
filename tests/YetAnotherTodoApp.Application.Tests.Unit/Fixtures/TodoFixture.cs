using Bogus;
using System;
using YetAnotherTodoApp.Domain.Entities;
using YetAnotherTodoApp.Domain.ValueObjects;

namespace YetAnotherTodoApp.Application.Tests.Unit.Fixtures
{
    public static class TodoFixture
    {
        public static Todo Create()
            => new Faker<Todo>()
                .CustomInstantiator(x => Activator.CreateInstance(typeof(Todo), nonPublic: true) as Todo)
                .RuleFor(x => x.Id, f => f.Random.Guid())
                .RuleFor(x => x.Title, f => CustomizedTitleFaker().Generate())
                .Generate();

        private static Faker<Title> CustomizedTitleFaker()
            => new Faker<Title>()
                .CustomInstantiator(x => Activator.CreateInstance(typeof(Title), nonPublic: true) as Title)
                .RuleFor(x => x.Value, x => x.Random.String2(8));
    }
}