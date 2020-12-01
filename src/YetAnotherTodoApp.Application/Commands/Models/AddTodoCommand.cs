using System;

namespace YetAnotherTodoApp.Application.Commands.Models
{
    public class AddTodoCommand : ICommand
    {
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public string Project { get; set; }
        public DateTime FinishDate { get; set; }
    }
}