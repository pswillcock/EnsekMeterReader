using ENSEK_Meter_Reader_Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ENSEK_Meter_Reader.CrudBackend.CsvParse {
    public class CustomerAccountCsvParser : CsvParser<CustomerAccount> {
        private const string ACCOUNT_HEADING = "AccountId";
        private const string FIRST_NAME_HEADING = "FirstName";
        private const string LAST_NAME_HEADING = "LastName";

        public CustomerAccountCsvParser() {
            PropertySetters = new Dictionary<string, Action<CustomerAccount, string>> {
                { ACCOUNT_HEADING, SetAccountId },
                { FIRST_NAME_HEADING, SetFirstName },
                { LAST_NAME_HEADING, SetLastName }
            };
        }

        private void SetAccountId(CustomerAccount account, string id) {
            account.Id = id;
        }

        private void SetFirstName(CustomerAccount account, string firstName) {
            account.FirstName = firstName;
        }

        private void SetLastName(CustomerAccount account, string lastName) {
            account.LastName = lastName;
        }
    }
}
