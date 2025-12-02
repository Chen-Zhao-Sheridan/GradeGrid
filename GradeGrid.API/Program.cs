using GradeGrid.Core;
using GradeGrid.Infrastructure;
using GradeGrid.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    }
);

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// connect Sqllite
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<GradeGridDbContext>(opt => // redirect migrations into infra instad of api
    opt.UseSqlite(connectionString, options => options.MigrationsAssembly("GradeGrid.Infrastructure"))
);

// dependency injection
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<ISectionRepository, SectionRepository>();
builder.Services.AddScoped<IEvaluationItemRepository, EvaluationItemRepository>();
builder.Services.AddScoped<IScheduleGenerator, CourseScheduleGenerator>();

var app = builder.Build();

// migrate and seed some data if not there
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<GradeGridDbContext>();
        context.Database.Migrate();
        CourseDataSeeder.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
