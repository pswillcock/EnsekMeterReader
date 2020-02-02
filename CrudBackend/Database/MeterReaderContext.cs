using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ENSEK_Meter_Reader_Server.Models {
    public class MeterReaderContext : DbContext {
        public virtual DbSet<CustomerAccount> Accounts { get; set; }
        public virtual DbSet<MeterReading> MeterReadings { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options) {
            if (!options.IsConfigured) {
#if DEBUG
                string rootPath = AppDomain.CurrentDomain.BaseDirectory + @"..\..\..";
                options.UseSqlServer(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = " + rootPath + @"\CrudBackend\Database\ENSEK_Meter_Reader_LocalDb.mdf; Initial Catalog=meterReaderDB; Integrated Security = True; Connect Timeout = 30;");
#else
                options.UseSqlServer(@"Data Source=ensekmeterreader-paulwillcockdbserver.database.windows.net;Initial Catalog=ENSEKMeterReader-PaulWillcock_db;User ID=paul.willcock;Password=L3mm1ng5!;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
#endif
            }
        }
    }
}