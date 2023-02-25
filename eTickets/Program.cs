using VideoGameStore.Data;
using VideoGameStore.Data.Cart;
using VideoGameStore.Data.Services;
using VideoGameStore.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Services Confirguration:
builder.Services.AddScoped<IActorsService, ActorsService>();
builder.Services.AddScoped<IProducersService, ProducersService>();
builder.Services.AddScoped<ICinemasService, CinemasService>();
builder.Services.AddScoped<IMoviesService, MoviesService>();
builder.Services.AddScoped<IOrdersService, OrdersService>();

//Addtions for cart session
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped(sc => ShoppingCart.GetShoppingCart(sc));

//Authentication and authorization
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddMemoryCache();
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
});

// Additions for cookies
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});



// Add services to the container.
builder.Services.AddControllersWithViews();


// DbContext configuration
// to add UseSqlServer(), go to Tools->"NuGet Package Manager"=>"Manage.."=>Browes for "sql server", click:
    // "Microsoft.EntityFrameworkCore.SqlServer" add it to the project and click install, then add:
    // using Microsoft.EntityFrameworkCore above.
    // Then configure the connection string set in appsettings.json
builder.Services.AddDbContext<VideoGameStore.Data.AppDbContext>
    (options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));

// After creating the above, will have to also go to NeGet and install "Microsoft.EntityFrameworkCore.Tools
// Then you have to add the migration by going to NeGet => Package Manager Console => type:
// "Add-Migration " + Name, can be "Initial" if it runs without errors, it will create the class:
// "class Initial : Migration" under the Migrations folder
// Then, in the console type: "Update-Database"

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

//Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Movies}/{action=Index}/{id?}");

//Seed Database
AppDbInitializer.Seed(app);



AppDbInitializer.SeedUsersAndRolesAsync(app).Wait();

app.Run();
