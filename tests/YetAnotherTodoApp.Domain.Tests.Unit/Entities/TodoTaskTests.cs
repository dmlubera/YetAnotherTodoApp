using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Domain.Entities;

namespace YetAnotherTodoApp.Domain.Tests.Unit.Entities
{
    public class TodoTaskTests
    {
        [Fact]
        public void Create_WhenValidDataWithoutDescription_ThenShouldCreateTodoTaskWithoutDescription()
        {
            var title = "Title";

            var task = new TodoTask(title);

            task.Should().NotBeNull();
            task.Title.Value.Should().Be(title);
            task.Description.Should().BeNull();
        }

        [Fact]
        public void Create_WhenValidDataWithDescription_ThenShouldCreateTodoTaskWithDescription()
        {
            var title = "Title";
            var description = "Description";

            var task = new TodoTask(title, description);

            task.Should().NotBeNull();
            task.Title.Value.Should().Be(title);
            task.Description.Should().Be(description);
        }

        [Fact]
        public void Update_WhenValidData_ThenShouldUpdateTitleAndDescription()
        {
            var task = CreateTodoTask();
            var updatedTitle = "Updated title";
            var updatedDescription = "Updated description";

            task.Update(updatedTitle, updatedDescription);

            task.Title.Value.Should().Be(updatedTitle);
            task.Description.Should().Be(updatedDescription);
        }

        [Fact]
        public void Complete_WhenValidData_ThenShouldUpdateTaskStatus()
        {
            var task = CreateTodoTask();

            task.Complete();

            task.IsFinished.Should().BeTrue();
        }

        private static TodoTask CreateTodoTask()
            => new TodoTask("Title", "Description");
    }
}