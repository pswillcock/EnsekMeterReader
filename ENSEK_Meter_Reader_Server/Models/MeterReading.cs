using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ENSEK_Meter_Reader_Server.Models {
    public class MeterReading {
        [Key]
        public int ReadingId { get; set; }

        [ForeignKey("AccountId")]
        public CustomerAccount Account { get; set; }

        public long Timestamp { get; set; }

        public int MeterValue { get; set; }
    }
}
