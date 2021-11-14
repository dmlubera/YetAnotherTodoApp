﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using YetAnotherTodoApp.Application.DTOs;
using YetAnotherTodoApp.Domain.Enums;

namespace YetAnotherTodoApp.Api.Models.Todos
{
    public class AddTodoRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string TodoList { get; set; }
        public DateTime FinishDate { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TodoPriority? Priority { get; set; }
        public ICollection<TodoTaskRequestDto> Tasks { get; set; }
    }
}