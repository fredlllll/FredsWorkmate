﻿using FredsWorkmate.Util;

namespace FredsWorkmate.Database.Models
{
    public class InvoiceSeller : Model
    {
        public required Invoice Invoice { get; set; }
        public required string CompanyName { get; set; }
        public required string ContactName { get; set; }
        public required string Email { get; set; }
        public required string Street { get; set; }
        public required string HouseNumber { get; set; }
        public required string PostalCode { get; set; }
        public required string City { get; set; }
        public required CountryCode Country { get; set; }
        public required string BankName { get; set; }
        public required string BankIBAN { get; set; }
        public required string BankBIC_Swift { get; set; }
        /// <summary>
        /// fiscal number, tax number
        /// </summary>
        public required string FC { get; set; } = "";
        /// <summary>
        /// VAT number, ust.Id
        /// </summary>
        public required string VA { get; set; } = "";

        public override string ToString()
        {
            return $"{ContactName} {CompanyName} {Email}, {Street} {HouseNumber} {PostalCode} {City} {Country}, {BankName} {BankIBAN} {BankBIC_Swift}({Id})";
        }
    }
}
