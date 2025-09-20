using System;
using System.Globalization;

namespace TodoConsoleApp
{
    internal static class Program
    {
        private static void Main()
        {
            using (var context = new TodoContext())
            {
                context.Database.EnsureCreated();
            }

            var service = new TodoService(() => new TodoContext());
            var app = new TodoConsoleApp(service);
            app.Run();
        }
    }

    internal sealed class TodoConsoleApp
    {
        private readonly TodoService _service;

        public TodoConsoleApp(TodoService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public void Run()
        {
            var shouldExit = false;
            while (!shouldExit)
            {
                DisplayMenu();
                var input = Console.ReadLine();
                Console.WriteLine();

                switch (input)
                {
                    case "1":
                        ListTodos();
                        break;
                    case "2":
                        AddTodo();
                        break;
                    case "3":
                        CompleteTodo();
                        break;
                    case "4":
                        DeleteTodo();
                        break;
                    case "5":
                        shouldExit = true;
                        break;
                    default:
                        Console.WriteLine("Unrecognized option. Please choose between 1 and 5.");
                        break;
                }

                if (!shouldExit)
                {
                    Console.WriteLine();
                    Console.WriteLine("Press ENTER to continue...");
                    Console.ReadLine();
                }
            }
        }

        private static void DisplayMenu()
        {
            Console.Clear();
            Console.WriteLine("==============================");
            Console.WriteLine("        TODO LIST APP         ");
            Console.WriteLine("==============================");
            Console.WriteLine("1. View tasks");
            Console.WriteLine("2. Add a task");
            Console.WriteLine("3. Mark task as completed");
            Console.WriteLine("4. Delete a task");
            Console.WriteLine("5. Exit");
            Console.Write("Select an option: ");
        }

        private void ListTodos()
        {
            var todos = _service.GetTodos();

            if (todos.Count == 0)
            {
                Console.WriteLine("No tasks found. Add your first task!");
                return;
            }

            Console.WriteLine("Current tasks:");
            Console.WriteLine();

            foreach (var todo in todos)
            {
                var status = todo.IsCompleted ? "[x]" : "[ ]";
                var created = todo.CreatedAt.ToLocalTime().ToString("g", CultureInfo.CurrentCulture);
                Console.WriteLine(string.Format(CultureInfo.CurrentCulture, "{0} {1}. {2} (Created {3})", status, todo.Id, todo.Title, created));
            }
        }

        private void AddTodo()
        {
            Console.Write("Enter a task description: ");
            var title = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(title))
            {
                Console.WriteLine("Task description cannot be empty.");
                return;
            }

            _service.AddTodo(title.Trim());
            Console.WriteLine("Task added successfully.");
        }

        private void CompleteTodo()
        {
            Console.Write("Enter the task id to mark as completed: ");
            var input = Console.ReadLine();

            if (!int.TryParse(input, NumberStyles.Integer, CultureInfo.InvariantCulture, out var id))
            {
                Console.WriteLine("Invalid id. Please enter a numeric value.");
                return;
            }

            var result = _service.CompleteTodo(id);
            Console.WriteLine(result ? "Task marked as completed." : "Task not found.");
        }

        private void DeleteTodo()
        {
            Console.Write("Enter the task id to delete: ");
            var input = Console.ReadLine();

            if (!int.TryParse(input, NumberStyles.Integer, CultureInfo.InvariantCulture, out var id))
            {
                Console.WriteLine("Invalid id. Please enter a numeric value.");
                return;
            }

            var result = _service.DeleteTodo(id);
            Console.WriteLine(result ? "Task deleted." : "Task not found.");
        }
    }
}
