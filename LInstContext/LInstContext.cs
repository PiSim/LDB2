using Microsoft.EntityFrameworkCore;

namespace LInst
{
    public class LInstContext : DbContext
    {
        #region Constructors

        public LInstContext()
        {
        }

        #endregion Constructors

        #region Properties

        public DbSet<CalibrationFile> CalibrationFiles { get; set; }
        public DbSet<CalibrationReportProperty> CalibrationReportProperties { get; set; }
        public DbSet<CalibrationReportReference> CalibrationReportReferences { get; set; }
        public DbSet<CalibrationReport> CalibrationReports { get; set; }
        public DbSet<CalibrationResult> CalibrationResults { get; set; }
        public DbSet<InstrumentFile> InstrumentFiles { get; set; }
        public DbSet<InstrumentMaintenanceEvent> InstrumentMaintenanceEvents { get; set; }
        public DbSet<InstrumentProperty> InstrumentProperties { get; set; }
        public DbSet<Instrument> Instruments { get; set; }
        public DbSet<InstrumentType> InstrumentTypes { get; set; }
        public DbSet<InstrumentUtilizationArea> InstrumentUtilizationAreas { get; set; }
        public DbSet<OrganizationRoleMapping> OrganizationRoleMappings { get; set; }
        public DbSet<OrganizationRole> OrganizationRoles { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<PersonRoleMapping> PersonRoleMappings { get; set; }
        public DbSet<PersonRole> PersonRoles { get; set; }
        public DbSet<UserRoleMapping> UserRoleMappings { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<User> Users { get; set; }

        #endregion Properties

        #region Methods

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=192.168.1.22;user id=root;Pwd=dicembre19;persistsecurityinfo=True;database=linstdb_dev;port=3306;SslMode=none");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CalibrationReportReference>()
                .HasKey(crr => new { crr.CalibrationReportID, crr.InstrumentID });

            modelBuilder.Entity<CalibrationReportReference>()
                .HasOne(crr => crr.CalibrationReport)
                .WithMany(cr => cr.CalibrationReportReferences)
                .HasForeignKey(cr => cr.CalibrationReportID)
                .HasConstraintName("FK_CalibrationReportReference_CalRep_CalRepID");

            modelBuilder.Entity<CalibrationReportReference>()
                .HasOne(crr => crr.Instrument)
                .WithMany(ins => ins.CalibrationsAsReference)
                .HasForeignKey(crr => crr.InstrumentID);

            modelBuilder.Entity<Instrument>()
                .HasIndex(ins => ins.Code);

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

            modelBuilder.Entity<OrganizationRoleMapping>()
                .HasOne(orm => orm.Organization)
                .WithMany(org => org.RoleMappings)
                .HasForeignKey(orm => orm.OrganizationID);

            modelBuilder.Entity<OrganizationRoleMapping>()
                .HasOne(orm => orm.OrganizationRole)
                .WithMany(orr => orr.OrganizationRoleMappings)
                .HasForeignKey(orm => orm.OrganizationRoleID);

            modelBuilder.Entity<PersonRoleMapping>()
                .HasOne(prm => prm.Person)
                .WithMany(per => per.RoleMappings)
                .HasForeignKey(prm => prm.PersonID);

            modelBuilder.Entity<PersonRoleMapping>()
                .HasOne(prm => prm.Role)
                .WithMany(prr => prr.RoleMappings)
                .HasForeignKey(prm => prm.RoleID);

            modelBuilder.Entity<UserRoleMapping>()
                .HasOne(urm => urm.User)
                .WithMany(usr => usr.RoleMappings)
                .HasForeignKey(urm => urm.UserID);

            modelBuilder.Entity<UserRoleMapping>()
                .HasOne(urm => urm.UserRole)
                .WithMany(ur => ur.UserMappings)
                .HasForeignKey(urm => urm.UserRoleID);
        }

        #endregion Methods
    }
}