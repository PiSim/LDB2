using DBManager;
using iText;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Formats
{
    public class ReportRawDataSheet : Document
    {
        public ReportRawDataSheet(Report target, string path) 
            : base(new PdfDocument(new PdfWriter(path)))
        {
            Add(new Paragraph("HW"));
        }
    }
}
