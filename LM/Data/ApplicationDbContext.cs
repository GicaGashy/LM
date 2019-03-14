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




        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Team> Teams{ get; set; }
        public DbSet<AppUser> AppUsers{ get; set; }
        public DbSet<Tipi> Tipies{ get; set; }
        public DbSet<TechArea> TechAreas{ get; set; }
        public DbSet<Software> Softwares{ get; set; }
        public DbSet<SoftwareTeam> SoftwareTeams{ get; set; }
    }
}
