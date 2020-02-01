using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ENSEK_Meter_Reader.CrudBackend.Database {

    /// <summary>
    /// Interface to be implemented by classes which perform SQL operations on a single database table.
    /// </summary>
    /// <typeparam name="T">Entity Framework model type for the database table.</typeparam>
    interface IDbTableInterface<T> {
        DbResult UpsertEntries(ICollection<T> data);
    }
}
