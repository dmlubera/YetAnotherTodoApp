using System.Text.Json.Serialization;
using YetAnotherTodoApp.Domain.Enums;

namespace YetAnotherTodoApp.Api.Models
{
    public class UpdateTodoStatusRequest
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TodoStatus Status { get; set; }
    }
}