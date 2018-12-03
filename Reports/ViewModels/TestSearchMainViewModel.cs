using DataAccess;
using Infrastructure.Events;
using Infrastructure.Queries;
using LabDbContext;
using Prism.Mvvm;
using Prism.Commands;
using Prism.Events;
using Reports.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Reports.ViewModels
{
    public class TestSearchMainViewModel : BindableBase
    {
        #region Fields

        private IEventAggregator _eventAggregator;
        private IDataService<LabDbEntities> _labDbData;

        #endregion Fields

        #region Constructors

        public TestSearchMainViewModel(IDataService<LabDbEntities> labDbData,
                                        IEventAggregator eventAggregator)
        {
            _labDbData = labDbData;
            _eventAggregator = eventAggregator;

            IncludeExternalReports = true;
            IncludeInternalReports = true;

            RunQueryCommand = new DelegateCommand(
                () =>
                {
                    IQuery<Test, LabDbEntities> testQuery = new TestQuery()
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

                    ResultList = _labDbData.RunQuery(testQuery).ToList();
                    RaisePropertyChanged("ResultList");
                });

            RowDoubleClickCommand = new DelegateCommand<Test>(
                tst =>
                {
                    object rpt;                    
                    string viewName;

                    if (tst.TestRecord.RecordTypeID == 1)
                    {
                        rpt = _labDbData.RunQuery(new ReportQuery() { TestRecordID = tst.TestRecord.ID });
                        viewName = ViewNames.ReportEditView;
                    }
                    else if (tst.TestRecord.RecordTypeID == 2)
                    {
                        rpt = _labDbData.RunQuery(new ExternalReportsQuery()).FirstOrDefault(exrep => exrep.TestRecords.Any(trec => trec.ID == tst.TestRecord.ID ));
                        viewName = ViewNames.ExternalReportEditView;
                    }
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

        #endregion Constructors

        #region Properties

        public string AspectCode { get; set; }

        public string BatchNumber { get; set; }

        public string ColorName { get; set; }

        public bool IncludeExternalReports { get; set; }

        public bool IncludeInternalReports { get; set; }

        public string LineCode { get; set; }

        public string MaterialTypeCode { get; set; }

        public string MethodName { get; set; }

        public string RecipeCode { get; set; }

        public IEnumerable<Test> ResultList { get; private set; }
        public DelegateCommand<Test> RowDoubleClickCommand { get; }

        public DelegateCommand RunQueryCommand { get; }
        public string TestName { get; set; }

        #endregion Properties
    }
}