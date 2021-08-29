﻿// <auto-generated />
using System;
using LinkExtractor.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LinkExtractor.DAL.Migrations
{
    [DbContext(typeof(LinkExtractorDbContext))]
    partial class LinkExtractorDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.8")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("LinkExtractor.Models.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(320)
                        .HasColumnType("nvarchar(320)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("TeamId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TeamId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("LinkExtractor.Models.EmployeeWorkshift", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<int>("WorkshiftId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("WorkshiftId");

                    b.ToTable("EmployeeWorkshifts");
                });

            modelBuilder.Entity("LinkExtractor.Models.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.HasKey("Id");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("LinkExtractor.Models.Tender", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("EmployeeWorkshiftId")
                        .HasColumnType("int");

                    b.Property<string>("Url")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeWorkshiftId");

                    b.ToTable("Tenders");
                });

            modelBuilder.Entity("LinkExtractor.Models.Workshift", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Workshifts");
                });

            modelBuilder.Entity("LinkExtractor.Models.Employee", b =>
                {
                    b.HasOne("LinkExtractor.Models.Team", "Team")
                        .WithMany()
                        .HasForeignKey("TeamId");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("LinkExtractor.Models.EmployeeWorkshift", b =>
                {
                    b.HasOne("LinkExtractor.Models.Employee", "Employee")
                        .WithMany("EmployeeWorkshifts")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LinkExtractor.Models.Workshift", "Workshift")
                        .WithMany("EmployeeWorkshifts")
                        .HasForeignKey("WorkshiftId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("Workshift");
                });

            modelBuilder.Entity("LinkExtractor.Models.Tender", b =>
                {
                    b.HasOne("LinkExtractor.Models.EmployeeWorkshift", "EmployeeWorkshift")
                        .WithMany("Tenders")
                        .HasForeignKey("EmployeeWorkshiftId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EmployeeWorkshift");
                });

            modelBuilder.Entity("LinkExtractor.Models.Employee", b =>
                {
                    b.Navigation("EmployeeWorkshifts");
                });

            modelBuilder.Entity("LinkExtractor.Models.EmployeeWorkshift", b =>
                {
                    b.Navigation("Tenders");
                });

            modelBuilder.Entity("LinkExtractor.Models.Workshift", b =>
                {
                    b.Navigation("EmployeeWorkshifts");
                });
#pragma warning restore 612, 618
        }
    }
}
