using Bogus;
using System;
using YetAnotherTodoApp.Domain.Entities;
using YetAnotherTodoApp.Domain.ValueObjects;

namespace YetAnotherTodoApp.Application.Tests.Unit.Fixtures
{
    public static class TodoListFixture
    {
        public static TodoList Create()
            => new Faker<TodoList>()
                .CustomInstantiator(x => Activator.CreateInstance(typeof(TodoList), nonPublic: true) as TodoList)
                .RuleFor(x => x.Id, f => f.Random.Guid())
                .RuleFor(x => x.Title, f => CustomizedEmailFaker().Generate())
                .Generate();

        private static Faker<Title> CustomizedEmailFaker()
            => new Faker<Title>()
                .CustomInstantiator(x => Activator.CreateInstance(typeof(Title), nonPublic: true) as Title)
                .RuleFor(x => x.Value, x => x.Random.String());
    }
}