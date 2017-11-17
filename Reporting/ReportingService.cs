using DBManager;
using Prism.Events;
using Reporting.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;

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
            PageContent content = new PageContent();
            batchReport.Pages.Add(content);
            FixedPage newpage = new FixedPage();
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

        internal void ShowPreview(FixedDocument doc)
        {
            Views.DocumentPreviewDialog previewDialog = new Views.DocumentPreviewDialog();
            previewDialog.Document = doc;

            previewDialog.Show();
        }
    }
}
