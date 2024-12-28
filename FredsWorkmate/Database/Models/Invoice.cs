namespace FredsWorkmate.Database.Models
{
    public class Invoice : Model
    {
        public Customer? Customer { get; set; }
        public Project? Project { get; set; }

        public required string InvoiceNumber { get; set; }
        public required DateOnly InvoiceDate { get; set; }
        public required decimal VATRate {  get; set; }

        public required ICollection<InvoicePosition> Positions { get; set; }
    }
}
