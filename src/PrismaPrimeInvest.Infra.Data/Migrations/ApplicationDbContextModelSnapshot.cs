﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PrismaPrimeInvest.Infra.Data.Contexts;

#nullable disable

namespace PrismaPrimeInvest.Infra.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PrismaPrimeInvest.Domain.Entities.Invest.Fund", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.HasKey("Id");

                    b.ToTable("Fund");
                });

            modelBuilder.Entity("PrismaPrimeInvest.Domain.Entities.Invest.FundBestDay", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("BestDay")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("FundId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("FundId");

                    b.ToTable("FundBestDay");
                });

            modelBuilder.Entity("PrismaPrimeInvest.Domain.Entities.Invest.FundDailyValue", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("FundId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("MaxValue")
                        .HasColumnType("float");

                    b.Property<double>("MinValue")
                        .HasColumnType("float");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("FundId");

                    b.ToTable("FundDailyValue");
                });

            modelBuilder.Entity("PrismaPrimeInvest.Domain.Entities.Invest.FundPayment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("AnalysisMonth")
                        .HasColumnType("datetime2");

                    b.Property<double>("AverageValue")
                        .HasColumnType("float");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<double>("Dividend")
                        .HasColumnType("float");

                    b.Property<Guid>("FundId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("MaximumValue")
                        .HasColumnType("float");

                    b.Property<DateTime>("MaximumValueDate")
                        .HasColumnType("datetime2");

                    b.Property<double>("MinimumValue")
                        .HasColumnType("float");

                    b.Property<DateTime>("MinimumValueDate")
                        .HasColumnType("datetime2");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("FundId");

                    b.ToTable("FundPayment");
                });

            modelBuilder.Entity("PrismaPrimeInvest.Domain.Entities.User.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Document")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("PrismaPrimeInvest.Domain.Entities.Invest.FundBestDay", b =>
                {
                    b.HasOne("PrismaPrimeInvest.Domain.Entities.Invest.Fund", "Fund")
                        .WithMany("FundBestDays")
                        .HasForeignKey("FundId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Fund");
                });

            modelBuilder.Entity("PrismaPrimeInvest.Domain.Entities.Invest.FundDailyValue", b =>
                {
                    b.HasOne("PrismaPrimeInvest.Domain.Entities.Invest.Fund", "Fund")
                        .WithMany("FundDailyValue")
                        .HasForeignKey("FundId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Fund");
                });

            modelBuilder.Entity("PrismaPrimeInvest.Domain.Entities.Invest.FundPayment", b =>
                {
                    b.HasOne("PrismaPrimeInvest.Domain.Entities.Invest.Fund", "Fund")
                        .WithMany("FundPayments")
                        .HasForeignKey("FundId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Fund");
                });

            modelBuilder.Entity("PrismaPrimeInvest.Domain.Entities.Invest.Fund", b =>
                {
                    b.Navigation("FundBestDays");

                    b.Navigation("FundDailyValue");

                    b.Navigation("FundPayments");
                });
#pragma warning restore 612, 618
        }
    }
}