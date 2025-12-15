using ExpenseTracker.Api.Middleware;
using ExpenseTracker.Application.Interfaces.Repositories;
using ExpenseTracker.Application.Interfaces.Services;
using ExpenseTracker.Application.Mappings;
using ExpenseTracker.Application.Services;
using ExpenseTracker.Infrastructure;
using ExpenseTracker.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.Extensions.Logging;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext
builder.Services.AddDbContext<ExpenseTrackerDbContext>(options =>
{
    var cs = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(cs);
});

// Repositories
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();

// Services
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IExpenseService, ExpenseService>();

builder.Services.AddSingleton(sp =>
{
    var loggerFactory = sp.GetRequiredService<ILoggerFactory>();

    return new AutoMapper.MapperConfiguration(
        (AutoMapper.IMapperConfigurationExpression cfg) =>
        {
            cfg.AddProfile<ExpenseTrackerMappingProfile>();
            // or: cfg.AddMaps(typeof(ExpenseTrackerMappingProfile).Assembly);
        },
        loggerFactory
    );
});

builder.Services.AddScoped<AutoMapper.IMapper>(sp =>
    sp.GetRequiredService<AutoMapper.MapperConfiguration>().CreateMapper()
);



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();
app.MapControllers();
app.MapGet("/ping", () => Results.Ok(new { ok = true, atUtc = DateTime.UtcNow }));
app.Run();
