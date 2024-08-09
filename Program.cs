using AvioIndustrija;
using AvioIndustrija.Data;
using AvioIndustrija.Helpers;
using AvioIndustrija.Interfaces;
using AvioIndustrija.Models;
using AvioIndustrija.Repository;
using AvioIndustrija.Services;
using AvioIndustrija.Utils;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddScoped<UploadImageFromExcel>();
builder.Services.AddScoped<IVijestRepository, VijestRepository>();
builder.Services.AddScoped<IOcjenaRepository, OcjenaRepository>();
builder.Services.AddScoped<IKomentarRepository, KomentarRepository>();
builder.Services.AddScoped<MarkdownService>();
builder.Services.AddScoped<IServisDioRepository, ServisDioRepository>();
builder.Services.AddScoped<IServisRepository, ServisRepository>();
builder.Services.AddScoped<IDioRepository, DioRepository>();
builder.Services.AddScoped<IAvioniRepository, AvionRepository>();
builder.Services.AddScoped<IPutnikRepository, PutnikRepository>();
builder.Services.AddScoped<IRelacijaRepository, RelacijaRepository>();
builder.Services.AddScoped<IIstorijaLetovaPutnikaRepository, IstorijaLetovaPutnikaRepository>();
builder.Services.AddScoped<ILetRepository, LetRepository>();
builder.Services.AddScoped<IPhotoService, PhotoService>();
builder.Services.AddScoped<ExcelService>();
builder.Services.AddScoped<AvionValidation>();
builder.Services.AddScoped<ListaNevalidnihAviona>();

builder.Services.AddDistributedMemoryCache(); // Adds a default in-memory implementation of IDistributedCache
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
builder.Services.AddDbContext<AvioIndustrija2424Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AvioIndustrija")));
builder.Services.AddIdentity<AppUser, IdentityRole>(options => {
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
})
    .AddEntityFrameworkStores<AvioIndustrija2424Context>();
builder.Services.AddMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(1); // Set session timeout
    options.Cookie.HttpOnly = true; // Make the session cookie HTTP only
    options.Cookie.IsEssential = true; // Make the session cookie essential
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();


builder.Services.AddSingleton<
    IAuthorizationMiddlewareResultHandler, SampleAuthorizationMiddlewareResultHandler>();


var app = builder.Build();

if (args.Length == 1 && args[0].ToLower() == "seeddata")
{
    await Seed.SeedUsersAndRolesAsync(app);
    //Seed.SeedData(app);
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
app.UseAuthentication();


app.UseAuthorization();
app.UseSession(); // Add this line


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}/{idd?}");

app.Run();
