using ENSEK_Meter_Reader_Server.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ENSEK_Meter_Reader.CrudBackend.Csv {
    /// <summary>
    /// Extracts smart meter reading data from a provided CSV file.
    /// </summary>
    public class MeterReadingCsvParser : ICsvParser<MeterReading> {

        private const string ACCOUNT_HEADING = "AccountId";
        private const string METER_READING_TIME_HEADING = "MeterReadingDateTime";
        private const string METER_VALUE_HEADING = "MeterReadValue";
        private HashSet<string> ValidCsvHeadingNames = new HashSet<string> {
            ACCOUNT_HEADING, METER_READING_TIME_HEADING, METER_VALUE_HEADING
        };

        // Meter readings must consist of exactly 5 numeric characters.
        private Regex meterReadingPattern = new Regex("^\\d{5}$");

        /// <summary>
        /// Extracts smart meter reading data from a provided CSV file. Expected format is the parameters names on the first row and the associated values in the same columns.
        /// </summary>
        /// <param name="csvFile">TextReader that reads smart meter data from the CSV file.</param>
        /// <returns></returns>
        public CsvParseResult<MeterReading> ParseCsvFile(TextReader csvFile) {
            int lineParseSuccessCount = 0;
            int lineParseFailCount = 0;
            var meterReadings = new List<MeterReading>();

            string headerRow = csvFile.ReadLine();
            string[] meterReadingColumnHeadings = headerRow.Split(',');

            bool columnHeadingsValid = VerifyColumnHeadings(meterReadingColumnHeadings);
            if (columnHeadingsValid) {
                string line = csvFile.ReadLine();
                while (line != null) {
                    MeterReading meterReading = new MeterReading();

                    string[] meterReadingValues = line.Split(',');
                    if (meterReadingValues.Length == meterReadingColumnHeadings.Length) {
                        try {
                            for (int i = 0; i < meterReadingValues.Length; i++) {
                                string heading = meterReadingColumnHeadings[i];
                                string value = meterReadingValues[i];

                                switch (heading) {
                                    case ACCOUNT_HEADING:
                                        meterReading.AccountId = Int32.Parse(value);

                                        break;

                                    case METER_READING_TIME_HEADING:
                                        bool dateTimeParseSuccess = meterReading.StoreTimestampFromString(value);
                                        if (!dateTimeParseSuccess) {
                                            throw new FormatException(
                                                String.Format("Invalid date/time format {0}.", value)
                                            );
                                        }

                                        break;

                                    case METER_VALUE_HEADING:
                                        bool meterReadingFormatValid = meterReadingPattern.IsMatch(value);
                                        if (!meterReadingFormatValid) {
                                            throw new FormatException(
                                                String.Format("Invalid meter reading format {0}.", value)
                                            );
                                        }

                                        meterReading.MeterValue = Int32.Parse(value);
                                        break;

                                    default:
                                        throw new FormatException("Invalid CSV column heading names.");
                                }
                            }

                            lineParseSuccessCount += 1;
                            meterReadings.Add(meterReading);
                        }
                        catch {
                            lineParseFailCount += 1;
                        }
                    }

                    else {
                        lineParseFailCount += 1;
                    }

                    line = csvFile.ReadLine();
                }
            }

            return new CsvParseResult<MeterReading> {
                LineParseSuccessCount = lineParseSuccessCount,
                LineParseFailureCount = lineParseFailCount,
                Data = meterReadings
            };
        }

        /// <summary>
        /// Verify that an array of column headings matches that specified set of valid names. Ordering does not affect validity. 
        /// </summary>
        /// <param name="columnHeadings">Column headings to verify.</param>
        /// <returns></returns>
        private bool VerifyColumnHeadings(string[] columnHeadings) {
            bool validQuantity = columnHeadings.Length == ValidCsvHeadingNames.Count;

            if (!validQuantity) {
                return false;
            }

            HashSet<string> headingsSet = new HashSet<string>(columnHeadings);
            return headingsSet.SetEquals(ValidCsvHeadingNames);
        }
    }
}
