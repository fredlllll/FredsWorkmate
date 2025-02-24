using FredsWorkmate.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;

namespace FredsWorkmate.Pages.Invoice
{
    [Route("Invoice/Pdf")]
    [ApiController]
    public class InvoicePdf : ControllerBase
    {
        readonly DatabaseContext db;
        readonly IStringLocalizer<DocumentGeneration.Invoice> localizer;

        public InvoicePdf(DatabaseContext db ,IStringLocalizer<DocumentGeneration.Invoice> localizer) {
            this.db = db; 
            this.localizer = localizer;
        }

        [HttpGet("{id}/{language}")]
        public IActionResult OnGet(string id, string language)
        {
            var generator = new DocumentGeneration.Invoice(db, localizer, id, language);
            var document = generator.Create();

            PdfDocumentRenderer renderer = new()
            {
                Document = document
            };
            renderer.RenderDocument();
            MemoryStream ms = new();
            renderer.Save(ms, false);
            ms.Position = 0;

            return File(ms, "application/pdf");
        }
    }
}
