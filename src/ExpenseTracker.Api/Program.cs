using ExpenseTracker.Application.Interfaces.Repositories;
using ExpenseTracker.Application.Interfaces.Services;
using ExpenseTracker.Application.Services;
using ExpenseTracker.Infrastructure;
using ExpenseTracker.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(); //asss MVC controllers, to [ApiController] classes work
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add SQL Server connection
builder.Services.AddDbContext<ExpenseTrackerDbContext>(options =>   //register my database context named ExpenseTrackerDbContext in the dependecy injection container and cofigure it to use sql server with the connection string called DefaulConnection from my configuration file.
{
    var cs = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(cs); //take the ConnectionString from my appsetting.Development.json and use in order to make a database using SQL server
    //now the database exists and when i want later to use it inside my controller, i can simply make a constructor using it with DI ... (_db = db)
});

// Services (Application)
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IExpenseService, ExpenseService>();

var app = builder.Build(); //create the web app pipeline object

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers(); //activate attrubute routing so my controllers [Route] and [HttpGet] are reachable

// Quick test endpoint
app.MapGet("/ping", () => Results.Ok(new { ok = true, atUtc = DateTime.UtcNow }));

app.Run();
