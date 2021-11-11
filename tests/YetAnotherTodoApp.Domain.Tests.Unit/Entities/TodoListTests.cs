using FluentAssertions;
using System;
using Xunit;
using YetAnotherTodoApp.Domain.Entities;

namespace YetAnotherTodoApp.Domain.Tests.Unit.Entities
{
    public class TodoListTests
    {
        [Fact]
        public void Create_WhenValidData_ThenShouldCreateTodoListWithGivenTitle()
        {
            var todoList = new TodoList("Title");

            todoList.Should().NotBeNull();
            todoList.Title.Value.Should().Be(todoList.Title);
        }

        [Fact]
        public void UpdateTitle_WhenValidData_ThenShouldUpdateTitle()
        {
            var todoList = CreateTodoList();
            var updatedTitle = "UpdatedTitle";

            todoList.UpdateTitle(updatedTitle);

            todoList.Title.Value.Should().Be(updatedTitle);
        }

        [Fact]
        public void AddTodo_WhenValidData_ThenShouldAddTodo()
        {
            var todoList = CreateTodoList();
            var todo = new Todo("Todo", DateTime.UtcNow.AddDays(1));

            todoList.AddTodo(todo);

            todoList.Todos.Count.Should().Be(1);
        }

        [Fact]
        public void DeleteTodo_WhenValidData_ThenShouldDeleteTodo()
        {
            var todoList = CreateTodoList();
            var todo = new Todo("Todo", DateTime.UtcNow.AddDays(1));
            todoList.AddTodo(todo);

            todoList.DeleteTodo(todo.Id);

            todoList.Todos.Count.Should().Be(0);
        }

        private static TodoList CreateTodoList()
            => new TodoList("Title");
    }
}