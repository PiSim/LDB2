using DBManager;
using DBManager.Services;
using Infrastructure;
using Infrastructure.Events;
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

            _eventAggregator.GetEvent<BatchCreated>().Subscribe(
                batch =>
                {
                    RaisePropertyChanged("BatchList");
                });

            _eventAggregator.GetEvent<SampleLogCreated>()
                .Subscribe(
                sample =>
                {
                    RaisePropertyChanged("BatchList");
                });

            _eventAggregator.GetEvent<BatchStatusListRefreshRequested>()
                            .Subscribe(
                            () =>
                            {
                                RaisePropertyChanged("BatchList");
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
        }

        public IEnumerable<Batch> BatchList
        {
            get { return _dataService.GetBatches(); }
        }

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
    }
}
