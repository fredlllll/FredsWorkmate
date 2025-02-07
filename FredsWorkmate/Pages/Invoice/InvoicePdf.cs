using FredsWorkmate.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;

namespace FredsWorkmate.Pages.Invoice
{
    [Route("Invoice/Pdf")]
    [ApiController]
    public class InvoicePdf : ControllerBase
    {
        readonly DatabaseContext db;

        public InvoicePdf(DatabaseContext db) { this.db = db; }

        [HttpGet("{id}/{language}")]
        public IActionResult OnGet(string id, string language)
        {
            var generator = new DocumentGeneration.Invoice(db,id, language);
            var document = generator.Create();

            PdfDocumentRenderer renderer = new PdfDocumentRenderer();
            renderer.Document = document;
            renderer.RenderDocument();
            MemoryStream ms = new MemoryStream();
            renderer.Save(ms, false);
            ms.Position = 0;

            return File(ms, "application/pdf");
        }
    }
}
