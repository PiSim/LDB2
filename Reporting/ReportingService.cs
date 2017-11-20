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
        private IDataService _dataService;

        public ReportingService(IDataService dataService,
                                EventAggregator eventAggregator)
        {
            _dataService = dataService;
            _docRenderer = new DocRenderer();
            _eventAggregator = eventAggregator;
        }

        public void PrintLatestBatchReport()
        {
            IEnumerable<Batch> batchList = _dataService.GetBatches(25);

            FixedDocument batchReport = new FixedDocument();
            _docRenderer.SetLandscape(batchReport);
            batchReport.DocumentPaginator.PageSize = PageSizes.A4Landscape;
            
            PageContent content = new PageContent();
            batchReport.Pages.Add(content);
            FixedPage newpage = new FixedPage()
            {
                Height = batchReport.DocumentPaginator.PageSize.Height,
                Width = batchReport.DocumentPaginator.PageSize.Width
            };
            content.Child = newpage;
            
            MainPageGrid mainGrid = new MainPageGrid()
            {
                Height = batchReport.DocumentPaginator.PageSize.Height,
                Width = batchReport.DocumentPaginator.PageSize.Width
            };

            _docRenderer.AddStandardHeader(mainGrid.HeaderGrid);
            _docRenderer.AddBatchList(mainGrid.BodyGrid, batchList);
            newpage.Children.Add(mainGrid);

            ShowPreview(batchReport);
        }

        internal void ShowPreview(IDocumentPaginatorSource doc)
        {
            Views.DocumentPreviewDialog previewDialog = new Views.DocumentPreviewDialog();
            previewDialog.Document = doc;

            previewDialog.Show();
        }
    }
}
