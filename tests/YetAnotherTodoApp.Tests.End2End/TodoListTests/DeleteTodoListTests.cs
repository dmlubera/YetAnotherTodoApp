﻿using FluentAssertions;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Api.Models.Errors;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Domain.Entities;
using YetAnotherTodoApp.Tests.End2End.Helpers;

namespace YetAnotherTodoApp.Tests.End2End.TodoListTests
{
    public class DeleteTodoListTests : IntegrationTestBase
    {
        private async Task<HttpResponseMessage> ActAsync(Guid id)
            => await TestClient.DeleteAsync($"/api/todolist/{id}");

        [Fact]
        public async Task WithExistingId_ShouldReturnNoContentAndRemoveResourceFromDatabase()
        {
            var todoListToRemove = User.TodoLists.FirstOrDefault(x => x.Title.Value == TestDbConsts.TestTodoList);

            var httpResponse = await HandleRequestAsync(() => ActAsync(todoListToRemove.Id));

            httpResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var todoList = await DbContext.GetAsync<TodoList>(todoListToRemove.Id);
            todoList.Should().BeNull();
        }

        [Fact]
        public async Task WithExistingIdOfTodoListContainingTodoWithAssignedSteps_ShouldReturnNoContentAndCascadeRemoveResourcesFromDatabase()
        {
            var todoListToRemove = User.TodoLists.FirstOrDefault(x => x.Title.Value == TestDbConsts.TestTodoListWithAssignedTodo);
            
            var httpResponse = await HandleRequestAsync(() => ActAsync(todoListToRemove.Id));

            httpResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
            var todoList = await DbContext.GetAsync<TodoList>(todoListToRemove.Id);
            var todo = await DbContext.GetAsync<Todo>(todoListToRemove.Todos.FirstOrDefault().Id);

            todoList.Should().BeNull();
            todo.Should().BeNull();
        }

        [Fact]
        public async Task WithNotExistingId_ShouldReturnBadRequestWithCustomError()
        {
            var id = Guid.NewGuid();
            var expectedException = new TodoListWithGivenIdDoesNotExistException(id);

            (var httpResponse, var errorResponse) =
                await HandleRequestAsync<ErrorResponse>(() => ActAsync(id));

            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Code.Should().Be(expectedException.Code);
            errorResponse.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public async Task OnInbox_ShouldReturnBadRequestWithCustomError()
        {
            var inbox = User.TodoLists.FirstOrDefault(x => x.Title.Value == "Inbox");
            var expectedException = new InboxDeletionIsNotAllowedException();

            (var httpResponse, var errorResponse) =
                await HandleRequestAsync<ErrorResponse>(() => ActAsync(inbox.Id));

            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errorResponse.Code.Should().Be(expectedException.Code);
            errorResponse.Message.Should().Be(expectedException.Message);
        }
    }
}