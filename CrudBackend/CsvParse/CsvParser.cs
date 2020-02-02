using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ENSEK_Meter_Reader.CrudBackend.CsvParse {
    /// <summary>
    /// Interface to be implemented for classes which extract data from CSV files and convert to C# objects.
    /// </summary>
    /// <typeparam name="T">Output type for the data extraction.</typeparam>
    public abstract class CsvParser<T> where T : new() {

        private HashSet<string> ValidCsvHeadings = new HashSet<string>();

        protected Dictionary<string, Action<T, string>> PropertySetters;

        /// <summary>
        /// Converts a CSV file to a collection of C# objects and records the number of successfully and unsuccessfully parsed lines.
        /// </summary>
        /// <param name="csvFile">TextReader that reads data from the CSV file.</param>
        /// <returns>CsvDataExtractionResult containing extracted data and error count.</returns>
        public CsvParseResult<T> ParseCsvFile(TextReader csvFile) {
            string headerRow = csvFile.ReadLine();
            string[] meterReadingColumnHeadings = headerRow.Split(',');

            bool columnHeadingsValid = VerifyColumnHeadings(meterReadingColumnHeadings);
            if (columnHeadingsValid) {
                return GenerateObjects(csvFile, meterReadingColumnHeadings);
            }
            else {
                return new CsvParseResult<T> {
                    ErrorCount = 0,
                    Data = new List<T>()
                };
            }
        }

        /// <summary>
        /// Verify that an array of column headings matches that specified set of valid names. Ordering does not affect validity. 
        /// </summary>
        /// <param name="columnHeadings">Column headings to verify.</param>
        /// <returns>True if the column headings are valid else false.</returns>
        private bool VerifyColumnHeadings(string[] columnHeadings) {
            if (ValidCsvHeadings.Count == 0) {
                ValidCsvHeadings = new HashSet<string>(PropertySetters.Keys);
            }

            bool validQuantity = columnHeadings.Length == ValidCsvHeadings.Count;

            if (!validQuantity) {
                return false;
            }

            HashSet<string> headingsSet = new HashSet<string>(columnHeadings);
            return headingsSet.SetEquals(ValidCsvHeadings);
        }

        /// <summary>
        /// Converts data portion of a CSV file into C# objects based upon the already read in column headings.
        /// </summary>
        /// <param name="csvFile">TextReader streaming CSV data.</param>
        /// <param name="meterReadingColumnHeadings">Array of column headings from first line of the CSV file.</param>
        /// <returns></returns>
        private CsvParseResult<T> GenerateObjects(TextReader csvFile, string[] meterReadingColumnHeadings) {
            List<T> objects = new List<T>();
            int lineParseFailCount = 0;

            string line = csvFile.ReadLine();
            while (line != null) {
                T t = new T();
                string[] meterReadingValues = line.Split(',');

                try {
                    if (meterReadingValues.Length != meterReadingColumnHeadings.Length) {
                        throw new FormatException("Mismatched number of columns in CSV file line.");
                    }

                    for (int i = 0; i < meterReadingValues.Length; i++) {
                        string heading = meterReadingColumnHeadings[i];
                        string value = meterReadingValues[i];

                        PropertySetters[heading](t, value);
                    }

                    objects.Add(t);
                }

                catch {
                    lineParseFailCount += 1;
                }

                line = csvFile.ReadLine();
            }

            return new CsvParseResult<T> {
                ErrorCount = lineParseFailCount,
                Data = objects
            };
        }
    }
}
