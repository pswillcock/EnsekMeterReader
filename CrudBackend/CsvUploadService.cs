using ENSEK_Meter_Reader.CrudBackend.CsvParse;
using ENSEK_Meter_Reader.CrudBackend.Database;
using ENSEK_Meter_Reader_Server.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ENSEK_Meter_Reader.CrudBackend {

    /// <summary>
    /// Service provider for ingesting comma separated value files and populating the SQL database with matching data.
    /// </summary>
    public class CsvUploadService {

        /// <summary>
        /// Extract data from a CSV file and populate the associated database table.
        /// </summary>
        /// <param name="csvFile">TextReader object which reads in the contents of the CSV file.</param>
        /// <returns>CsvUploadResult detailing the number of entries which succeeded and failed to load.</returns>
        public CsvUploadResult UploadCsvFile(TextReader csvFile) {
            MeterReadingCsvParser parser = new MeterReadingCsvParser();
            CsvParseResult<MeterReading> parseResult = parser.ParseCsvFile(csvFile);

            MeterReadingDbTableInterface meterReadingTable = new MeterReadingDbTableInterface();
            DbResult insertResult = meterReadingTable.InsertEntries(parseResult.Data);

            return new CsvUploadResult {
                RowInsertCount = insertResult.InsertCount,
                ErrorCount = parseResult.ErrorCount + insertResult.ErrorCount
            };
        }
    }
}
