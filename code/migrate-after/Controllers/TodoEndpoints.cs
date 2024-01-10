using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TodoApi.Models;
using Microsoft.AspNetCore.OpenApi;

namespace TodoApi.Controllers
{
    public static class TodoEndpoints
    {
        //private static readonly TodoContext _context;

        public static void RegisterEndpoints(this IEndpointRouteBuilder app)
        {
            var grp = app.MapGroup("/api/todo")
                .WithOpenApi();

            grp.MapGet("", async (TodoContext ctx) => await GetTodoItem(ctx));
            grp.MapGet("/{id}", async (int id, TodoContext ctx) => await GetTodoItem(id, ctx));
            grp.MapPost("", async (TodoItem todo, TodoContext ctx) => await PostTodoItem(todo, ctx));
            grp.MapPut("/{id}", async (int id, TodoItem todo, TodoContext ctx) => await PutTodoItem(id, todo, ctx));
            grp.MapDelete("/{id}", async (int id, TodoContext ctx) => await DeleteTodoItem(id, ctx));
        }

        //public TodoController(TodoContext context)
        //{
        //    _context = context;

        //    if (_context.TodoItems.Count() == 0)
        //    {
        //        _context.TodoItems.Add(new TodoItem { Name = "Item1" });
        //        _context.SaveChanges();
        //    }
        //}

        public static async Task<IEnumerable<TodoItem>> GetTodoItem(TodoContext _context)
        {
            return await _context.TodoItems.ToListAsync();
        }

        public static async Task<IResult> GetTodoItem(long id, TodoContext _context)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return Results.NotFound();
            }

            return Results.Ok(todoItem);
        }

        public static async Task<IResult> PutTodoItem(long id, TodoItem todoItem, TodoContext _context)
        {
            if (id != todoItem.Id)
            {
                return Results.BadRequest();
            }

            _context.Entry(todoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(id, _context))
                {
                    return Results.NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Results.NoContent();
        }

        public static async Task<IResult> PostTodoItem(TodoItem todoItem, TodoContext _context)
        {
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            //return Results.Created("GetTodoItem", new { id = todoItem.Id }, todoItem);
            return Results.Created($"/todo/{todoItem.Id}", todoItem);
        }

        public static async Task<IResult> DeleteTodoItem(long id, TodoContext _context)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return Results.NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return Results.Ok(todoItem);
        }

        private static bool TodoItemExists(long id, TodoContext _context)
        {
            return _context.TodoItems.Any(e => e.Id == id);
        }
    }
}
