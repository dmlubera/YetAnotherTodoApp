﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using YetAnotherTodoApp.Domain.Enums;

namespace YetAnotherTodoApp.Application.DTOs
{
    public class TodoDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string TodoList { get; set; }
        public DateTime FinishDate { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TodoStatus Status { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TodoPriority Priority { get; set; }
        public List<StepDto> Steps { get; set; }
    }
}