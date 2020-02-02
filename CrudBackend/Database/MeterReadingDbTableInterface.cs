using ENSEK_Meter_Reader_Server.Models;
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
        /// Asynchronously inserts meter reading data into the database if it is unique otherwise ignores it.
        /// </summary>
        /// <param name="meterReadings">Meter reading data to insert.</param>
        /// <returns>DbResult detailing the result of the insert operation.</returns>
        public async Task<DbResult> InsertEntriesAsync(ICollection<MeterReading> meterReadings) {
            using (var context = new MeterReaderContext()) {
                IEnumerable<MeterReading> validReadings = RemoveInvalidReadings(meterReadings, context);

                try {
                    foreach (MeterReading reading in validReadings) {
                        reading.Id = GenerateMeterReadingId(reading);
                    }

                    int initialRowCount = context.MeterReadings.Count();

                    await context.BulkMergeAsync(validReadings);

                    int finalRowCount = context.MeterReadings.Count();
                    int insertCount = finalRowCount - initialRowCount;

                    return new DbResult {
                        InsertCount = insertCount,
                        ErrorCount = meterReadings.Count - insertCount
                    };
                }
                catch (Exception e) {
                    return new DbResult {
                        InsertCount = 0,
                        ErrorCount = meterReadings.Count
                    };
                }  
            }
        }

        /// <summary>
        /// Removes all entries from the meter readings table.
        /// </summary>
        /// <returns>True if all entries were deleted else false.</returns>
        public async Task<bool> ClearTableAsync() {
            using (var context = new MeterReaderContext()) {
                await context.BulkDeleteAsync(context.MeterReadings);

                return context.MeterReadings.Count() == 0;
            }
        }

        /// <summary>
        /// Returns the filtered subset of meter readings that have a valid Account ID.
        /// </summary>
        /// <param name="readings">Meter readings to filter.</param>
        /// <returns>Valid meter readings subset.</returns>
        private IEnumerable<MeterReading> RemoveInvalidReadings(ICollection<MeterReading> readings, MeterReaderContext context) {
            IEnumerable<string> actualAccountIds = context.Accounts.Select(account => account.Id);

            return readings.Where(reading => actualAccountIds.Contains(reading.AccountId));
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
