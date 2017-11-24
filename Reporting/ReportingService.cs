using DBManager;
using Prism.Events;
using Reporting.Controls;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Xps.Packaging;

namespace Reporting
{
    public class ReportingService : IReportingService
    {
        private DocRenderer _docRenderer;
        private EventAggregator _eventAggregator;

        public ReportingService(EventAggregator eventAggregator)
        {
            _docRenderer = new DocRenderer();
            _eventAggregator = eventAggregator;
        }

        /// <summary>
        /// Renders a document detailing a list of batch items and shows it in a preview window for printing.
        /// </summary>
        /// <param name="batchesToPrint">The list of Batches to print</param>
        public void PrintBatchReport(IEnumerable<Batch> batchesToPrint)
        {
            FixedDocument batchReport = new FixedDocument();
            batchReport.DocumentPaginator.PageSize = PageSizes.A4Landscape;

            IEnumerable<IEnumerable<Batch>> paginatedBatches = _docRenderer.PaginateBatchList(batchesToPrint);
            
            foreach(IEnumerable<Batch> batchListPage in paginatedBatches)
            {
                FixedPage currentPage = _docRenderer.AddPageToFixedDocument(batchReport);
                MainPageGrid currentMainGrid = _docRenderer.AddMainGrid(currentPage);
                _docRenderer.AddStandardHeader(currentMainGrid.HeaderGrid);
                _docRenderer.AddBatchList(currentMainGrid.BodyGrid, batchListPage);
            };

            ShowPreview(batchReport);
        }

        internal void ShowPreview(IDocumentPaginatorSource doc)
        {
            Views.DocumentPreviewDialog previewDialog = new Views.DocumentPreviewDialog
            {
                Document = doc
            };

            previewDialog.Show();
        }
    }
}
