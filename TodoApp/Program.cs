using Microsoft.EntityFrameworkCore;
using TodoApp.Models;
using TodoApp.ViewModel;
 

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DbConnection") ?? throw new InvalidOperationException("Database not found.");

builder.Services.AddDbContext<TodoDbContext>(options =>
   options.UseSqlServer(connectionString)
);

// Add services to the container.  
builder.Services.AddSingleton<UserViewModel>();
builder.Services.AddSingleton<TodoViewModel>();
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

//Add session services
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => { 
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout to 30 minutes
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAuthentication("sessionAuth").AddCookie("sessionAuth", options =>
{
    options.LoginPath = "/Users/Login";
});

var app = builder.Build();

// Configure the HTTP request pipeline.  
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.  
    app.UseHsts();
}

app.UseSession(); // Enable session middleware

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
   name: "default",
   pattern: "{controller=User}/{action=Login}/{id?}");

app.Run();
