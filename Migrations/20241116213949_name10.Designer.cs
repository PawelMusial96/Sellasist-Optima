﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sellasist_Optima.BazyDanych;

#nullable disable

namespace Sellasist_Optima.Migrations.Configuration
{
    [DbContext(typeof(ConfigurationContext))]
    [Migration("20241116213949_name10")]
    partial class name10
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.18")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Sellasist_Optima.Mapping.AttributeMappingModels", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("SellAsistAttributeId")
                        .HasColumnType("int");

                    b.Property<int>("WebApiAttributeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("AttributeMappings");
                });

            modelBuilder.Entity("Sellasist_Optima.Models.SellAsistAPI", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("KeyAPI")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("ShopName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.ToTable("SellAsistAPI");
                });

            modelBuilder.Entity("Sellasist_Optima.SellAsistModels.SellAsistProduct", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("EAN")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("SellAsistProduct");
                });

            modelBuilder.Entity("Sellasist_Optima.WebApiModels.ProductMapping", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("SellAsistProductId")
                        .HasColumnType("int");

                    b.Property<int>("WebApiProductId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SellAsistProductId");

                    b.HasIndex("WebApiProductId");

                    b.ToTable("ProductMappings");
                });

            modelBuilder.Entity("Sellasist_Optima.WebApiModels.WebApiClient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Grant_type")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Localhost")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("TokenAPI")
                        .IsRequired()
                        .HasMaxLength(400)
                        .HasColumnType("nvarchar(400)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.ToTable("WebApiClient");
                });

            modelBuilder.Entity("Sellasist_Optima.WebApiModels.WebApiProduct", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("barcode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("WebApiProduct");
                });

            modelBuilder.Entity("Sellasist_Optima.WebApiModels.ProductMapping", b =>
                {
                    b.HasOne("Sellasist_Optima.SellAsistModels.SellAsistProduct", "SellAsistProduct")
                        .WithMany()
                        .HasForeignKey("SellAsistProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Sellasist_Optima.WebApiModels.WebApiProduct", "WebApiProduct")
                        .WithMany()
                        .HasForeignKey("WebApiProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SellAsistProduct");

                    b.Navigation("WebApiProduct");
                });
#pragma warning restore 612, 618
        }
    }
}
