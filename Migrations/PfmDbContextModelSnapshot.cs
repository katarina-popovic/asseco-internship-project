﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TransactionAPI.Database.Repositories;

#nullable disable

namespace TransactionAPI.Migrations
{
    [DbContext(typeof(PfmDbContext))]
    partial class PfmDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TransactionAPI.Database.Entities.CategoryEntity", b =>
                {
                    b.Property<string>("Code")
                        .HasMaxLength(5)
                        .HasColumnType("character varying(5)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<string>("ParentCode")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("character varying(5)");

                    b.HasKey("Code");

                    b.ToTable("Category", (string)null);
                });

            modelBuilder.Entity("TransactionAPI.Database.Entities.SplitEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<double>("Amount")
                        .HasColumnType("double precision");

                    b.Property<string>("CategoryCode")
                        .IsRequired()
                        .HasColumnType("character varying(5)");

                    b.Property<string>("TransactionId")
                        .IsRequired()
                        .HasColumnType("character varying(10)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryCode");

                    b.HasIndex("TransactionId");

                    b.ToTable("Split", (string)null);
                });

            modelBuilder.Entity("TransactionAPI.Database.Entities.TransactionEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<double>("Amount")
                        .HasColumnType("double precision");

                    b.Property<string>("BeneficiaryName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<string>("CatCode")
                        .HasMaxLength(5)
                        .HasColumnType("character varying(5)");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("character varying(5)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<char>("Direction")
                        .HasColumnType("character(1)");

                    b.Property<string>("Kind")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("character varying(5)");

                    b.Property<int>("Mcc")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Transaction", (string)null);
                });

            modelBuilder.Entity("TransactionAPI.Database.Entities.SplitEntity", b =>
                {
                    b.HasOne("TransactionAPI.Database.Entities.CategoryEntity", "Category")
                        .WithMany("SplitCategoryList")
                        .HasForeignKey("CategoryCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TransactionAPI.Database.Entities.TransactionEntity", "Transaction")
                        .WithMany("SplitTransactionList")
                        .HasForeignKey("TransactionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Transaction");
                });

            modelBuilder.Entity("TransactionAPI.Database.Entities.CategoryEntity", b =>
                {
                    b.Navigation("SplitCategoryList");
                });

            modelBuilder.Entity("TransactionAPI.Database.Entities.TransactionEntity", b =>
                {
                    b.Navigation("SplitTransactionList");
                });
#pragma warning restore 612, 618
        }
    }
}