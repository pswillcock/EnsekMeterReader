using System.Collections.Generic;

namespace ENSEK_Meter_Reader.CrudBackend {
    /// <summary>
    /// Countainer class for the result of extracting data from a CSV file.
    /// </summary>
    /// <typeparam name="T">Output object type extracted from the CSV file.</typeparam>
    public class CsvDataExtractionResult<T> {
        public int LineParseSuccessCount;
        public int LineParseFailureCount;
        public ICollection<T> Data;
    }
}