using DBManager;
using Infrastructure.Events;
using Infrastructure.Queries;
using Microsoft.Practices.Prism.Mvvm;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reports.ViewModels
{
    public class TestSearchMainViewModel : BindableBase
    {
        DelegateCommand _runQuery;
        DataAccessService _dataService;
        EventAggregator _eventAggregator;
        IEnumerable<Test> _resultList;

        public TestSearchMainViewModel(DataAccessService dataService,
                                        EventAggregator eventAggregator)
        {
            _dataService = dataService;
            _eventAggregator = eventAggregator;

            IncludeExternalReports = true;
            IncludeInternalReports = true;

            _runQuery = new DelegateCommand(
                () =>
                {
                    IQuery<Test> testQuery = new TestQuery()
                    {
                        AspectCode = AspectCode,
                        BatchNumber = BatchNumber,
                        ColorName = ColorName,
                        IncludeExternalReports = IncludeExternalReports,
                        IncludeInternalReports = IncludeInternalReports,
                        LineCode = LineCode,
                        MaterialTypeCode = MaterialTypeCode,
                        MethodName = MethodName,
                        RecipeCode = RecipeCode,
                        TestName = TestName
                    };

                    _resultList = _dataService.GetQueryResults(testQuery);
                    OnPropertyChanged("ResultList");
                });

            RowDoubleClickCommand = new DelegateCommand<Test>(
                tst =>
                {
                    Object rpt = tst.TestRecord.GetReport();
                    string viewName;

                    if (tst.TestRecord.RecordTypeID == 1)
                        viewName = ViewNames.ReportEditView;

                    else if (tst.TestRecord.RecordTypeID == 2)
                        viewName = ViewNames.ExternalReportEditView;

                    else
                        return;

                    NavigationToken token = new NavigationToken(viewName,
                                                                rpt,
                                                                null);

                    _eventAggregator.GetEvent<NavigationRequested>()
                                    .Publish(token);
                }
                );
        }

        public string AspectCode { get; set; }

        public string BatchNumber { get; set; }

        public string ColorName { get; set; }

        public bool IncludeExternalReports { get; set; }

        public bool IncludeInternalReports { get; set; }

        public string LineCode { get; set; }

        public string MaterialTypeCode { get; set; }

        public string MethodName { get; set; }

        public string RecipeCode { get; set; }

        public DelegateCommand<Test> RowDoubleClickCommand { get; }

        public DelegateCommand RunQueryCommand => _runQuery;

        public IEnumerable<Test> ResultList => _resultList;

        public string TestName { get; set; }
    }
}
