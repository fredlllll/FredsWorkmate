using FredsWorkmate.Database;
using FredsWorkmate.Database.Models;
using FredsWorkmate.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using PdfSharp.Pdf;
using s2industries.ZUGFeRD;
using System.Globalization;

namespace FredsWorkmate.DocumentGeneration
{
    public class Invoice
    {
        private readonly DatabaseContext db;
        private readonly IStringLocalizer<Invoice> localizer;
        private readonly string id;
        //private readonly string language;

        private readonly Document document;
        private readonly Section section;
        private readonly CultureInfo cultureInfo;

        //data
        class InvoiceData
        {
            public required Database.Models.Invoice invoice;
            public required InvoiceBuyer invoiceBuyer;
            public required InvoiceSeller invoiceSeller;
            public required IEnumerable<InvoicePosition> invoicePositions;
        }
        InvoiceData? data;

        public string lastXRechnungXmlPath { get; set; } = string.Empty;

        public Invoice(DatabaseContext db, IStringLocalizer<Invoice> localizer, string id, string language)
        {
            this.db = db;
            this.localizer = localizer;
            this.id = id;
            //this.language = language;
            cultureInfo = language switch
            {
                "de" => CultureInfo.GetCultureInfo("de-DE"),
                "en" => CultureInfo.GetCultureInfo("en-US"),
                _ => CultureInfo.InvariantCulture,
            };
            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;

            document = new Document
            {
                Info =
                {
                    Title = localizer["Invoice"]+" "+id,
                    Author = "Freds Workmate",
                }
            };
            //page width is 21cm, with margin normal working area is 17cm
            section = document.AddSection();
            section.PageSetup.LeftMargin = "2cm";
            section.PageSetup.RightMargin = "2cm";
        }

        public Document Create()
        {
            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;

            LoadData();
            AddHeader();
            AddContent();
            AddFooter();

            AddXRechnung();

            return document;
        }

        void LoadData()
        {
            var invoice = db.Invoices.Include(x => x.Positions).First(i => i.Id == id);
            if (invoice == null)
            {
                throw new InvalidOperationException();
            }
            db.LoadReferences(invoice);

            data = new InvoiceData
            {
                invoice = invoice,
                invoiceBuyer = invoice.Buyer,
                invoiceSeller = invoice.Seller,
                invoicePositions = invoice.Positions,
            };
        }

        void AddHeader()
        {
            if (data == null)
            {
                throw new InvalidOperationException();
            }
            var seller = data.invoiceSeller;
            Paragraph header = section.Headers.Primary.AddParagraph($"{seller.CompanyName} | {seller.Street} {seller.HouseNumber} | {seller.PostalCode} {seller.City}");
            header.Format.Font.Size = 11;
            header.Format.SpaceAfter = "1cm";
            header.Format.Alignment = ParagraphAlignment.Left;
        }

