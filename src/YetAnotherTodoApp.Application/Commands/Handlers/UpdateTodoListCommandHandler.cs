﻿using System.Threading.Tasks;
using YetAnotherTodoApp.Application.Commands.Models;
using YetAnotherTodoApp.Application.Exceptions;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Commands.Handlers
{
    public class UpdateTodoListCommandHandler : ICommandHandler<UpdateTodoListCommand>
    {
        private readonly ITodoListRepository _repository;

        public UpdateTodoListCommandHandler(ITodoListRepository repository)
        {
            _repository = repository;
        }

        public async Task HandleAsync(UpdateTodoListCommand command)
        {
            if (await _repository.CheckIfUserHasGotTodoListWithGivenTitle(command.UserId, command.Title))
                throw new TodoListWithGivenTitleAlreadyExistsException(command.Title);

            var todoList = await _repository.GetForUserAsync(command.UserId, command.TodoListId);
            todoList.UpdateTitle(command.Title);

            _repository.SaveChangesAsync();
        }
    }
}