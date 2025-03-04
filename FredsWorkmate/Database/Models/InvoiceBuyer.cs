using FredsWorkmate.Util;

namespace FredsWorkmate.Database.Models
{
    public class InvoiceBuyer : Model
    {
        public required Invoice Invoice { get; set; }
        public required string CompanyName { get; set; }
        public required string ContactName { get; set; }
        public required string Email { get; set; }
        public required string Street { get; set; } = "";
        public required string HouseNumber { get; set; } = "";
        public required string PostalCode { get; set; } = "";
        public required string City { get; set; } = "";
        public required CountryCode Country { get; set; }
        public required decimal VATRate { get; set; }

        public override string ToString()
        {
            return $"{ContactName} {CompanyName} {Email} VAT:{VATRate * 100}%({Id})";
        }
    }
}
