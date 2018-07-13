using DBManager;
using DBManager.Services;
using Infrastructure;
using Infrastructure.Events;
using Infrastructure.Queries;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Reporting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Materials.ViewModels
{
    public class BatchStatusListViewModel : BindableBase
    {
        private DelegateCommand<DataGrid> _openBatch;
        private DelegateCommand<IList> _printSelected;
        private DelegateCommand<Window> _cancel, _confirm;
        private EventAggregator _eventAggregator;
        private IDataService _dataService;
        private IReportingService _reportingService;

        public BatchStatusListViewModel(EventAggregator eventAggregator,
                                        IDataService dataService,
                                        IReportingService reportingService) : base()
        {
            _dataService = dataService;
            _eventAggregator = eventAggregator;
            _reportingService = reportingService;

            _cancel = new DelegateCommand<Window>(
                parentDialog =>
                {
                    parentDialog.DialogResult = false;
                });

            _confirm = new DelegateCommand<Window>(
                parentDialog =>
                {
                    parentDialog.DialogResult = true;
                });

            _eventAggregator.GetEvent<BatchChanged>().Subscribe(
                token =>
                {
                    RunBatchQueryCommand.Execute();
                });

            _eventAggregator.GetEvent<SampleLogCreated>()
                .Subscribe(
                sample =>
                {
                    RunBatchQueryCommand.Execute();
                });

            _eventAggregator.GetEvent<BatchStatusListRefreshRequested>()
                            .Subscribe(
                            () =>
                            {
                                RunBatchQueryCommand.Execute();
                            });

            _openBatch = new DelegateCommand<DataGrid>(
                grid =>
                {
                    Batch btc = grid.SelectedItem as Batch;
                    NavigationToken token = new NavigationToken(MaterialViewNames.BatchInfoView,
                                                                btc);

                    _eventAggregator.GetEvent<NavigationRequested>()
                                    .Publish(token);
                });

            _printSelected = new DelegateCommand<IList>(
                batchList =>
                {
                    Batch[] processList = new Batch[batchList.Count];
                    batchList.CopyTo(processList, 0);
                    _reportingService.PrintBatchReport(processList);
                });

            RunBatchQueryCommand = new DelegateCommand(
                () =>
                {
                    IQuery<Batch> batchQuery = new BatchQuery()
                    {
                        AspectCode = AspectCode,
                        BatchNumber = BatchNumber,
                        ColorName = ColorName,
                        ConstructionName = ConstructionName,
                        MaterialLineCode = MaterialLineCode,
                        MaterialTypeCode = MaterialTypeCode,
                        Notes = Notes,
                        OEMName = OEMName,
                        ProjectDescription = ProjectDescription,
                        ProjectNumber = ProjectNumber,
                        RecipeCode = RecipeCode
                    };

                    BatchList = _dataService.GetQueryResults<Batch>(batchQuery);
                    RaisePropertyChanged("BatchList");
                });

            RunBatchQueryCommand.Execute();
        }

        public IEnumerable<Batch> BatchList { get; private set; }

        public DelegateCommand<Window> CancelCommand
        {
            get { return _cancel; }
        }

        public DelegateCommand<Window> ConfirmCommand
        {
            get { return _confirm; }
        }

        public DelegateCommand<DataGrid> OpenBatchCommand
        {
            get { return _openBatch; }
        }

        public DelegateCommand<IList> PrintSelectedCommand => _printSelected;


        public DelegateCommand RunBatchQueryCommand { get; }

        #region Search Parameter Properties

        public string AspectCode { get; set; }

        public string BatchNumber { get; set; }

        public string ColorName { get; set; }

        public string ConstructionName { get; set; }

        public string MaterialLineCode { get; set; }

        public string MaterialTypeCode { get; set; }

        public string Notes { get; set; }

        public string OEMName { get; set; }

        public string ProjectNumber { get; set; }

        public string ProjectDescription { get; set; }

        public string RecipeCode { get; set; }

        #endregion
    }
}
