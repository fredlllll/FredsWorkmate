﻿
using System.Diagnostics.Metrics;

namespace FredsWorkmate.Database.Models
{
    public class CompanyInformation : Model
    {
        public required string CompanyName { get; set; }
        public required string ContactName { get; set; }
        public required string Email { get; set; }
        public required Address Address { get; set; }
        public required BankInformation BankInformation { get; set; }
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
            return $"{ContactName} {CompanyName} {Email}({Id})";
        }
    }
}
