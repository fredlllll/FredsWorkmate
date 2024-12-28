
namespace FredsWorkmate.Database.Models
{
    public class CompanyInformation : Model
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required Address Address { get; set; }
    }
}
