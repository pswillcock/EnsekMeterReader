using System.Collections.Generic;

namespace ENSEK_Meter_Reader.CrudBackend.Csv {
    /// <summary>
    /// Countainer class for the result of extracting data from a CSV file.
    /// </summary>
    /// <typeparam name="T">Output object type extracted from the CSV file.</typeparam>
    public class CsvParseResult<T> {
        public int LineParseSuccessCount { get; set; }
        public int LineParseFailureCount { get; set; }
        public ICollection<T> Data { get; set; }
    }
}