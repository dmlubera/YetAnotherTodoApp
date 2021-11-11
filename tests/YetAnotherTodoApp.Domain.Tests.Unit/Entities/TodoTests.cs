using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Domain.Entities;
using YetAnotherTodoApp.Domain.Enums;
using YetAnotherTodoApp.Domain.Exceptions;

namespace YetAnotherTodoApp.Domain.Tests.Unit.Entities
{
    public class TodoTests
    {
        [Fact]
        public void Create_WithoutDescriptionAndPriority_ThenShouldCreateValidTodoWithNormalPriority()
        {
            var title = "Title";
            var finishDate = DateTime.UtcNow.AddDays(1);
            
            var todo = new Todo(title, finishDate);

            todo.Should().NotBeNull();
            todo.Title.Value.Should().Be(title);
            todo.FinishDate.Value.Should().Be(finishDate.Date);
            todo.Description.Should().BeNull();
            todo.Priority.Should().Be(TodoPriority.Normal);
        }

        [Fact]
        public void Create_WithDescriptionAndSpecifiedPriority_ThenShouldCreateValidTodoWithSpecifiedPriority()
        {
            var title = "Title";
            var description = "Description";
            var finishDate = DateTime.UtcNow.AddDays(1);
            var priority = TodoPriority.High;

            var todo = new Todo(title, finishDate, description, priority);

            todo.Should().NotBeNull();
            todo.Title.Value.Should().Be(title);
            todo.FinishDate.Value.Should().Be(finishDate.Date);
            todo.Description.Should().Be(description);
            todo.Priority.Should().Be(priority);
        }

        [Fact]
        public void Update_WithValidData_ThenShouldUpdateTodo()
        {
            var todo = CreateTodo();
            var updatedTitle = "Updated title";
            var updatedDescription = "UpdatedDescription";
            var updatedFinishDate = DateTime.UtcNow.AddDays(7);

            todo.Update(updatedTitle, updatedDescription, updatedFinishDate);

            todo.Title.Value.Should().Be(updatedTitle);
            todo.Description.Should().Be(updatedDescription);
            todo.FinishDate.Value.Should().Be(updatedFinishDate.Date);
        }

        [Fact]
        public void UpdateStatus_WhenStatusToUpdateIsDoneButTodoHasUnfinishedTasks_ThenShouldTrownAnException()
        {
            var todo = CreateWithUnfinishedTask();
            var expectedException = new CannotChangeStatusToDoneOfTodoWithUnfinishedTaskException();

            Assert.Throws<CannotChangeStatusToDoneOfTodoWithUnfinishedTaskException>(() => todo.UpdateStatus(TodoStatus.Done));
        }

        [Theory]
        [InlineData(TodoStatus.Done)]
        [InlineData(TodoStatus.InProgress)]
        public void UpdateStatus_WhenTodoDoesNotHaveAnyTasks_ThenShouldUpdateTodoStatus(TodoStatus status)
        {
            var todo = CreateTodo();

            todo.UpdateStatus(status);

            todo.Status.Should().Be(status);
        }

        [Theory]
        [InlineData(TodoStatus.Done)]
        [InlineData(TodoStatus.InProgress)]
        public void UpdateStatus_WhenTodoHaveFinishedTasks_ThenShouldUpdateTodoStatus(TodoStatus status)
        {
            var todo = CreateWithFinishedTask();

            todo.UpdateStatus(status);

            todo.Status.Should().Be(status);
        }

        [Theory]
        [InlineData(TodoPriority.High)]
        [InlineData(TodoPriority.Normal)]
        [InlineData(TodoPriority.Low)]
        public void UpdatePriority_WhenValid_ThenShouldUpdatePriority(TodoPriority priority)
        {
            var todo = CreateTodo();

            todo.UpdatePriority(priority);

            todo.Priority.Should().Be(priority);
        }

        [Fact]
        public void AddTasks_WhenValid_ThenShouldAddTasks()
        {
            var todo = CreateTodo();
            var tasks = new List<TodoTask>
            {
                new TodoTask("Task1"),
                new TodoTask("Task2")
            };

            todo.AddTasks(tasks);

            todo.Tasks.Count.Should().Be(tasks.Count);
        }

        [Fact]
        public void RemoveTask_WhenValid_ThenShouldRemoveTask()
        {
            var todo = CreateWithUnfinishedTask();

            todo.RemoveTask(todo.Tasks.FirstOrDefault().Id);

            todo.Tasks.Count.Should().Be(0);
        }

        private static Todo CreateTodo()
            => new Todo("Title", DateTime.UtcNow.AddDays(1));

        private static Todo CreateWithUnfinishedTask()
        {
            var todo = CreateTodo();
            var task = new TodoTask("Task");
            todo.AddTasks(new[] { task });

            return todo;
        }

        private static Todo CreateWithFinishedTask()
        {
            var todo = CreateWithUnfinishedTask();
            todo.Tasks.FirstOrDefault()?.Complete();

            return todo;
        }
    }
}