        void AddContent()
        {
            if (data == null)
            {
                throw new InvalidOperationException();
            }

            var buyer = data.invoiceBuyer;
            var invoice = data.invoice;

            var topTable = section.AddTable();
            //topTable.Borders.Width = 0;
            topTable.AddColumn("9cm");
            topTable.AddColumn("4cm");
            topTable.AddColumn("4cm");

            var row1 = topTable.AddRow();
            row1.Cells[0].MergeDown = 1;
            row1.Cells[0].AddParagraph($"{buyer.CompanyName}\n{buyer.ContactName}\n{Address.Format("", buyer.Street, buyer.HouseNumber, buyer.PostalCode, buyer.City, buyer.Country)}");
            row1.Height = "16pt";

            row1.Cells[1].AddParagraph(localizer["Invoice Number"]);
            row1.Cells[1].Format.Alignment = ParagraphAlignment.Right;
            row1.Cells[2].AddParagraph(invoice.InvoiceNumber);
            row1.Cells[2].Format.Alignment = ParagraphAlignment.Right;

            var row2 = topTable.AddRow();

            row2.Cells[1].AddParagraph(localizer["Invoice Date"]);
            row2.Cells[1].Format.Alignment = ParagraphAlignment.Right;
            row2.Cells[2].AddParagraph(invoice.InvoiceDate.ToString("d. MMMM yyyy", cultureInfo));
            row2.Cells[2].Format.Alignment = ParagraphAlignment.Right;

            var row3 = topTable.AddRow();
            row3.Cells[1].AddParagraph(localizer["Delivery Date"]);
            row3.Cells[1].Format.Alignment = ParagraphAlignment.Right;
            row3.Cells[2].AddParagraph(invoice.DeliveryDate.ToString("d. MMMM yyyy", cultureInfo));
            row3.Cells[2].Format.Alignment = ParagraphAlignment.Right;

            var p = section.AddParagraph("");
            p.Format.Font.Size = 16;

            var invoiceHeading = section.AddParagraph(localizer["Invoice"]);
            invoiceHeading.Format.Font.Size = 18;
            invoiceHeading.Format.Font.Bold = true;
            invoiceHeading.Format.SpaceAfter = "10pt";

            Table itemTable = section.AddTable();
            itemTable.Borders.Bottom.Width = 1;
            itemTable.Format.SpaceBefore = "2pt";
            itemTable.Format.SpaceAfter = "2pt";

            // Define columns
            itemTable.AddColumn("8cm"); // Description
            itemTable.AddColumn("2cm"); // Count
            itemTable.AddColumn("3.5cm"); // Price/Pc
            itemTable.AddColumn("3.5cm"); // Total

            itemTable.Columns[1].Format.Alignment = ParagraphAlignment.Right;
            itemTable.Columns[2].Format.Alignment = ParagraphAlignment.Right;
            itemTable.Columns[3].Format.Alignment = ParagraphAlignment.Right;

            // Add header row
            Row headerRow = itemTable.AddRow();
            //headerRow.Shading.Color = Colors.LightGray; // Light gray background
            headerRow.Cells[0].AddParagraph(localizer["Description"]);
            headerRow.Cells[1].AddParagraph(localizer["Count"]);
            headerRow.Cells[2].AddParagraph(localizer["Price/Pc"]);
            headerRow.Cells[3].AddParagraph(localizer["Total"]);

            var currency = invoice.Currency.ToString();

            decimal totalPrice = 0;

            foreach (var position in data.invoicePositions)
            {
                var row = itemTable.AddRow();
                row.Cells[0].AddParagraph(position.Description);
                row.Cells[1].AddParagraph(position.Count.ToString(cultureInfo));
                row.Cells[2].AddParagraph(string.Create(cultureInfo, $"{currency} {position.Price:F2}"));
                var rowPrice = Math.Round(position.Count * position.Price, 2);
                totalPrice += rowPrice;
                row.Cells[3].AddParagraph(string.Create(cultureInfo, $"{currency} {rowPrice:F2}"));
            }

            p = section.AddParagraph("");
            p.Format.Font.Size = 16;

            var totalTable = section.AddTable();
            totalTable.AddColumn("13.5cm");
            totalTable.Columns[0].Format.Alignment = ParagraphAlignment.Right;
            totalTable.AddColumn("3.5cm");
            totalTable.Columns[1].Format.Alignment = ParagraphAlignment.Right;

            row1 = totalTable.AddRow();
            row1.Cells[0].AddParagraph(localizer["Total"] + ":");
            row1.Cells[1].AddParagraph(string.Create(cultureInfo, $"{currency} {totalPrice:F2}"));

            if (buyer.VATRate > 0)
            {
                decimal vat = Math.Round(totalPrice * buyer.VATRate, 2);
                decimal totalWithVat = totalPrice + vat;

                row1 = totalTable.AddRow();
                row1.Cells[0].AddParagraph(string.Create(cultureInfo, $"{localizer["VAT"]} {buyer.VATRate * 100:F2}%:"));
                row1.Cells[1].AddParagraph(string.Create(cultureInfo, $"{currency} {vat:F2}"));

                row2 = totalTable.AddRow();
                row2.Cells[0].AddParagraph(localizer["Total+VAT"] + ":");
                row2.Cells[1].AddParagraph(string.Create(cultureInfo, $"{currency} {totalWithVat:F2}"));
            }
            else
            {
                p = section.AddParagraph("");
                p.Format.Font.Size = 16;
                section.AddParagraph(localizer["No VAT added, due to work done for a company based in a non EU country"]);
            }
        }

        void AddFooter()
        {
            if (data == null)
            {
                throw new InvalidOperationException();
            }

            var seller = data.invoiceSeller;

            var footer = section.Footers.Primary.AddTable();
            footer.Format.Font.Size = 10;
            footer.AddColumn("6cm");
            footer.AddColumn("7cm");
            footer.AddColumn("4cm");
            var row = footer.AddRow();
            row.Cells[0].AddParagraph($"{seller.ContactName}\n{seller.Street} {seller.HouseNumber}\n{seller.PostalCode} {seller.City}\n\nEmail: {seller.Email}");
            row.Cells[1].AddParagraph($"{localizer["Bank Information"]}:\n{localizer["Bank"]}: {seller.BankName}\n{localizer["IBAN"]}: {seller.BankIBAN}\n{localizer["BIC/Swift"]}: {seller.BankBIC_Swift}");
            row.Cells[2].AddParagraph($"{localizer["VAT IdNr."]}:\n {data.invoiceSeller.VA}");
        }

