using System.Diagnostics.Metrics;

namespace FredsWorkmate.Database.Models
{
    public class BankInformation : Model
    {
        public required string BankName { get; set; }
        public required string IBAN { get; set; }
        public required string BIC_Swift { get; set; }

        public override string ToString()
        {
            return $"{BankName} {IBAN} {BIC_Swift}({Id})";
        }
    }
}
