using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sellasist_Optima.Areas.Identity.Data;

namespace Sellasist_Optima.Areas.Identity.Data;

public class Sellasist_OptimaContext : IdentityDbContext<Sellasist_OptimaUser>
{
    public Sellasist_OptimaContext(DbContextOptions<Sellasist_OptimaContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
