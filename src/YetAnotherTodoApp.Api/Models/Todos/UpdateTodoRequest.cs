﻿using System;

namespace YetAnotherTodoApp.Api.Models.Todos
{
    public class UpdateTodoRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime FinishDate { get; set; }
    }
}