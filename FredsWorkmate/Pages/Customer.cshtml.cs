using FredsWorkmate.Database;
using FredsWorkmate.Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FredsWorkmate.Pages
{
    public class CustomerModel : PageModel
    {
        public Customer Customer { get; set; }

        public readonly DatabaseContext databaseContext;

#pragma warning disable CS8618
        public CustomerModel(DatabaseContext databaseContext)
#pragma warning restore CS8618
        {
            this.databaseContext = databaseContext;
        }

        public void OnGet(string id)
        {
            Customer = databaseContext.Customers.First(x => x.Id == id);
        }
    }
}
