using FredsWorkmate.Util;

namespace FredsWorkmate.Database.Models
{
    public class Address : Model
    {
        public required string AddressExtra { get; set; }
        public required string Street { get; set; }
        public required string HouseNumber { get; set; }
        public required string PostalCode { get; set; }
        public required string City { get; set; }
        public required string Country { get; set; }


        public override string ToString()
        {
            return $"{AddressExtra} {Street} {HouseNumber}, {PostalCode} {City}, {Country}({Id})";
        }
    }
}
