using System;

namespace YetAnotherTodoApp.Application.Commands.Models
{
    public class UpdateTodoCommand : ICommand
    {
        public Guid UserId { get; set; }
        public Guid TodoId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime FinishDate { get; set; }
    }
}
