using FredsWorkmate.Util;

namespace FredsWorkmate.Database.Models
{
    public class Invoice : Model
    {
        public Project? Project { get; set; }

        public required string InvoiceNumber { get; set; }
        public required CurrencyCode Currency { get; set; }
        public required InvoiceBuyer Buyer { get; set; }
        public required InvoiceSeller Seller { get; set; }
        public required DateOnly InvoiceDate { get; set; }

        public ICollection<InvoicePosition> Positions { get; set; } = new List<InvoicePosition>();

        public override string ToString()
        {
            return $"{InvoiceNumber}({Id})";
        }
    }
}
