namespace FredsWorkmate.Database.Models
{
    public class InvoicePosition : Model
    {
        public required Invoice Invoice { get; set; }
        public required string Description { get; set; }
        public required decimal Price { get; set; }
        public required decimal Count { get; set; }

        public override string ToString()
        {
            return $"{Description} {Price}x{Count}({Id})";
        }
    }
}
