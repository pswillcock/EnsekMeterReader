using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ENSEK_Meter_Reader.CrudBackend {

    /// <summary>
    /// Service provider for ingesting comma separated value files and populating the SQL database with matching data.
    /// </summary>
    public class CsvParserService {

        public bool LoadCsvFile(TextReader csvStream) {
            return false;
        }
    }
}
