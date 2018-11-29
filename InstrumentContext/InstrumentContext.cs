using Microsoft.EntityFrameworkCore;
using System;

namespace InstrumentContext
{
    public class InstrumentContext : DbContext
    {

        string connectionString = "metadata=res://*/LabDB.csdl|res://*/LabDB.ssdl|res://*/LabDB.msl;provider=MySql.Data.MySqlClient;provider connection string=&quot;server=192.168.1.22;user id=root;Pwd=dicembre19;persistsecurityinfo=True;database=linstdb;port=3306;SslMode=none&quot;";

        public DbSet<Instrument> Instruments { get; set; }
        public DbSet<Organization> Organizations { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(connectionString);
        }
    }
}
