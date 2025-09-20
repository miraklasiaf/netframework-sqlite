using System;
using System.Collections.Generic;
using System.Linq;

namespace TodoConsoleApp
{
    public sealed class TodoService
    {
        private readonly Func<TodoContext> _contextFactory;

        public TodoService(Func<TodoContext> contextFactory)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
        }

        public IList<TodoItem> GetTodos()
        {
            using (var context = _contextFactory())
            {
                return context.TodoItems
                    .OrderBy(t => t.IsCompleted)
                    .ThenBy(t => t.CreatedAt)
                    .ToList();
            }
        }

        public void AddTodo(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(title));
            }

            using (var context = _contextFactory())
            {
                var todo = new TodoItem
                {
                    Title = title.Trim(),
                    CreatedAt = DateTime.UtcNow,
                    IsCompleted = false
                };

                context.TodoItems.Add(todo);
                context.SaveChanges();
            }
        }

        public bool CompleteTodo(int id)
        {
            using (var context = _contextFactory())
            {
                var todo = context.TodoItems.FirstOrDefault(t => t.Id == id);
                if (todo == null)
                {
                    return false;
                }

                if (!todo.IsCompleted)
                {
                    todo.IsCompleted = true;
                    context.SaveChanges();
                }

                return true;
            }
        }

        public bool DeleteTodo(int id)
        {
            using (var context = _contextFactory())
            {
                var todo = context.TodoItems.FirstOrDefault(t => t.Id == id);
                if (todo == null)
                {
                    return false;
                }

                context.TodoItems.Remove(todo);
                context.SaveChanges();
                return true;
            }
        }
    }
}
