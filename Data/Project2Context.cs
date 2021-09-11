using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project2.Areas.Identity.Data;
using Project2.Models;

namespace Project2.Data
{
    public class Project2Context : IdentityDbContext<Project2User>
    {
        public Project2Context (DbContextOptions<Project2Context> options)
            : base(options)
        {
        }

        public DbSet<Project2.Models.Department> Department { get; set; }

        public DbSet<Project2.Models.Doctor> Doctor { get; set; }

        public DbSet<Project2.Models.Patient> Patient { get; set; }

        public DbSet<Project2.Models.Appointment> Appointment { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Appointment>()
            .HasOne<Doctor>(p => p.Doctor)
            .WithMany(p => p.Patients)
            .HasForeignKey(p => p.DoctorId);



            builder.Entity<Appointment>()
            .HasOne<Patient>(p => p.Patient)
            .WithMany(p => p.Doctors)
            .HasForeignKey(p => p.PatientId);



            builder.Entity<Doctor>()
            .HasOne<Department>(p => p.Department)
            .WithMany(p => p.Doctors)
            .HasForeignKey(p => p.DepartmentId);



            builder.Entity<Patient>()
           .HasOne<Department>(p => p.Department)
           .WithMany(p => p.Patients)
           .HasForeignKey(p => p.DepartmentId);
        }
    }
}
