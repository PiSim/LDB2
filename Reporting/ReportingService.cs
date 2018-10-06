using LabDbContext;
using Prism.Events;
using Reporting.Controls;
using System.Collections.Generic;
using System.Windows.Documents;

namespace Reporting
{
    public class ReportingService : IReportingService
    {
        #region Fields

        private DocRenderer _docRenderer;
        private IEventAggregator _eventAggregator;

        #endregion Fields

        #region Constructors

        public ReportingService(IEventAggregator eventAggregator)
        {
            _docRenderer = new DocRenderer();
            _eventAggregator = eventAggregator;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Renders a document detailing a list of batch items and shows it in a preview window for printing.
        /// </summary>
        /// <param name="batchesToPrint">The list of Batches to print</param>
        public void PrintBatchReport(IEnumerable<Batch> batchesToPrint)
        {
            FixedDocument batchReport = new FixedDocument();
            batchReport.DocumentPaginator.PageSize = PageSizes.A4Landscape;

            IEnumerable<IEnumerable<object>> paginatedBatches = _docRenderer.PaginateEntityList(batchesToPrint);

            foreach (IEnumerable<object> batchListPage in paginatedBatches)
            {
                FixedPage currentPage = _docRenderer.AddPageToFixedDocument(batchReport);
                MainPageGrid currentMainGrid = _docRenderer.AddMainGrid(currentPage);
                _docRenderer.AddStandardHeader(currentMainGrid.HeaderGrid);
                _docRenderer.AddBatchList(currentMainGrid.BodyGrid, batchListPage);
            };

            ShowPreview(batchReport);
        }

        /// <summary>
        /// Renders a datasheet to collect test data for a report, and shows it in a preview window for printing
        /// </summary>
        /// <param name="entry">The Report entry</param>
        public void PrintReportDataSheet(Report entry)
        {
            FixedDocument dataSheet = new FixedDocument();
            dataSheet.DocumentPaginator.PageSize = PageSizes.A4;

            FixedPage currentPage = _docRenderer.AddPageToFixedDocument(dataSheet);
            ReportDataSheetMainGrid mainGrid = _docRenderer.AddReportDataSheetGrid(currentPage);
            mainGrid.ReportInstance = entry;
            ShowPreview(dataSheet);
        }

        /// <summary>
        /// Renders a document detailing a list of Task items and shows it in a preview window for printing.
        /// </summary>
        /// <param name="batchesToPrint">The list of Tasks to print</param>
        public void PrintTaskList(IEnumerable<Task> taskList)
        {
            FixedDocument taskReport = new FixedDocument();
            taskReport.DocumentPaginator.PageSize = PageSizes.A4Landscape;

            IEnumerable<IEnumerable<object>> paginatedTasks = _docRenderer.PaginateEntityList(taskList);

            foreach (IEnumerable<object> taskListPage in paginatedTasks)
            {
                FixedPage currentPage = _docRenderer.AddPageToFixedDocument(taskReport);
                MainPageGrid currentMainGrid = _docRenderer.AddMainGrid(currentPage);
                _docRenderer.AddStandardHeader(currentMainGrid.HeaderGrid,
                                                "Lista Lavori");
                _docRenderer.AddTaskList(currentMainGrid.BodyGrid, taskListPage);
            };

            ShowPreview(taskReport);
        }

        internal void ShowPreview(IDocumentPaginatorSource doc)
        {
            Views.DocumentPreviewDialog previewDialog = new Views.DocumentPreviewDialog
            {
                Document = doc
            };

            previewDialog.Show();
        }

        #endregion Methods
    }
}