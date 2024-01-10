using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace TodoApi.Models
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {
            if (this.TodoItems.Count() == 0)
            {
                this.TodoItems.Add(new TodoItem { Name = "Item1" });
                this.SaveChanges();
            }
        }

        public DbSet<TodoItem> TodoItems { get; set; }
    }
}
