using Bogus;
using System;
using YetAnotherTodoApp.Domain.Entities;
using YetAnotherTodoApp.Domain.ValueObjects;

namespace YetAnotherTodoApp.Application.Tests.Unit.Fixtures
{
    public class TodoTaskFixture
    {
        public static TodoTask Create()
            => new Faker<TodoTask>()
                .CustomInstantiator(x => Activator.CreateInstance(typeof(TodoTask), nonPublic: true) as TodoTask)
                .RuleFor(x => x.Id, f => f.Random.Guid())
                .RuleFor(x => x.Title, f => CustomizedTitleFaker().Generate())
                .Generate();

        private static Faker<Title> CustomizedTitleFaker()
            => new Faker<Title>()
                .CustomInstantiator(x => Activator.CreateInstance(typeof(Title), nonPublic: true) as Title)
                .RuleFor(x => x.Value, x => x.Random.String2(8));
    }
}