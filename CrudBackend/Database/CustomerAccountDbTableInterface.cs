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
                DatabaseFacade db = context.Database;
 
                try {
                    db.OpenConnection();
                    db.ExecuteSqlRaw("SET IDENTITY_INSERT Accounts ON");
                    context.Accounts.AddRange(accounts);
                    context.SaveChanges();
                }
                finally {
                    db.ExecuteSqlRaw("SET IDENTITY_INSERT Accounts OFF");
                    db.CloseConnection();
                }

                int insertCount = context.ChangeTracker.Entries<CustomerAccount>()
                    .Count(e => e.State == EntityState.Added);

                return new DbResult {
                    InsertCount = insertCount,
                    ErrorCount = accounts.Count - insertCount
                };
            }
        }
    }
}
