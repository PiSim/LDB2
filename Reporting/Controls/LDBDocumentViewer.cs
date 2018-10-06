using System.Printing;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Xps;

namespace Reporting.Controls
{
    public class LDBDocumentViewer : DocumentViewer
    {
        #region Methods

        protected override void OnPrintCommand()
        {
            // get a print dialog, defaulted to default printer and default printer's preferences.
            PrintDialog printDialog = new PrintDialog();
            printDialog.PrintQueue = LocalPrintServer.GetDefaultPrintQueue();
            printDialog.PrintTicket = printDialog.PrintQueue.DefaultPrintTicket;

            // get a reference to the FixedDocumentSequence for the viewer.
            FixedDocument docSeq = this.Document as FixedDocument;

            // set the default page orientation based on the desired output.
            printDialog.PrintTicket.PageOrientation = DocRenderer.GetPageOrientationOfFirstPageOfFixedDocument(docSeq);
            printDialog.PrintTicket.Duplexing = Duplexing.TwoSidedShortEdge;

            if (printDialog.ShowDialog() == true)
            {
                // set the print ticket for the document sequence and write it to the printer.
                docSeq.PrintTicket = printDialog.PrintTicket;

                XpsDocumentWriter writer = PrintQueue.CreateXpsDocumentWriter(printDialog.PrintQueue);
                writer.WriteAsync(docSeq, printDialog.PrintTicket);
            }
        }

        #endregion Methods
    }
}