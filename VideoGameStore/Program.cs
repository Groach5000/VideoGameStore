using VideoGameStore.Data;
using VideoGameStore.Data.Cart;
using VideoGameStore.Data.Services;
using VideoGameStore.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VideoGameStore.Configurations;
using System.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Services Confirguration:
builder.Services.AddScoped<IPublishersService, PublishersService>();
builder.Services.AddScoped<IDevelopersService, DevelopersService>();
builder.Services.AddScoped<IVideoGamesService, VideoGamesService>();
builder.Services.AddScoped<IOrdersService, OrdersService>();

//Addtions for cart session
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped(sc => ShoppingCart.GetShoppingCart(sc));

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

// Get the secret key for JWT configuration. This will replace the cookie authentication previously used.
builder.Services.Configure<JwtConfiguration>(builder.Configuration.GetSection(key: "JwtConfig"));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(jwt =>
{
    var key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection(key: "JwtConfig:Secret").Value);

    jwt.SaveToken = true;
    jwt.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),

        // While developing on localhost, set false so not to cause issues with HTTPS
        ValidateIssuer = false, //ToDo Dev purposes only, change to true on launch
        ValidateAudience = false, //ToDo Dev purposes only, change to true on launch
        RequireExpirationTime = false, //ToDo Dev purposes only, change to true on launch, update when refresh token is added

        ValidateLifetime = true,
    };
});

//Authentication and authorization
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddMemoryCache();

// ToDo: Replacing with JWT, but currently keeping in code to maintain user interface login until the conversion is complete.
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
});



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
    pattern: "{controller=VideoGames}/{action=Index}/{id?}");

//Seed Database
AppDbInitializer.Seed(app);



AppDbInitializer.SeedUsersAndRolesAsync(app).Wait();

app.Run();
