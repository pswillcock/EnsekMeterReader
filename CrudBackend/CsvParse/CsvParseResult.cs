using System.Collections.Generic;

namespace ENSEK_Meter_Reader.CrudBackend.CsvParse {
    /// <summary>
    /// Countainer class for the result of extracting data from a CSV file.
    /// </summary>
    /// <typeparam name="T">Output object type extracted from the CSV file.</typeparam>
    public class CsvParseResult<T> {
        public int ErrorCount { get; set; }
        public ICollection<T> Data { get; set; }
    }
}