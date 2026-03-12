using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using MinsaitPersonBack.DTOs;
using MinsaitPersonBack.Models;
using MinsaitPersonBack.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers(options =>
{
    options.Filters.Add<MinsaitPersonBack.Filters.FluentValidationActionFilter>();
});
builder.Services.AddTransient<IValidator<CreatePersonDto>, CreatePersonDtoValidator>();
builder.Services.AddTransient<IValidator<UpdatePersonDto>, UpdatePersonDtoValidator>();
builder.Services.AddTransient<IValidator<Person>, PersonValidator>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Zeferini Person API",
        Version = "v1",
        Description = "API REST para gerenciamento de pessoas (CRUD com SQLite)"
    });

    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        options.IncludeXmlComments(xmlPath);

    options.CustomSchemaIds(type => type.FullName?.Replace("+", "."));
});

// Configure SQLite database
builder.Services.AddDbContext<PersonDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
        ?? "Data Source=persons.db";
    options.UseSqlite(connectionString);
});

// Register application services
builder.Services.AddScoped<IPersonService, PersonService>();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseCors();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Zeferini Person API v1");
    options.RoutePrefix = string.Empty;
});
app.UseAuthorization();
app.MapControllers();

// Ensure SQLite database is created
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PersonDbContext>();
    db.Database.EnsureCreated();
}

var port = Environment.GetEnvironmentVariable("PORT") ?? "3000";
app.Run($"http://0.0.0.0:{port}");

public partial class Program { } // For integration testing purposes

/*Commit #*/