var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();

// Enable session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // How long the session lasts
    options.Cookie.HttpOnly = true; // Prevent client-side access to the cookie
    options.Cookie.IsEssential = true; // Ensure the session cookie is always set
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.Use(async (context, next) =>
{
    var isLoggedIn = context.Session.GetString("IsLoggedIn");
    var path = context.Request.Path.ToString().ToLower();

    // Allow access to following pages without authentication
    if (string.IsNullOrEmpty(isLoggedIn) &&
        !path.Contains("/auth/login") &&
        !path.Contains("/auth/register") &&
        !path.Contains("home/index") &&
        !path.Contains("home/privacy"))
        
    {
        context.Response.Redirect("/Home/Index");
        return;
    }

    await next();
});

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();