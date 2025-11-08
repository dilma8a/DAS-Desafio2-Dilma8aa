using Microsoft.EntityFrameworkCore;
using DAS_Desafio2_Dilma8a.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure EF Core DbContext for SQL Server (Windows Authentication / Trusted Connection)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? "Server=localhost;Database=RegistroAcademico;Trusted_Connection=True;";

builder.Services.AddDbContext<DAS_Desafio2_Dilma8a.Data.AppDbContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
