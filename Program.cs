using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using todo_webapi.Core.Entities;
using todo_webapi.Filters;
using TodoApp.Application.Interfaces;
using TodoApp.Application.Services;
using TodoApp.Domain;
using TodoApp.Infrastructure.Data;
using TodoApp.Infrastructure.Repositories;

public static class Program
{
    public static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        var useInMemoryDB = builder.Configuration.GetValue<bool>("UseInMemoryDB");

        // Add services to the container.

        builder.Services.AddControllers(options =>
        {
            options.Filters.Add<GlobalExceptionFilter>();
        });
       
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
            });
        });

        builder.Services.AddScoped<IToDoItemRepository, ToDoItemRepository>();
        builder.Services.AddScoped<IToDoItemService, ToDoItemService>();
        builder.Services.AddScoped<IToDoListRepository, ToDoListRepository>();
        builder.Services.AddScoped<IToDoListService, TodoListService>();
        builder.Services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
        builder.Services.AddScoped<IApplicationUserService, ApplicationUserService>();

        if (useInMemoryDB)
        {
            builder.Services.AddInMemoryDatabase();

        }
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            };
        });




        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        if (useInMemoryDB)
        {
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                SeedData(context);
            }
        }

        app.Run();

    }

    private static void SeedData(AppDbContext context)
    {
        context.Users.AddRange(
            new ApplicationUser { UserId = 1, UserName = "JonDoe", Email = "jonh@example.com", },
            new ApplicationUser { UserId = 2, UserName = "JonDoe", Email = "jonh@example.com", }
         );

        context.SaveChanges();

       
        var todoLists = new List<TodoList>
    {
        new TodoList
        {
            Id = 1,
            UserId = 1,
            Title = "Work Projects",
            Description = "My current work tasks",
            Items = new List<TodoItem>()
        },
        new TodoList
        {
            Id = 2,
            UserId = 1,
            Title = "Shopping List",
            Description = "Things to buy",
            Items = new List<TodoItem>()
        }
    };
        context.TodoList.AddRange(todoLists);
        context.SaveChanges();

        
        var todoItems = new List<TodoItem>
    {
        new TodoItem
        {
            Id = 1,
            Name = "Complete API documentation",
            IsComplete = false,  
            TodoListId = 1
        },
        new TodoItem
        {
            Id = 2,
            Name = "Review pull requests",
            IsComplete = false,
            TodoListId = 1
        },
        new TodoItem
        {
             Id = 3,
            Name = "Buy groceries",
            IsComplete = false,
            TodoListId = 2
        },
        new TodoItem
        {
             Id = 4,
            Name = "Call dentist",
            IsComplete = false,
            TodoListId = 2
        }
    };
        context.TodoItems.AddRange(todoItems);
        context.SaveChanges();
    }
}

