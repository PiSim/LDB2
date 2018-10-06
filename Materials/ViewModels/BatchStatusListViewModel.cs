using DataAccess;
using Infrastructure;
using Infrastructure.Events;
using LabDbContext;
using Materials.Queries;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Reporting;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Materials.ViewModels
{
    public class BatchStatusListViewModel : BindableBase
    {
        #region Fields

        private IEventAggregator _eventAggregator;
        private IDataService<LabDbEntities> _labDbData;
        private IReportingService _reportingService;

        #endregion Fields

        #region Constructors

        public BatchStatusListViewModel(IEventAggregator eventAggregator,
                                        IDataService<LabDbEntities> labDbData,
                                        IReportingService reportingService) : base()
        {
            _labDbData = labDbData;
            _eventAggregator = eventAggregator;
            _reportingService = reportingService;

            CancelCommand = new DelegateCommand<Window>(
                parentDialog =>
                {
                    parentDialog.DialogResult = false;
                });

            ConfirmCommand = new DelegateCommand<Window>(
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

            OpenBatchCommand = new DelegateCommand<DataGrid>(
                grid =>
                {
                    Batch btc = grid.SelectedItem as Batch;
                    NavigationToken token = new NavigationToken(MaterialViewNames.BatchInfoView,
                                                                btc);

                    _eventAggregator.GetEvent<NavigationRequested>()
                                    .Publish(token);
                });

            OpenOrderFileCommand = new DelegateCommand<Batch>(
                batch =>
                {
                    try
                    {
                        System.Diagnostics.Process.Start(batch.OrderFilePath);
                    }
                    catch
                    {
                    }
                });

            PrintSelectedCommand = new DelegateCommand<IList>(
                batchList =>
                {
                    Batch[] processList = new Batch[batchList.Count];
                    batchList.CopyTo(processList, 0);
                    _reportingService.PrintBatchReport(processList);
                });

            RunBatchQueryCommand = new DelegateCommand(
                () =>
                {
                    IQuery<Batch, LabDbEntities> batchQuery = new BatchesQuery()
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

                    BatchList = _labDbData.RunQuery(batchQuery).ToList();
                    RaisePropertyChanged("BatchList");
                });

            RunBatchQueryCommand.Execute();
        }

        #endregion Constructors

        #region Properties

        public IEnumerable<Batch> BatchList { get; private set; }

        public DelegateCommand<Window> CancelCommand { get; }

        public DelegateCommand<Window> ConfirmCommand { get; }
        public DelegateCommand<DataGrid> OpenBatchCommand { get; }
        public DelegateCommand<Batch> OpenOrderFileCommand { get; }
        public DelegateCommand<IList> PrintSelectedCommand { get; }

        public DelegateCommand RunBatchQueryCommand { get; }

        #endregion Properties

        #region Search Parameter Properties

        public string AspectCode { get; set; }

        public string BatchNumber { get; set; }

        public string ColorName { get; set; }

        public string ConstructionName { get; set; }

        public string MaterialLineCode { get; set; }

        public string MaterialTypeCode { get; set; }

        public string Notes { get; set; }

        public string OEMName { get; set; }

        public string ProjectDescription { get; set; }
        public string ProjectNumber { get; set; }
        public string RecipeCode { get; set; }

        #endregion Search Parameter Properties
    }
}