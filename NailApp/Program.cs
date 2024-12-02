using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NailApp.Commands.CreateAccount;
using NailApp.Data;
using NailApp.Models;
using NailApp.Repository;
using NailApp.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontEnd", policy =>
    policy.WithOrigins("http://localhost:3000")
        .AllowAnyMethod()
        .AllowAnyHeader());
});


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddMediatR(config =>
    config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Registro do RoleManager e UserManager
builder.Services.AddScoped<RoleManager<IdentityRole>>();  // Registrar o RoleManager com IdentityRole
builder.Services.AddScoped<UserManager<ApplicationUser>>();

builder.Services.AddScoped<CommandRepository>();  // Registro do CommandRepository
builder.Services.AddScoped<QueriesRepository>();  // Registro do QueriesRepository
builder.Services.AddScoped<IQueriesServices, QueriesServices>();  // Registro do QueriesServices
builder.Services.AddScoped<ICommandService, CommandService>();  // Registro do QueriesServices

// Registrar o UserManager com ApplicationUser
builder.Services.AddRazorPages();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Seed the database with test data.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    DbInitializer.Initialize(services, userManager, roleManager).Wait();
}

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

app.UseCors("AllowFrontEnd");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.MapRazorPages();
app.Run();