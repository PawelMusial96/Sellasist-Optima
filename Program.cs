using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sellasist_Optima.Areas.Identity.Data;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Sellasist_OptimaContextConnection") ?? throw new InvalidOperationException("Connection string 'Sellasist_OptimaContextConnection' not found.");

builder.Services.AddDbContext<Sellasist_OptimaContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
   .AddRoles<IdentityRole>()
   .AddEntityFrameworkStores<Sellasist_OptimaContext>();

//builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<Sellasist_OptimaContext>();

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

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var roles = new[] { "Admin", "Customer" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }
}


using (var scope = app.Services.CreateScope())
{
    var userManager=
        scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    string emial = "admin@admin.com";
    string password = "Test1234!";

    if (await userManager.FindByEmailAsync(emial) == null)
    {
        var user = new IdentityUser();
        user.UserName = emial;
        user.Email = emial;

        await userManager.CreateAsync(user, password);

        await userManager.AddToRoleAsync(user, "Admin");
    }
}
app.Run();
