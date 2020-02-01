using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ENSEK_Meter_Reader.CrudBackend {
    public class CsvUploadResult {
        public int RowInsertCount { get; }
        public int RowUpdateCount { get; }
        public int ErrorCount { get; }
    }
}
