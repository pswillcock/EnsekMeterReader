﻿using ENSEK_Meter_Reader_Server.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ENSEK_Meter_Reader.CrudBackend.Database {
    public class MeterReadingDbTableInterface : IDbTableInterface<MeterReading> {
        /// <summary>
        /// Inserts meter reading data into the database if it is unique otherwise ignores it.
        /// </summary>
        /// <param name="meterReadings">Meter reading data to insert.</param>
        /// <returns>DbResult detailing the result of the insert operation.</returns>
        public DbResult InsertEntries(ICollection<MeterReading> meterReadings) {
            using (var context = new MeterReaderContext()) {
                foreach(MeterReading meterReading in meterReadings) {
                    meterReading.Id = GenerateMeterReadingId(meterReading);
                }

                context.MeterReadings.AddRange(meterReadings);
                context.SaveChanges();

                int insertCount = context.ChangeTracker.Entries<MeterReading>()
                    .Count(e => e.State == EntityState.Added);

                return new DbResult {
                    InsertCount = insertCount,
                    ErrorCount = meterReadings.Count - insertCount
                };
            }
        }

        /// <summary>
        /// Generate deterministic ID for a meter reading based on the account ID and timestamp. Prevents duplicate readings from being stored.
        /// </summary>
        /// <param name="reading">Meter reading to generate an ID for.</param>
        /// <returns>Meter reading ID.</returns>
        private string GenerateMeterReadingId(MeterReading reading) {
            using (SHA256 idGenerator = SHA256.Create()) {
                byte[] accountBytes = Encoding.ASCII.GetBytes(reading.AccountId);
                byte[] timestampBytes = BitConverter.GetBytes(reading.Timestamp);

                byte[] readingBytes = new byte[accountBytes.Length + timestampBytes.Length];
                accountBytes.CopyTo(readingBytes, 0);
                timestampBytes.CopyTo(readingBytes, accountBytes.Length);

                byte[] idBytes = idGenerator.ComputeHash(readingBytes);
                return Convert.ToBase64String(idBytes);
            }
        }
    }
}