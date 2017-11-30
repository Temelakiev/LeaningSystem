using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LearningSystem.Services.Implementations
{
    public class PdfGenerator : IPdfGenerator
    {
        public byte[] GeneratePdfFromHtml(string html)
        {
            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
            HtmlWorker htmlParser = new HtmlWorker(pdfDoc);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                pdfDoc.Open();

                using (var stringReader = new StringReader(html))
                {
                    htmlParser.Parse(stringReader);
                }

                pdfDoc.Close();

               return  memoryStream.ToArray();
            }
        }
    }
}
