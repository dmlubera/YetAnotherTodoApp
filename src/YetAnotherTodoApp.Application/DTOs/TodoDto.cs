using System;
using System.Text.Json.Serialization;
using YetAnotherTodoApp.Domain.Enums;

namespace YetAnotherTodoApp.Application.DTOs
{
    public class TodoDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime FinishDate { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TodoStatus Status { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TodoPriority Priority { get; set; }
    }
}