        void AddXRechnung()
        {
            if (data == null)
            {
                throw new InvalidOperationException();
            }

            InvoiceDescriptor invoiceDoc = InvoiceDescriptor.CreateInvoice(data.invoice.InvoiceNumber, data.invoice.InvoiceDate.ToDateTime(TimeOnly.MinValue), CurrencyCodeUtil.ToCurrencyCodes(data.invoice.Currency));

            invoiceDoc.Type = InvoiceType.Invoice; //TODO: make this configurable
            invoiceDoc.ActualDeliveryDate = data.invoice.DeliveryDate.ToDateTime(TimeOnly.MinValue);

            invoiceDoc.Buyer = new Party();
            invoiceDoc.Buyer.Name = data.invoiceBuyer.CompanyName;
            invoiceDoc.Buyer.ContactName = data.invoiceBuyer.ContactName;
            invoiceDoc.Buyer.Street = data.invoiceBuyer.Street + " " + data.invoiceBuyer.HouseNumber;
            invoiceDoc.Buyer.Postcode = data.invoiceBuyer.PostalCode;
            invoiceDoc.Buyer.City = data.invoiceBuyer.City;
            invoiceDoc.Buyer.Country = CountryCodeUtil.ToCountryCodes(data.invoiceBuyer.Country);
            invoiceDoc.BuyerElectronicAddress = new ElectronicAddress();
            invoiceDoc.BuyerElectronicAddress.Address = data.invoiceBuyer.Email;
            invoiceDoc.BuyerElectronicAddress.ElectronicAddressSchemeID = ElectronicAddressSchemeIdentifiers.EM;

            invoiceDoc.Seller = new Party();
            invoiceDoc.Seller.Name = data.invoiceSeller.CompanyName;
            invoiceDoc.Seller.ContactName = data.invoiceSeller.ContactName;
            invoiceDoc.Seller.Street = data.invoiceSeller.Street + " " + data.invoiceSeller.HouseNumber;
            invoiceDoc.Seller.Postcode = data.invoiceSeller.PostalCode;
            invoiceDoc.Seller.City = data.invoiceSeller.City;
            invoiceDoc.Seller.Country = CountryCodeUtil.ToCountryCodes(data.invoiceSeller.Country);
            invoiceDoc.Seller.ID = new GlobalID(null, "TODO Seller Id in buyers system");
            invoiceDoc.SellerContact = new Contact();
            invoiceDoc.SellerContact.Name = data.invoiceSeller.ContactName;
            invoiceDoc.SellerContact.EmailAddress = data.invoiceSeller.Email;
            invoiceDoc.SellerContact.PhoneNo = "0190666666"; //for some reason phone number is mandatory, but who uses phones anymore?
            invoiceDoc.SellerElectronicAddress = new ElectronicAddress();
            invoiceDoc.SellerElectronicAddress.Address = data.invoiceSeller.Email;
            invoiceDoc.SellerElectronicAddress.ElectronicAddressSchemeID = ElectronicAddressSchemeIdentifiers.EM;
            invoiceDoc.AddSellerTaxRegistration(data.invoiceSeller.FC, TaxRegistrationSchemeID.FC);
            invoiceDoc.AddSellerTaxRegistration(data.invoiceSeller.VA, TaxRegistrationSchemeID.VA);

            decimal total = 0;
            foreach (var pos in data.invoicePositions)
            {
                var lineTotal = pos.Price * pos.Count;
                total += lineTotal;
                invoiceDoc.AddTradeLineItem(pos.Description, pos.Price, unitCode: QuantityCodes.HUR, unitQuantity: pos.Count);
            }
            if (data.invoiceBuyer.VATRate > 0)
            {
                invoiceDoc.AddApplicableTradeTax(total, data.invoiceBuyer.VATRate * 100, total * data.invoiceBuyer.VATRate, TaxTypes.VAT, TaxCategoryCodes.S);
            }

            // Save the XML
            var xmlPath = lastXRechnungXmlPath = Path.Combine(Path.GetTempPath(), $"invoice_{data.invoice.Id}.xml");
            invoiceDoc.Save(xmlPath, ZUGFeRDVersion.Version23, Profile.XRechnung, ZUGFeRDFormats.CII);

            //TODO: actually attach the fucking file, but i cant find how anywhere
        }
    }
}
