
using GradeGrid.Core.Interfaces;
using GradeGrid.Infrastructure;
using System.Net.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

builder.Services.AddScoped<INoteRepository, NoteRepository>();
builder.Services.AddScoped<ICalendarEventRepository, CalendarEventRepository>();

builder.Services.AddHttpClient("GradeGridApi", client =>
{
    var baseUrl = "https://localhost:7000/";
    client.BaseAddress = new Uri(baseUrl);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    //pattern: "{controller=Notes}/{action=NotesList}/{id?}")
    .WithStaticAssets();

app.Run();
