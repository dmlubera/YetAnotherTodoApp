﻿using System;
using System.Threading.Tasks;
using YetAnotherTodoApp.Application.Cache;
using YetAnotherTodoApp.Application.Commands.Models;
using YetAnotherTodoApp.Domain.Entities;
using YetAnotherTodoApp.Domain.Repositories;

namespace YetAnotherTodoApp.Application.Commands.Handlers
{
    public class CreateTodoListCommandHandler : ICommandHandler<CreateTodoListCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICache _cache;

        public CreateTodoListCommandHandler(IUserRepository userRepository, ICache cache)
        {
            _userRepository = userRepository;
            _cache = cache;
        }
        
        public async Task HandleAsync(CreateTodoListCommand command)
        {
            var user = await _userRepository.GetByIdAsync(command.UserId);
            var todoList = new TodoList(command.Title);
            user.AddTodoList(todoList);
            await _userRepository.SaveChangesAsync();

            _cache.Set(command.CacheToken.ToString(), todoList.Id, TimeSpan.FromSeconds(99));
        }
    }
}