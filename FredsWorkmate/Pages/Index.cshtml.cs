using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FredsWorkmate.Pages
{
    public class IndexPage : PageModel
    {
        private readonly ILogger<IndexPage> _logger;

        public IndexPage(ILogger<IndexPage> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}
