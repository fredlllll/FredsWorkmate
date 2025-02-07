using FredsWorkmate.Database;
using FredsWorkmate.Database.Models;
using FredsWorkmate.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using System.Globalization;

namespace FredsWorkmate.DocumentGeneration
{
    public class Invoice
    {
        private readonly DatabaseContext db;
        private readonly IStringLocalizer<Invoice> localizer;
        private readonly string id;
        private readonly string language;

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


        public Invoice(DatabaseContext db, IStringLocalizer<Invoice> localizer, string id, string language)
        {
            this.db = db;
            this.localizer = localizer;
            this.id = id;
            this.language = language;
            switch (language)
            {
                case "de":
                    cultureInfo = CultureInfo.GetCultureInfo("de-DE");
                    break;
                case "en":
                    cultureInfo = CultureInfo.GetCultureInfo("en-US");
                    break;
                default:
                    cultureInfo = CultureInfo.InvariantCulture;
                    break;
            }
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
            row1.Cells[0].AddParagraph($"{buyer.ContactName}\n{buyer.CompanyName}\n{buyer.Email}");
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
                row1.Cells[0].AddParagraph(localizer["Total+VAT"] + ":");
                row1.Cells[1].AddParagraph(string.Create(cultureInfo, $"{currency} {totalWithVat:F2}"));
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
            footer.AddColumn("6cm");
            footer.AddColumn("5cm");
            var row = footer.AddRow();
            row.Cells[0].AddParagraph($"{seller.ContactName}\n{seller.Street} {seller.HouseNumber}\n{seller.PostalCode} {seller.City}\n\nEmail: {seller.Email}");
            row.Cells[1].AddParagraph($"{localizer["Bank Information"]}:\n{localizer["Bank"]}: {seller.BankName}\n{localizer["IBAN"]}: {seller.BankIBAN}\n{localizer["BIC/Swift"]}: {seller.BankBIC_Swift}");
            row.Cells[2].AddParagraph($"{localizer["VAT IdNr."]}:\n TODO");
        }
    }
}
