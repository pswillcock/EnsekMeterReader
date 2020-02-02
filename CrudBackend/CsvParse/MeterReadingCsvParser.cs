using ENSEK_Meter_Reader_Server.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ENSEK_Meter_Reader.CrudBackend.CsvParse {
    /// <summary>
    /// Extracts smart meter reading data from a provided CSV file.
    /// </summary>
    public class MeterReadingCsvParser : CsvParser<MeterReading> {
        private const string ACCOUNT_HEADING = "AccountId";
        private const string METER_READING_TIME_HEADING = "MeterReadingDateTime";
        private const string METER_VALUE_HEADING = "MeterReadValue";

        private const string CSV_DATE_TIME_FORMAT = "dd/MM/yyyy HH:mm";

        public MeterReadingCsvParser() {
            PropertySetters = new Dictionary<string, Action<MeterReading, string>> {
                { ACCOUNT_HEADING, SetAccountId },
                { METER_READING_TIME_HEADING, SetTimestamp },
                { METER_VALUE_HEADING, SetMeterValue }
            };
        }

        // Meter readings must consist of exactly 5 numeric characters.
        private Regex meterReadingPattern = new Regex("^\\d{5}$");

        private void SetAccountId(MeterReading reading, string accountId) {
            reading.AccountId = accountId;
        }

        private void SetTimestamp(MeterReading reading, string dateTimeString) {
            DateTime dateTime = DateTime.ParseExact(dateTimeString, CSV_DATE_TIME_FORMAT, null);
            reading.Timestamp = new DateTimeOffset(dateTime).ToUnixTimeSeconds();
        }

        private void SetMeterValue(MeterReading reading, string meterValue) {
            bool meterReadingFormatValid = meterReadingPattern.IsMatch(meterValue);
            if (!meterReadingFormatValid) {
                throw new FormatException(
                    String.Format("Invalid meter reading format {0}.", meterValue)
                );
            }

            reading.MeterValue = Int32.Parse(meterValue);
        }
    }
}
