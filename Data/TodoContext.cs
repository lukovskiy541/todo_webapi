using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using todo_webapi.Models;

namespace todo_webapi.Data
{
    public class TodoContext : IdentityDbContext<ApplicationUser>
    {
        public TodoContext(DbContextOptions<TodoContext> options)
       : base(options)
        {
        }

        public DbSet<TodoItem> TodoItems { get; set; } = null!;

        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoItem>()
                .HasOne(t => t.Category)
                .WithMany(c => c.TodoItems)
                .HasForeignKey(t => t.CategoryId);

            modelBuilder.Entity<Category>().HasData(
               new Category { Id = 1, Name = "Work" },
               new Category { Id = 2, Name = "Personal" }
           );

            modelBuilder.Entity<TodoItem>().HasData(
                new TodoItem { Id = 1, Name = "Finish project", IsComplete = false, CategoryId = 1 },
                new TodoItem { Id = 2, Name = "Buy groceries", IsComplete = true, CategoryId = 2 }
            );

            base.OnModelCreating(modelBuilder);
        }


    }
}
