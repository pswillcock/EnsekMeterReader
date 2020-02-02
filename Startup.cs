using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ENSEK_Meter_Reader.CrudBackend;
using ENSEK_Meter_Reader.CrudBackend.CsvParse;
using ENSEK_Meter_Reader.CrudBackend.Database;
using ENSEK_Meter_Reader_Server.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ENSEK_Meter_Reader {
    public class Startup {

        private const string CUSTOMER_CSV_FILE_PATH = "Resources/Test_Accounts.csv";

        public Startup(IConfiguration configuration) {
            Configuration = configuration;

            InitializeDatabase();
        }
        
        /// <summary>
        /// Creates necessary database tables and seeds initial data.
        /// </summary>
        private void InitializeDatabase() {
            using (var context = new MeterReaderContext()) {
                DatabaseFacade db = context.Database;
                RelationalDatabaseCreator databaseCreator = (RelationalDatabaseCreator)db.GetService<IDatabaseCreator>();

                try {
                    databaseCreator.Create();                   
                }
                catch {
                    // Database already exists
                }
                try {
                    databaseCreator.CreateTables();
                }
                catch {
                    // Tables already exist
                }
                
                using (StreamReader accountsCsv = new StreamReader(CUSTOMER_CSV_FILE_PATH)) {
                    CustomerAccountCsvParser csvParser = new CustomerAccountCsvParser();
                    CsvParseResult<CustomerAccount> accounts = csvParser.ParseCsvFile(accountsCsv);

                    CustomerAccountDbTableInterface accountsTable = new CustomerAccountDbTableInterface();
                    accountsTable.InsertEntries(accounts.Data);
                }
            }
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services) {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<CsvUploadService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints => {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
