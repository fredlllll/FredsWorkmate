using FredsWorkmate.Database;
using FredsWorkmate.Database.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FredsWorkmate.Pages
{
    public class CustomersModel : PageModel
    {
        public readonly DatabaseContext databaseContext;

        public CustomersModel(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }
        public void OnGet()
        {

        }
        public void OnPostAdd(string name, string email, string address) {
            string newId = databaseContext.GetNewId<Customer>();
            var c = new Customer()
            {
                Id = newId,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                Name = name,
                Email = email,
                Address = address
            };
            databaseContext.Customers.Add(c);
            databaseContext.SaveChanges();

            LocalRedirect($"/Customers/{newId}");
        }
    }
}
