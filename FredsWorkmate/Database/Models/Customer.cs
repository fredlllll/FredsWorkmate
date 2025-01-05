using FredsWorkmate.Util;

namespace FredsWorkmate.Database.Models
{
    public class Customer : Model, INoteOwner
    {
        public required string CompanyName { get; set; }
        public required string ContactName { get; set; }
        public required string Email { get; set; }
        public required Address Address { get; set; }
        public required CompanyInformation Company { get; set; }
        public required decimal VATRate { get; set; }
        public ICollection<Note> Notes { get; set; } = new List<Note>();

        public override string ToString()
        {
            return $"{ContactName} {CompanyName} {Email} VAT:{VATRate}({Id})";
        }
    }
}
