using System.Text.Json.Serialization;
using YetAnotherTodoApp.Domain.Enums;

namespace YetAnotherTodoApp.Api.Models.Todos
{
    public class UpdateTodoPriorityRequest
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TodoPriority Priority { get; set; }
    }
}