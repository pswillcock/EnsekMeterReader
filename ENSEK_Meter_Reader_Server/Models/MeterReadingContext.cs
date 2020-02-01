using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ENSEK_Meter_Reader_Server.Models {
    public class MeterReadingContext : DbContext {
        public virtual DbSet<CustomerAccount> Accounts { get; set; }
        public virtual DbSet<MeterReading> MeterReadings { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options) {
            if (!options.IsConfigured) {
                options.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ENSEK_Meter_Reader_LocalDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }
    }
}