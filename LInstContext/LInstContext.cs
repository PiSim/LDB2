using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LInst
{
    public class LInstContext : DbContext
    {
        public LInstContext()
        {

        }

        public DbSet<CalibrationFile> CalibrationFiles { get; set; }
        public DbSet<CalibrationReport> CalibrationReports { get; set; }
        public DbSet<CalibrationResult> CalibrationResults { get; set; }
        public DbSet<Instrument> Instruments { get; set; }
        public DbSet<InstrumentFile> InstrumentFiles { get; set; }
        public DbSet<InstrumentProperty> InstrumentProperties { get; set; }
        public DbSet<InstrumentMaintenanceEvent> InstrumentMaintenanceEvents { get; set; }
        public DbSet<InstrumentType> InstrumentTypes { get; set; }
        public DbSet<InstrumentUtilizationArea> InstrumentUtilizationAreas { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<OrganizationRole> OrganizationRoles { get; set; }
        public DbSet<OrganizationRoleMapping> OrganizationRoleMappings { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<PersonRole> PersonRoles { get; set; }
        public DbSet<PersonRoleMapping> PersonRoleMappings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=192.168.1.22;user id=root;Pwd=dicembre19;persistsecurityinfo=True;database=linstdb_dev;port=3306;SslMode=none");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Instrument>()
                .Property(ip => ip.IsInService)
                .HasDefaultValue(true);

            modelBuilder.Entity<Instrument>()
                .Property(ip => ip.IsUnderControl)
                .HasDefaultValue(false);

            modelBuilder.Entity<InstrumentProperty>()
                .Property(ip => ip.Value)
                .HasDefaultValue(0);

            modelBuilder.Entity<InstrumentProperty>()
                .Property(ip => ip.IsCalibrationProperty)
                .HasDefaultValue(false);
        }
    }
}
