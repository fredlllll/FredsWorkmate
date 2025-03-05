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
        public required CountryCode Country { get; set; }


        public override string ToString()
        {
            return $"{AddressExtra} {Street} {HouseNumber}, {PostalCode} {City}, {Country}({Id})";
        }

        public string Format()
        {
            return Format(AddressExtra, Street, HouseNumber, PostalCode, City, Country);
        }

        public static string Format(string addressExtra, string street, string houseNumber, string postalCode, string city, CountryCode country)
        {
            string baseAddress = $"{street} {houseNumber}\n{postalCode} {city}";
            if (country != CountryCode.DE)
            {
                baseAddress += $"\n{CountryCodeUtil.ToNativeCountryName(country)}";
            }
            if (!string.IsNullOrEmpty(addressExtra))
            {
                return addressExtra + "\n" + baseAddress;
            }
            return baseAddress;
        }
    }
}
