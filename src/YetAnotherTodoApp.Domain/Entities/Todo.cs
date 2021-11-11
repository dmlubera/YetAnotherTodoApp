using System;
using System.Collections.Generic;
using System.Linq;
using YetAnotherTodoApp.Domain.Enums;
using YetAnotherTodoApp.Domain.Exceptions;
using YetAnotherTodoApp.Domain.ValueObjects;

namespace YetAnotherTodoApp.Domain.Entities
{
    public class Todo : AuditableEntity
    {
        private readonly List<TodoTask> _tasks = new List<TodoTask>();
        public Title Title { get; private set; }
        public string Description { get; private set; }
        public FinishDate FinishDate { get; private set; }
        public TodoStatus Status { get; private set; }
        public TodoPriority Priority { get; private set; }
        public TodoList TodoList { get; private set; }
        public IReadOnlyCollection<TodoTask> Tasks => _tasks.AsReadOnly();

        protected Todo() { }

        public Todo(string title, DateTime finishDate, string description = null, TodoPriority priority = TodoPriority.Normal)
        {
            Id = Guid.NewGuid();
            Title = Title.Create(title);
            FinishDate = FinishDate.Create(finishDate);
            if (description is { })
                Description = description;
            Priority = priority;
            UpdateAuditInfo();
        }

        public void Update(string title, string description, DateTime finishDate)
        {
            Title = Title.Create(title);
            Description = description;
            FinishDate = FinishDate.Create(finishDate);
            LastModifiedAt = DateTime.UtcNow;
        }

        public void UpdateStatus(TodoStatus updatedStatus)
        {
            if (updatedStatus == TodoStatus.Done && _tasks.Any(x => !x.IsFinished))
                throw new CannotChangeStatusToDoneOfTodoWithUnfinishedTaskException();

            Status = updatedStatus;
            LastModifiedAt = DateTime.UtcNow;
        }

        public void UpdatePriority(TodoPriority updatedPriority)
        {
            Priority = updatedPriority;
            LastModifiedAt = DateTime.UtcNow;
        }

        public void AddTasks(IEnumerable<TodoTask> tasks)
            => _tasks.AddRange(tasks);

        public void DeleteTask(Guid taskId)
        {
            var task = _tasks.FirstOrDefault(x => x.Id == taskId);
            _tasks.Remove(task);
        }
    }
}