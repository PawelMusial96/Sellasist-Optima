﻿using Microsoft.EntityFrameworkCore;
using Sellasist_Optima.Mapping;
using Sellasist_Optima.Models;
using Sellasist_Optima.ModelsAplikacji;
using Sellasist_Optima.SellAsistModels;
using Sellasist_Optima.WebApiModels;

namespace Sellasist_Optima.BazyDanych
{
    public class ConfigurationContext : DbContext
    {
        public ConfigurationContext(DbContextOptions<ConfigurationContext> options)
        : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //SellAsistAPI
            builder.Entity<SellAsistAPI>()
                .HasKey(a => a.Id);

            builder.Entity<SellAsistAPI>()
               .Property(a => a.ShopName)
               .IsRequired()
               .HasMaxLength(255);

            builder.Entity<SellAsistAPI>()
                .Property(a => a.KeyAPI)
                .IsRequired()
                .HasMaxLength(255); // Maksymalna długość nazwy (255 znaków)

            //WebAPI

            builder.Entity<WebApiClient>()
                .HasKey(a => a.Id);

            builder.Entity<WebApiClient>()
               .Property(a => a.Username)
               .IsRequired()
               .HasMaxLength(255);

            builder.Entity<WebApiClient>()
               .Property(a => a.Password)
               .IsRequired()
               .HasMaxLength(255);

            builder.Entity<WebApiClient>()
               .Property(a => a.Grant_type)
               .IsRequired()
               .HasMaxLength(255);

            builder.Entity<WebApiClient>()
               .Property(a => a.Localhost)
               .IsRequired()
               .HasMaxLength(255);
            
            builder.Entity<WebApiClient>()
              .Property(a => a.TokenAPI)
              .IsRequired()
              .HasMaxLength(400);

            builder.Entity<WebApiClient>()
              .Property(a => a.CompanyName)
              .HasMaxLength(255);

            builder.Entity<WebApiClient>()
              .Property(a => a.DatabaseName)
              .HasMaxLength(255);

            builder.Entity<KonfiguracjaAplikacji>()
                  .HasKey(k => k.Id);

            // Możesz ograniczyć długość kolumny:
            builder.Entity<KonfiguracjaAplikacji>()
                  .Property(k => k.PayerCodeName)
                  .HasMaxLength(255)
                  .IsRequired(false);


        }

        public DbSet<SellAsistAPI> SellAsistAPI { get; set; }
        public DbSet<WebApiClient> WebApiClient { get; set; }
        public DbSet<AttributeMappingModels> AttributeMappings { get; set; }
        public DbSet<ProductMapping> ProductMappings { get; set; }
        public DbSet<KonfiguracjaAplikacji> KonfiguracjaAplikacji { get; set; }

    }
}

