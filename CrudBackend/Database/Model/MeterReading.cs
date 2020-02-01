using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ENSEK_Meter_Reader_Server.Models {
    public class MeterReading {
        [Key]
        public string Id { get; set; }

        public long Timestamp { get; set; }

        public int MeterValue { get; set; }

        public string AccountId { get; set; }

        [ForeignKey("AccountId")]
        public CustomerAccount Account { get; set; }

        /// <summary>
        /// Converts a string of the format "dd/mm/yyyy hh:mm" to a Unix timestamp and stores it in Timestamp.
        /// </summary>
        /// <param name="timeDateString">Date time string to convert.</param>
        /// <returns>True if the conversion succeeded else false.</returns>
        public bool StoreTimestampFromString(string timeDateString) {
            DateTime dateTime;
            bool dateParseSuccess = DateTime.TryParse(timeDateString, out dateTime);

            if (dateParseSuccess) {
                Timestamp = new DateTimeOffset(dateTime).ToUnixTimeSeconds();
            }

            return dateParseSuccess;
        }
    }
}
