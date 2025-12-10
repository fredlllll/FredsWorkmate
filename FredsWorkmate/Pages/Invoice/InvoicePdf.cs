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

        public InvoicePdf(DatabaseContext db, IStringLocalizer<DocumentGeneration.Invoice> localizer)
        {
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
            var tmpPdfPath = Path.Combine(Path.GetTempPath(), $"{id}_tmp.pdf");
            using (var fs = new FileStream(tmpPdfPath, FileMode.Create))
            {
                renderer.Save(fs, false);
            }
            var pdfPath = Path.Combine(Path.GetTempPath(), $"{id}.pdf");
            var result = Util.Python.Venv.RunPythonFile(Util.Python.Venv.DefaultVenv, "embed_xml.py", tmpPdfPath, generator.lastXRechnungXmlPath, pdfPath);
            if (result != 0)
            {
                throw new Exception("could not append xml to pdf");
            }

            var pdfStream = new FileStream(pdfPath, FileMode.Open);
            return File(pdfStream, "application/pdf");
        }
    }
}
