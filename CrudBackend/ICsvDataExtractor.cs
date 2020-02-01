using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ENSEK_Meter_Reader.CrudBackend {
    /// <summary>
    /// Interface to be implemented for classes which extract data from CSV files and convert to C# objects.
    /// </summary>
    /// <typeparam name="T">Output type for the data extraction.</typeparam>
    interface ICsvDataExtractor<T> {
        /// <summary>
        /// Converts a CSV file to a collection of C# objects and records the number of successfully and unsuccessfully parsed lines.
        /// </summary>
        /// <param name="csvFile">TextReader that reads data from the CSV file.</param>
        /// <returns>CsvDataExtractionResult containing extracted data and success/failure counts.</returns>
        CsvDataExtractionResult<T> ParseCsvFile(TextReader csvFile);
    }
}
