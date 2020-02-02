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
        /// Inserts account data into the database if it is unique otherwise ignores it.
        /// </summary>
        /// <param name="accounts">Account data to insert.</param>
        /// <returns>DbResult detailing the result of the insert operation.</returns>
        public DbResult InsertEntries(ICollection<CustomerAccount> accounts) {
            using (var context = new MeterReaderContext()) {
                int initialRowCount = context.Accounts.Count();

                context.BulkMerge(accounts);

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
