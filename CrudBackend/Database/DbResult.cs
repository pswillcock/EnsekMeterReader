using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ENSEK_Meter_Reader.CrudBackend.Database {
    /// <summary>
    /// Container class for the results of performing an insert to a database table.
    /// </summary>
    public class DbResult {
        public int InsertCount { get; set; }
        public int ErrorCount { get; set; }
    }
}
