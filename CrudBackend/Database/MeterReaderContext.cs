using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ENSEK_Meter_Reader_Server.Models {
    public class MeterReaderContext : DbContext {
        public virtual DbSet<CustomerAccount> Accounts { get; set; }
        public virtual DbSet<MeterReading> MeterReadings { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options) {
            if (!options.IsConfigured) {
                using (StreamReader appSettingsFile = File.OpenText(@"appsettings.json")) {
                    string text = appSettingsFile.ReadToEnd();
                    JObject jObject = JObject.Parse(text);

                    JToken connectionStrings = jObject["ConnectionStrings"];
#if DEBUG                
                    string localConnectionString = connectionStrings["LocalConnection"].ToString();
                    string rootPath = AppDomain.CurrentDomain.BaseDirectory.Split("\\bin")[0];
                    localConnectionString = localConnectionString.Replace("<root>", rootPath);

                    options.UseSqlServer(localConnectionString);
#else
                    string remoteConnectionString = connectionStrings["RemoteConnection"].ToString();
                    options.UseSqlServer(remoteConnectionString);
#endif                    
                }
            }
        }
    }
}