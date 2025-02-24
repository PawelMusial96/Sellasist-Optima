﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sellasist_Optima.BazyDanych;

#nullable disable

namespace Sellasist_Optima.Migrations
{
    [DbContext(typeof(ConfigurationContext))]
    [Migration("20250205205853_name3")]
    partial class name3
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.20")
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

            modelBuilder.Entity("Sellasist_Optima.ModelsAplikacji.KonfiguracjaAplikacji", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("PayerCodeName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.ToTable("KonfiguracjaAplikacji");
                });

            modelBuilder.Entity("Sellasist_Optima.ModelsWebApi.Products.Attribute", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("WebApiProductId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("WebApiProductId");

                    b.ToTable("Attribute");
                });

            modelBuilder.Entity("Sellasist_Optima.ModelsWebApi.Products.CompanyData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DatabaseName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("CompanyData");
                });

            modelBuilder.Entity("Sellasist_Optima.ModelsWebApi.Products.Price", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Error")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ItemBarcode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ItemCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Number")
                        .HasColumnType("int");

                    b.Property<string>("OptimaItemId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Type")
                        .HasColumnType("int");

                    b.Property<string>("Unit")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Value")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("VatRate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("WebApiProductId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("WebApiProductId");

                    b.ToTable("Price");
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

                    b.Property<string>("Symbol")
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

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("DatabaseName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

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

                    b.Property<string>("Barcode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CatalogNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CompanyDataId")
                        .HasColumnType("int");

                    b.Property<string>("Created")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DefaultGroup")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("GettingElementsForFSPA")
                        .HasColumnType("int");

                    b.Property<int?>("Height")
                        .HasColumnType("int");

                    b.Property<int?>("Inactive")
                        .HasColumnType("int");

                    b.Property<int?>("Length")
                        .HasColumnType("int");

                    b.Property<string>("ManufacturerCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PackageDeposit")
                        .HasColumnType("int");

                    b.Property<int>("Product")
                        .HasColumnType("int");

                    b.Property<string>("SalesCategory")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SupplierCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<string>("Unit")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Updated")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("VatRate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("VatRateFlag")
                        .HasColumnType("int");

                    b.Property<int?>("Width")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CompanyDataId");

                    b.ToTable("WebApiProduct");
                });

            modelBuilder.Entity("Sellasist_Optima.ModelsWebApi.Products.Attribute", b =>
                {
                    b.HasOne("Sellasist_Optima.WebApiModels.WebApiProduct", null)
                        .WithMany("Attributes")
                        .HasForeignKey("WebApiProductId");
                });

            modelBuilder.Entity("Sellasist_Optima.ModelsWebApi.Products.Price", b =>
                {
                    b.HasOne("Sellasist_Optima.WebApiModels.WebApiProduct", null)
                        .WithMany("Prices")
                        .HasForeignKey("WebApiProductId");
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

            modelBuilder.Entity("Sellasist_Optima.WebApiModels.WebApiProduct", b =>
                {
                    b.HasOne("Sellasist_Optima.ModelsWebApi.Products.CompanyData", "CompanyData")
                        .WithMany()
                        .HasForeignKey("CompanyDataId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CompanyData");
                });

            modelBuilder.Entity("Sellasist_Optima.WebApiModels.WebApiProduct", b =>
                {
                    b.Navigation("Attributes");

                    b.Navigation("Prices");
                });
#pragma warning restore 612, 618
        }
    }
}
