using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ENSEK_Meter_Reader.CrudBackend.Database {
    /// <summary>
    /// Container class for the results of performing an insert or update to a database table.
    /// </summary>
    public class DbResult {
        public int InsertCount;
        public int UpdateCount;
        public int ErrorCount;
    }
}
