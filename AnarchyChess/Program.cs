
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using AnarchyChess.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using AnarchyChess.Services;
using AnarchyChess.Hubs;
using AnarchyChess.Models;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AnarchyChessContextConnection") ?? throw new InvalidOperationException("Connection string 'AnarchyChessContextConnection' not found.");

var options = new DbContextOptionsBuilder()
    .UseSqlite($"Data Source={connectionString}")
    .Options;

using (var db = new AnarchyChessContext(options))
{
    //db.Database.EnsureDeleted();
    db.Database.EnsureCreated();
}

builder.Services.AddDbContext<AnarchyChessContext>(options =>
    options.UseSqlite($"Data Source={connectionString}"));

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<AnarchyChessContext>();

// Add services to the container.
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeAreaFolder("Play", "/");
    options.Conventions.AuthorizeAreaFolder("Vote", "/");
});

builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddTransient<GameService>();
builder.Services.AddTransient<PropositionService>();
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.MapRazorPages();

app.UseEndpoints(endpoints =>
endpoints.MapHub<GameHub>("/gameHub"));

app.Run();
