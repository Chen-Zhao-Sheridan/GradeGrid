using GradeGrid.Infrastructure;
using GradeGrid.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// connect Sqllite
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<GradeGridDbContext>(opt => // redirect migrations into infra instad of api
    opt.UseSqlite(connectionString, options => options.MigrationsAssembly("GradeGrid.Infrastructure"))
);

// dependency injection
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IEvaluationItemRepository, EvaluationItemRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
