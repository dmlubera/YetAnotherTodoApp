using AutoFixture;
using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using YetAnotherTodoApp.Application.DTOs;
using YetAnotherTodoApp.Application.Queries.Handlers;
using YetAnotherTodoApp.Application.Queries.Models;
using YetAnotherTodoApp.Domain.Entities;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Tests.Unit.Queries
{
    public class GetTodosQueryHandlerTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetTodosQueryHandler _handler;

        public GetTodosQueryHandlerTests()
        {
            _fixture = new Fixture();
            _userRepositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetTodosQueryHandler(_userRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Test()
        {
            var query = new GetTodosQuery(Guid.NewGuid());
            var user = new User("validUsername", "validEmail@test.com", "secretPassowrd", "salt");
            var todoList = CreateTodoListFixture();
            var todo = CreateTodoFixture();
            todoList.AddTodo(todo);
            user.AddTodoList(todoList);
            _userRepositoryMock.Setup(x => x.GetByIdAsync(query.UserId))
                .ReturnsAsync(user);
            _mapperMock.Setup(x => x.Map<IEnumerable<TodoDto>>(It.IsAny<List<Todo>>()))
                .Returns(new List<TodoDto>() { new TodoDto { Id = todo.Id, Name = todo.Name, FinishDate = todo.FinishDate } });

            var todos = await _handler.HandleAsync(query);

            _mapperMock.Verify(x => x.Map<IEnumerable<TodoDto>>(todoList.Todos), Times.Once);
        }

        private Todo CreateTodoFixture()
            => _fixture.Build<Todo>().Create();

        private TodoList CreateTodoListFixture()
            => _fixture.Build<TodoList>().Create();
    }
}
