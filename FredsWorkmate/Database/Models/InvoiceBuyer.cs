namespace FredsWorkmate.Database.Models
{
    public class InvoiceBuyer :Model
    {
        public required Invoice Invoice { get; set; }
        public required string CompanyName {  get; set; }
        public required string ContactName { get; set; }
        public required string Email {  get; set; }
        public required decimal VATRate { get; set; }

        public override string ToString()
        {
            return $"{ContactName} {CompanyName} {Email} VAT:{VATRate}({Id})";
        }
    }
}
