using Reporting.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;

namespace Reporting
{
    internal class DocRenderer
    {
        internal DocRenderer()
        {

        }

        internal void AddStandardHeader(Grid targetGrid)
        {
            targetGrid.Children.Add(new StandardHeader());
        }

        internal void AddBatchList(Grid targetGrid, IEnumerable<DBManager.Batch> batchList)
        {
            BatchList listElement = new BatchList();
            listElement.MainList.ItemsSource = batchList;

            targetGrid.Children.Add(listElement);
        }

        internal FixedPage GetNewPage()
        {
            FixedPage output = new FixedPage();

            output.Children.Add(new Grid()
            {
                Margin = new Thickness(20, 20, 20, 20)
            });

            return output;
        }

        internal void SetLandscape(FixedDocument doc)
        {
            doc.PrintTicket = new PrintTicket()
            {
                PageOrientation = PageOrientation.Landscape
            };
        }

        /// <summary>
        /// Gets the first fixed page in a fixed document, or null.
        /// </summary>
        /// <param name="doc">The fixed document</param>
        /// <returns>The first fixed document, or null.</returns>
        private static FixedPage GetFirstPageOfFixedDocument(FixedDocument doc)
        {
            FixedPage firstPage = null;

            if (doc != null && doc.Pages.Count != 0)
            {
                PageContent firstCont = doc.Pages[0];
                firstPage = firstCont.GetPageRoot(false);
            }

            return firstPage;
        }

        /// <summary>
        /// Gets the first fixed document in a fixed document sequence, or null.
        /// </summary>
        /// <param name="docSeq">The fixed document sequence</param>
        /// <returns>The first fixed document, or null.</returns>
        private static FixedDocument GetFirstDocumentOfFixedDocSeq(FixedDocumentSequence docSeq)
        {
            FixedDocument firstDoc = null;

            if (docSeq != null && docSeq.References != null && docSeq.References.Count > 0)
            {
                DocumentReference firstDocRef = docSeq.References[0];
                firstDoc = firstDocRef.GetDocument(true);
            }

            return firstDoc;
        }
        
        /// <summary>
        /// Gets the PageOrientation of the first page of a fixed document, based on that page's dimensions.
        /// </summary>
        /// <param name="doc">The fixed document.</param>
        /// <returns>
        /// If the first page could not be found, returns Unknown.
        /// Returns Portrait when the page width is less than the height.  
        /// Otherwise (width is greater than OR EQUAL), returns Landscape.
        /// </returns>

        internal static PageOrientation GetPageOrientationOfFirstPageOfFixedDocument(FixedDocument doc)
        {
            PageOrientation orientation = PageOrientation.Unknown;

            FixedPage firstPage = GetFirstPageOfFixedDocument(doc);

            if (firstPage != null)
            {
                orientation = (firstPage.Width >= firstPage.Height) ? PageOrientation.Landscape : PageOrientation.Portrait;
            }

            return orientation;
        }

        /// <summary>
        /// Writes a FixedDocument to a temp path and returns the closed XpsDocument
        /// </summary>
        /// <param name="doc">The document to write</param>
        /// <returns>The closed XpsDocument instance</returns>

        internal XpsDocument WriteXps(FixedDocument doc)
        {
            string tempPath = Path.GetTempPath()
                + DateTime.Now.ToString("yyyyMMddHHmmss")
                + ".xps";

            XpsDocument output = new XpsDocument(tempPath, FileAccess.ReadWrite);
            XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(output);
            writer.Write(doc);
            output.Close();

            return output;
        }
    }
}
