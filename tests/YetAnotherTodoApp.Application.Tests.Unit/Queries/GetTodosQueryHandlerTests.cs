﻿using AutoFixture;
using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Application.DTOs;
using YetAnotherTodoApp.Application.Queries.Handlers.Todos;
using YetAnotherTodoApp.Application.Queries.Models.Todos;
using YetAnotherTodoApp.Domain.Entities;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Tests.Unit.Queries
{
    public class GetTodosQueryHandlerTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<ITodoRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetTodosQueryHandler _handler;

        public GetTodosQueryHandlerTests()
        {
            _fixture = new Fixture();
            _repositoryMock = new Mock<ITodoRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetTodosQueryHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Test()
        {
            var query = new GetTodosQuery(Guid.NewGuid());
            var user = new User("validUsername", "validEmail@test.com", "secretPassowrd", "salt");
            var todoList = CreateTodoListFixture();
            var todo = new Todo("TestTitle", DateTime.UtcNow.Date);
            todoList.AddTodo(todo);
            user.AddTodoList(todoList);
            _repositoryMock.Setup(x => x.GetAllForUserAsync(query.UserId))
                .ReturnsAsync(todoList.Todos.ToList());
            _mapperMock.Setup(x => x.Map<IEnumerable<TodoDto>>(It.IsAny<List<Todo>>()))
                .Returns(new List<TodoDto>() { new TodoDto { Id = todo.Id, Title = todo.Title.Value, FinishDate = todo.FinishDate.Value } });

            var todos = await _handler.HandleAsync(query);

            _mapperMock.Verify(x => x.Map<IEnumerable<TodoDto>>(todoList.Todos), Times.Once);
        }

        private TodoList CreateTodoListFixture()
            => _fixture.Build<TodoList>().Create();
    }
}
