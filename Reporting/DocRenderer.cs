using DBManager;
using Reporting.Controls;
using System;
using System.Collections;
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

        /// <summary>
        /// Fills the page with a standard MainPageGrid to store content and returns a reference to it
        /// </summary>
        /// <param name="page">The page</param>
        /// <returns>A reference to the MainPageGrid</returns>
        internal MainPageGrid AddMainGrid(FixedPage page)
        {
            MainPageGrid mainGrid = new MainPageGrid()
            {
                Height = page.Height,
                Width = page.Width
            };
            page.Children.Add(mainGrid);
            return mainGrid;
        }

        /// <summary>
        /// Adds a new Page to a FixedDocument and returns a reference to it
        /// </summary>
        /// <param name="doc">The document to which a page is added</param>
        /// <returns>A reference to the new page</returns>
        internal FixedPage AddPageToFixedDocument(FixedDocument doc)
        {
            PageContent content = new PageContent();
            doc.Pages.Add(content);
            FixedPage newPage = new FixedPage()
            {
                Height = doc.DocumentPaginator.PageSize.Height,
                Width = doc.DocumentPaginator.PageSize.Width
            };
            content.Child = newPage;

            return newPage;
        }

        internal void AddStandardHeader(Grid targetGrid,
                                        string title = "")
        {
            targetGrid.Children.Add(new StandardHeader()
            {
                Title = title
            });
        }

        internal void AddBatchList(Grid targetGrid, IEnumerable<object> batchList)
        {
            BatchList listElement = new BatchList();
            listElement.MainList.ItemsSource = batchList;

            targetGrid.Children.Add(listElement);
        }

        internal void AddTaskList(Grid targetGrid, IEnumerable<object> taskList)
        {
            TaskList listElement = new TaskList();
            listElement.MainList.ItemsSource = taskList;

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
        /// Divides an IEnumerable in multiple ones with an appropriate number of elements for visualization in a single page
        /// </summary>
        /// <param name="entityList">The IEnumerable to divide</param>
        /// <returns>An IEnumerable containing the divided Ienumerables</returns>
        internal IEnumerable<IEnumerable<object>> PaginateEntityList(IEnumerable<object> entityList)
        {
            int elementsPerPage = 25;

            return entityList.Select((x, i) => new { Index = i, Value = x })
                            .GroupBy(x => x.Index / elementsPerPage)
                            .Select(x => x.Select(v => v.Value).ToList())
                            .ToList();
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
