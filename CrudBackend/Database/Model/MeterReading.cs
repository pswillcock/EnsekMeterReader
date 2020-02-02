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
    }
}
