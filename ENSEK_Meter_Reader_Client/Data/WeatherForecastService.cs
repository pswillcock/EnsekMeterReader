using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ENSEK_Meter_Reader_Server.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace ENSEK_Meter_Reader_Client.Data {
    public class WeatherForecastService {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public WeatherForecast[] GetForecasts(DateTime startDate) {
            var rng = new Random();

            WeatherForecast[] forecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast {
                Date = startDate.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            }).ToArray();

            return forecasts;
        }

        public void TestLocalDb() {
            using (var db = new MeterReadingContext()) {
                bool connect = db.Database.CanConnect();

                try {
                    var databaseCreator = (db.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator);
                    databaseCreator.CreateTables();
                }
                catch (SqlException e) {
                    // Ignore
                }
                db.Database.OpenConnection();
                db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Accounts ON");
                db.Add(new CustomerAccount {
                    AccountId = 1234,
                    FirstName = "John",
                    LastName = "Doe"
                });
                db.SaveChanges();
                db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Accounts OFF");
                db.Database.CloseConnection();
            }
        }
    }
}
