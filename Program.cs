using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sellasist_Optima.Areas.Identity.Data;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Sellasist_OptimaContextConnection") ?? throw new InvalidOperationException("Connection string 'Sellasist_OptimaContextConnection' not found.");

builder.Services.AddDbContext<Sellasist_OptimaContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<Sellasist_OptimaUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<Sellasist_OptimaContext>();

// Add services to the container.
builder.Services.AddRazorPages();

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

app.UseAuthorization();

app.MapRazorPages();

app.Run();
