using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sellasist_Optima.Areas.Identity.Data;
using Sellasist_Optima.Models;

namespace Sellasist_Optima.Areas.Identity.Data;

public class Sellasist_OptimaContext : IdentityDbContext<IdentityUser>
{
    public Sellasist_OptimaContext(DbContextOptions<Sellasist_OptimaContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Dodaj konfigurację dla klasy Api
        builder.Entity<SellAsistAPI>()
            .HasKey(a => a.Id);

        builder.Entity<SellAsistAPI>()
            .Property(a => a.TokenAPI)
            .IsRequired()
            .HasMaxLength(255); // Maksymalna długość nazwy (255 znaków)

        // Configure UserId to be linked to IdentityUser.Id
        builder.Entity<SellAsistAPI>()
            .HasOne<IdentityUser>()
            .WithMany()
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(builder);

    }

    public DbSet<SellAsistAPI> SellAsistAPI { get; set; }
}
