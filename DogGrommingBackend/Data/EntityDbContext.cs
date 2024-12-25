using DogGrommingBackend.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using static DogGrommingBackend.DTOModels.AppointmentDTO;

namespace DogGrommingBackend.Data
{
    public class EntityDbContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the keyless DTO entity
            modelBuilder.Entity<AppointmentDto>().HasNoKey();
        }
        public EntityDbContext(DbContextOptions<EntityDbContext> options) : base(options)
        {

        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public virtual DbSet<AppointmentDto> AppointmentDto{ get; set; }
    }
}
