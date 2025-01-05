using FredsWorkmate.Database;
using FredsWorkmate.Database.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;

namespace FredsWorkmate.Pages.Invoice
{
    public class InvoiceCreate : PageModel
    {
        public readonly DatabaseContext db;
        public string ProposedInvoiceNumber { get; private set; } = "";
        public string CurrentDate { get; private set; } = "";

        public InvoiceCreate(DatabaseContext db)
        {
            this.db = db;
        }

        public void OnGet()
        {
            DateTime now = DateTime.Now;
            string fixedPart = $"{now.Year}{now.Month:00}{now.Day:00}";
            int variablePart = 1;

            ProposedInvoiceNumber = $"{fixedPart}{variablePart:000}";
            while (db.Invoices.Any(x => x.InvoiceNumber == ProposedInvoiceNumber))
            {
                variablePart++;
                ProposedInvoiceNumber = $"{fixedPart}{variablePart:000}";
            }

            CurrentDate = now.ToString("yyyy-MM-dd");
        }

        public void OnPost()
        {
            HttpRequest req = HttpContext.Request;

            string invoiceNumber = req.Form["InvoiceNumber"].First<string>();
            DateOnly invoiceDate = DateOnly.Parse(req.Form["InvoiceDate"].First<string>());

            InvoiceBuyer buyer = new()
            {
                Id = db.GetNewId<InvoiceBuyer>(),
                ContactName = req.Form["Buyer.ContactName"].First<string>(),
                CompanyName = req.Form["Buyer.CompanyName"].First<string>(),
                Email = req.Form["Buyer.Email"].First<string>(),
                VATRate = decimal.Parse(req.Form["Buyer.VATRate"].First<string>(), CultureInfo.InvariantCulture)
            };
            string buyerId = req.Form["Buyer"].First<string>();
            if (!string.IsNullOrEmpty(buyerId))
            {
                var buyerCustomer = db.Customers.Find(buyerId);
                if (buyerCustomer != null)
                {
                    buyer.ContactName = buyerCustomer.ContactName;
                    buyer.CompanyName = buyerCustomer.CompanyName;
                    buyer.Email = buyerCustomer.Email;
                    buyer.VATRate = buyerCustomer.VATRate;
                }
            }

            InvoiceSeller seller = new()
            {
                Id = db.GetNewId<InvoiceSeller>(),
                ContactName = req.Form["Seller.ContactName"].First<string>(),
                CompanyName = req.Form["Seller.CompanyName"].First<string>(),
                Email = req.Form["Seller.Email"].First<string>(),
                Street = req.Form["Seller.Street"].First<string>(),
                HouseNumber = req.Form["Seller.HouseNumber"].First<string>(),
                PostalCode = req.Form["Seller.PostalCode"].First<string>(),
                City = req.Form["Seller.City"].First<string>(),
                Country = req.Form["Seller.Country"].First<string>(),
                BankName = req.Form["Seller.BankName"].First<string>(),
                BankIBAN = req.Form["Seller.BankIBAN"].First<string>(),
                BankBIC_Swift = req.Form["Seller.BankBIC_Swift"].First<string>(),
            };
            string sellerId = req.Form["Seller"].First<string>();
            if (!string.IsNullOrEmpty(sellerId))
            {
                var sellerCompany = db.CompanyInformations.Find(sellerId);
                if (sellerCompany != null)
                {
                    db.LoadReferences(sellerCompany);
                    seller.ContactName = sellerCompany.ContactName;
                    seller.CompanyName = sellerCompany.CompanyName;
                    seller.Email = sellerCompany.Email;
                    seller.Street = sellerCompany.Address.Street;
                    seller.HouseNumber = sellerCompany.Address.HouseNumber;
                    seller.PostalCode = sellerCompany.Address.PostalCode;
                    seller.City = sellerCompany.Address.City;
                    seller.Country = sellerCompany.Address.Country;
                    seller.BankName = sellerCompany.BankInformation.BankName;
                    seller.BankIBAN = sellerCompany.BankInformation.IBAN;
                    seller.BankBIC_Swift = sellerCompany.BankInformation.BIC_Swift;
                }
            }

            string currency = req.Form["Currency"].First<string>();
            Database.Models.Invoice invoice = new()
            {
                Id = db.GetNewId<Database.Models.Invoice>(),
                InvoiceNumber = invoiceNumber,
                InvoiceDate = invoiceDate,
                Buyer = buyer,
                Seller = seller,
                Currency = Enum.Parse<Currency>(currency)
            };

            db.Add(buyer);
            db.Add(seller);
            db.Add(invoice);

            for (int i = 0; i < 3; i++)
            {
                var description = req.Form[$"Position.{i}.Description"].FirstOrDefault<string>();
                if (!string.IsNullOrEmpty(description))
                {
                    Database.Models.InvoicePosition position = new()
                    {
                        Id = db.GetNewId<Database.Models.InvoicePosition>(),
                        Description = description,
                        Count = int.Parse(req.Form[$"Position.{i}.Count"].First<string>(), CultureInfo.InvariantCulture),
                        Price = decimal.Parse(req.Form[$"Position.{i}.Price"].First<string>(), CultureInfo.InvariantCulture),
                        Invoice = invoice
                    };
                    db.Add(position);
                }
            }
            db.SaveChanges();
            LocalRedirect($"/Invoice/Detail/{invoice.Id}");
        }
    }
}
