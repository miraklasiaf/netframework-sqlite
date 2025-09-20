using System;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace TodoConsoleApp
{
    public sealed class TodoContext : DbContext
    {
        private readonly string _databasePath;

        public TodoContext()
            : this(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "todo.db"))
        {
        }

        public TodoContext(string databasePath)
        {
            if (string.IsNullOrWhiteSpace(databasePath))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(databasePath));
            }

            _databasePath = databasePath;
        }

        public DbSet<TodoItem> TodoItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(string.Format("Data Source={0}", _databasePath));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200);
                entity.Property(e => e.CreatedAt)
                    .IsRequired();
            });
        }
    }
}
