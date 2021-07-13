using LinkExtractor.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace LinkExtractor.DAL
{
    public class LinkExtractorDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Workshift> Workshifts { get; set; }
        public LinkExtractorDbContext() : base() { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Possibly need to change that
            // optionsBuilder.UseSqlServer("Server=.; Database=DevAidLinks; Trusted_Connection=True");
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json").Build();
            //Real file path - D:\DevelopmentAid\DevelopmentAidLinks\LinkExtractor.DAL\bin\Debug\net5.0
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        }
    }
}
