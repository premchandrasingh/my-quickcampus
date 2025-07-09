using My.QuickCampus;
using My.QuickCampus.Data;

var builder = WebApplication.CreateBuilder(args);


var appSettings = builder.Configuration.ConfigureAndGet<AppSettings>(builder.Services, "AppSettings");
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<QuickCampusService>();

builder.Services.AddSqliteDatabase(builder.Configuration);

var app = builder.Build();
app.Migrate();

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
