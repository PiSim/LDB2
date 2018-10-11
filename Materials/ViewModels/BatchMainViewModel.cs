using Controls.Views;
using DataAccess;
using Infrastructure;
using Infrastructure.Events;
using LabDbContext;
using Materials.Queries;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Reporting;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Materials.ViewModels
{
    internal class BatchMainViewModel : BindableBase
    {
        #region Fields

        private IEventAggregator _eventAggregator;
        private bool _isPrintMenuOpen;
        private IDataService<LabDbEntities> _labDbData;
        private MaterialService _materialService;
        private DelegateCommand _refresh;
        private IReportingService _reportingService;

        #endregion Fields

        #region Constructors

        public BatchMainViewModel(IEventAggregator eventAggregator,
                                IDataService<LabDbEntities> labDbData,
                                MaterialService materialService,
                                IReportingService reportingService)
        {
            _labDbData = labDbData;
            _eventAggregator = eventAggregator;
            _isPrintMenuOpen = false;
            _materialService = materialService;
            _reportingService = reportingService;

            CreateBatchCommand = new DelegateCommand(
                () =>
                {
                    _materialService.CreateBatch();
                },
                () => Thread.CurrentPrincipal.IsInRole(UserRoleNames.BatchEdit));

            OpenPrintMenuCommand = new DelegateCommand(
                () =>
                {
                    IsPrintMenuOpen = true;
                });

            OpenSampleLogViewCommand = new DelegateCommand(
                () =>
                {
                    _materialService.ShowSampleLogDialog();
                },
                () => Thread.CurrentPrincipal.IsInRole(UserRoleNames.SampleEdit));
            
            PrintBatchQueryCommand = new DelegateCommand<IQueryPresenter<Batch, LabDbEntities>>(
                query =>
                {
                    IEnumerable<Batch> tempQuery = _labDbData.RunQuery(query.Query);
                    _reportingService.PrintBatchReport(tempQuery);
                });

            PrintStatusListCommand = new DelegateCommand(
                () =>
                {
                    _reportingService.PrintBatchReport(_labDbData.RunQuery(new LatestNBatchesQuery(50)));
                });

            SearchNewBatchesCommand = new DelegateCommand(
                () =>
                {
                    ICollection<Tuple<string, string, string>> fileList = _materialService.GetNewBatchesFromFile();

                    ICollection<Batch> parsedBatches = ParseBatches(fileList);

                    Views.NewBatchSearchResultDialog dialog = new Views.NewBatchSearchResultDialog()
                    {
                        ParsedBatches = parsedBatches
                    };

                    if (dialog.ShowDialog() == true)
                    {
                    }
                });

            _refresh = new DelegateCommand(
                () =>
                {
                    _eventAggregator.GetEvent<BatchStatusListRefreshRequested>()
                                    .Publish();
                });
        }

        #endregion Constructors

        #region Methods

        private ICollection<Batch> ParseBatches(ICollection<Tuple<string, string, string>> fileList)
        {
            Batch current;
            List<Batch> output = new List<Batch>(fileList.Count);
            foreach (Tuple<string, string, string> batchfile in fileList)
            {
                string parsedTypeCode = (batchfile.Item2.Length >= 4) ? batchfile.Item2.Substring(0, 4) : "";
                string parsedLineCode = (batchfile.Item2.Length >= 7) ? batchfile.Item2.Substring(4, 3) : "";
                string parsedAspectCode = (batchfile.Item2.Length >= 10) ? batchfile.Item2.Substring(7, 3) : "";
                string parsedRecipeCode = (batchfile.Item2.Length >= 14) ? batchfile.Item2.Substring(10, 4) : "";

                current = new Batch()
                {
                    Material = new Material()
                    {
                        Aspect = new Aspect() { Code = parsedAspectCode },
                        MaterialLine = new MaterialLine() { Code = parsedLineCode },
                        MaterialType = new MaterialType() { Code = parsedTypeCode },
                        Recipe = new Recipe() { Code = parsedRecipeCode }
                    },
                    Number = batchfile.Item1,
                    OrderFilePath = batchfile.Item3
                };

                output.Add(current);
            }

            return output;
        }

        #endregion Methods

        #region Properties

        public string BatchStatusListRegionName => RegionNames.BatchStatusListRegion;
        public DelegateCommand CreateBatchCommand { get; }

        public bool IsPrintMenuOpen
        {
            get => _isPrintMenuOpen;
            set
            {
                _isPrintMenuOpen = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand OpenPrintMenuCommand { get; }
        public DelegateCommand OpenSampleLogViewCommand { get; }
        public DelegateCommand<IQueryPresenter<Batch, LabDbEntities>> PrintBatchQueryCommand { get; }
        public DelegateCommand PrintStatusListCommand { get; }

        public IEnumerable<IQueryPresenter<Batch, LabDbEntities>> QueryList { get; } = new List<IQueryPresenter<Batch, LabDbEntities>>
        {
            new ArrivedUntestedBatchesQueryPresenter(),
            new BatchesNotArrivedQueryPresenter(),
            new Latest25BatchesQueryPresenter()
        };

        public DelegateCommand RefreshCommand => _refresh;

        public string SampleArchiveRegionName => RegionNames.SampleArchiveRegion;

        public string SampleLongTermStorageRegionName => RegionNames.SampleLongTermStorageRegion;

        public DelegateCommand SearchNewBatchesCommand { get; set; }
        public Sample SelectedSampleArrival { get; set; }

        #endregion Properties
    }
}