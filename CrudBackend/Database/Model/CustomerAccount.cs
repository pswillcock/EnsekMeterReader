using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ENSEK_Meter_Reader_Server.Models {

    [Table("Accounts")]
    public class CustomerAccount {
        [Key]
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public List<MeterReading> MeterReadings { get; set; }
    }
}
