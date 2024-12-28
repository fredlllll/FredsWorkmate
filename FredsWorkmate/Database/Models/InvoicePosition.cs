namespace FredsWorkmate.Database.Models
{
    public class InvoicePosition : Model
    {
        public required Invoice Invoice { get; set; }
        public required string Decription { get; set; }
        public required decimal Price {  get; set; }
        public required int Count { get; set; }
    }
}
