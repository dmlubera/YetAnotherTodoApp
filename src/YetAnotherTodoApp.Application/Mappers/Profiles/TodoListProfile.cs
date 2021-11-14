using AutoMapper;
using YetAnotherTodoApp.Application.DTOs;
using YetAnotherTodoApp.Domain.Entities;

namespace YetAnotherTodoApp.Application.Mappers.Profiles
{
    public class TodoListProfile : Profile
    {
        public TodoListProfile()
        {
            CreateMap<TodoList, TodoListDto>()
                .ForMember(x => x.Title, opt => opt.MapFrom(x => x.Title.Value))
                .ForMember(x => x.TodosNumber, opt => opt.MapFrom(x => x.Todos.Count));

            CreateMap<TodoList, DetailedTodoListDto>()
                .ForMember(x => x.Title, opt => opt.MapFrom(x => x.Title.Value));
        }
    }
}