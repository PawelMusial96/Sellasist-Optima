using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sellasist_Optima.Areas.Identity.Data;
using Sellasist_Optima.Models;

namespace Sellasist_Optima.BazyDanych
{
    public class SellAsistContext : DbContext
    {
        public SellAsistContext(DbContextOptions<SellAsistContext> options)
        : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Dodaje konfigurację dla klasy Api
            builder.Entity<SellAsistAPI>()
                .HasKey(a => a.Id);

            builder.Entity<SellAsistAPI>()
               .Property(a => a.API)
               .IsRequired()
               .HasMaxLength(255);

            builder.Entity<SellAsistAPI>()
                .Property(a => a.KeyAPI)
                .IsRequired()
                .HasMaxLength(255); // Maksymalna długość nazwy (255 znaków)



           
            //builder.Entity<SellAsistAPI>()
            //    .HasOne<IdentityUser>()
            //    .WithMany()
            //    .HasForeignKey(a => a.UserId)
            //    .OnDelete(DeleteBehavior.Cascade);



            base.OnModelCreating(builder);

        }

        public DbSet<SellAsistAPI> SellAsistAPI { get; set; }

    }
}
