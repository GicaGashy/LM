using System;
using System.Collections.Generic;
using System.Text;
using LM.Models.LM;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LM.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Department>().HasData(new Department
            {
                DepartmentId = 1,
                Name = "Infrastructure",
                Description = "Infrastructure stuff"
            });

            builder.Entity<Purpose>().HasData(new Purpose {
                PurposeId = 1,
                Name = "IT",
                Description = "Purpose for IT only"
            });

            builder.Entity<Purpose>().HasData(new Purpose
            {
                PurposeId = 2,
                Name = "Business",
                Description = "Purpose for Business only"
            });

            builder.Entity<Purpose>().HasData(new Purpose
            {
                PurposeId = 3,
                Name = "Other",
                Description = "Other"
            });

            builder.Entity<SoftwareTeam>()
                .HasKey(t => new { t.SoftwareId, t.TeamId} );

            builder.Entity<SoftwareTeam>()
                .HasOne(s => s.Software)
                .WithMany(st => st.SoftwareTeams)
                .HasForeignKey(s => s.SoftwareId);

            builder.Entity<SoftwareTeam>()
                .HasOne(t => t.Team)
                .WithMany(st => st.SoftwareTeams)
                .HasForeignKey(t => t.TeamId);
            
            
            builder.Entity<SoftwareBusinessOwnerTeam>()
                .HasKey(sbo => new { sbo.SoftwareId, sbo.BusinessOwnerTeamId });

            builder.Entity<SoftwareBusinessOwnerTeam>()
                .HasOne<Software>(s => s.Software)
                .WithMany(sbo => sbo.SoftwareBusinessOwnerTeams)
                .HasForeignKey(sbo => sbo.SoftwareId);

            builder.Entity<SoftwareBusinessOwnerTeam>()
                .HasOne<Team>(t => t.BusinessOwnerTeam)
                .WithMany(sbo => sbo.SoftwareBusinessOwnerTeams)
                .HasForeignKey(sbo => sbo.BusinessOwnerTeamId);
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Team> Teams{ get; set; }
        public DbSet<AppUser> AppUsers{ get; set; }
        public DbSet<Tipi> Tipies{ get; set; }
        public DbSet<TechArea> TechAreas{ get; set; }
        public DbSet<Software> Softwares{ get; set; }
        public DbSet<SoftwareTeam> SoftwareTeams{ get; set; }
        public DbSet<SoftwareBusinessOwnerTeam> SoftwareBusinessOwnerTeams{ get; set; }
        public DbSet<Purpose> Purposes{ get; set; }
        public DbSet<Reseller> Reseller{ get; set; }
    }
}
