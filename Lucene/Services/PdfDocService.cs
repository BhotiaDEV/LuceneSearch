using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucene.Services
{
    internal class PdfDocService
    {
        public string ReadPdf(string filepath)
        {
            StringBuilder pdf = new StringBuilder();
            PdfReader pdfReader = new PdfReader(filepath);
            PdfDocument pdfDocument = new PdfDocument(pdfReader);
            for (int pageNum = 1; pageNum <= pdfDocument.GetNumberOfPages(); pageNum++)
            {
                PdfPage page = pdfDocument.GetPage(pageNum);
                ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                string currentText = PdfTextExtractor.GetTextFromPage(page, strategy);
                pdf.Append(currentText);
            }
            return pdf.ToString();
        }
    }
}
