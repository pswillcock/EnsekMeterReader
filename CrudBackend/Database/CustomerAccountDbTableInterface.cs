using ENSEK_Meter_Reader_Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ENSEK_Meter_Reader.CrudBackend.Database {

    /// <summary>
    /// Interface class for the Accounts table.
    /// </summary>
    public class CustomerAccountDbTableInterface : IDbTableInterface<CustomerAccount> {
        /// <summary>
        /// Asynchronously inserts account data into the database if it is unique otherwise ignores it.
        /// </summary>
        /// <param name="accounts">Account data to insert.</param>
        /// <returns>DbResult detailing the result of the insert operation.</returns>
        public async Task<DbResult> InsertEntriesAsync(ICollection<CustomerAccount> accounts) {
            using (var context = new MeterReaderContext()) {
                int initialRowCount = context.Accounts.Count();

                await context.BulkMergeAsync(accounts);

                int finalRowCount = context.Accounts.Count();
                int insertCount = finalRowCount - initialRowCount;

                return new DbResult {
                    InsertCount = insertCount,
                    ErrorCount = accounts.Count - insertCount
                };
            }
        }
    }
}
