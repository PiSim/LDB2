using DBManager;
using Infrastructure;
using Infrastructure.Events;
using Microsoft.Practices.Prism.Mvvm;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Reports.ViewModels
{
    public class ExternalReportEditViewModel : BindableBase
    {
        private Batch _selectedBatch;
        private DBEntities _entities;
        private DelegateCommand _addBatch, _addFile, _addPO, _openBatch, _openFile, _removeBatch, _removeFile;
        private EventAggregator _eventAggregator;
        private ExternalReport _instance;
        private ExternalReportFile _selectedFile;
        private IMaterialServiceProvider _materialServiceProvider;
        private IReportServiceProvider _reportServiceProvider;
        private ObservableCollection<Batch> _batchList;
        private ObservableCollection<ExternalReportFile> _reportFiles;

        public ExternalReportEditViewModel(DBEntities entities,
                                            EventAggregator aggregator,
                                            IMaterialServiceProvider materialServiceProvider,
                                            IReportServiceProvider reportServiceProvider) : base()
        {
            _eventAggregator = aggregator;
            _materialServiceProvider = materialServiceProvider;
            _reportServiceProvider = reportServiceProvider;
            _entities = entities;
            _eventAggregator.GetEvent<CommitRequested>().Subscribe(() => _entities.SaveChanges());
            
            _addBatch = new DelegateCommand(
                () => 
                {
                    Batch tempBatch = _materialServiceProvider.StartBatchSelection();
                    if (tempBatch != null)
                    {
                        ExternalReportBatchMapping tempMap = new ExternalReportBatchMapping();
                        tempMap.Batch = _entities.Batches.First(btc => btc.ID == tempBatch.ID);
                        _instance.BatchMappings.Add(tempMap);
                        _batchList.Add(tempBatch);
                    }
                } );

            _addFile = new DelegateCommand(
                () =>
                {
                    OpenFileDialog fileDialog = new OpenFileDialog();
                    fileDialog.Multiselect = true;
                    
                    if (fileDialog.ShowDialog() == DialogResult.OK)
                    {
                        foreach (string pth in fileDialog.FileNames)
                        {
                            ExternalReportFile temp = new ExternalReportFile();
                            temp.Path = pth;
                            temp.Description = "";
                            ReportFiles.Add(temp);   
                            _instance.ExternalReportFiles.Add(temp);
                        }
                    }
                });

            _addPO = new DelegateCommand(
                () =>
                {
                    _reportServiceProvider.AddPOToExternalReport(_instance);
                    OnPropertyChanged("PurchaseOrder");
                });
            
            _openBatch = new DelegateCommand(
                () =>
                {
                    NavigationToken token = new NavigationToken(MaterialViewNames.BatchInfoView,
                                                                _selectedBatch);
                    _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
                },
                () => _selectedBatch != null);

            _openFile = new DelegateCommand(
                () =>
                {
                    System.Diagnostics.Process.Start(_selectedFile.Path);
                },
                () => _selectedFile != null);

            _removeBatch = new DelegateCommand(
                () =>
                {
                    ExternalReportBatchMapping tempMap = _instance.BatchMappings
                        .FirstOrDefault(btm => btm.BatchID == _selectedBatch.ID);

                    if (tempMap != null)
                        _entities.ExternalReportBatchMappings.Remove(tempMap);

                    _batchList.Remove(_selectedBatch);
                    SelectedBatch = null;
                },
                () => _selectedBatch != null);

            _removeFile = new DelegateCommand(
                () =>
                {
                    ReportFiles.Remove(_selectedFile);
                    _instance.ExternalReportFiles.Remove(_selectedFile);
                    SelectedFile = null;
                },
                () => _selectedFile != null);
        }

        public DelegateCommand AddBatchCommand
        {
            get { return _addBatch;}
        }

        public DelegateCommand AddFileCommand
        {
            get { return _addFile; }
        }

        public DelegateCommand AddPOCommand
        {
            get { return _addPO; }
        }
        
        public ObservableCollection<Batch> BatchList
        {
            get { return _batchList; }
        }

        public string Description
        {
            get 
            { 
                if (_instance == null)
                    return null;
                    
                return _instance.Description; 
            }
            set { _instance.Description = value; }
        }

        public string Currency
        {
            get 
            { 
                if (_instance == null)
                    return null;
                    
                return _instance.Currency; 
            }
            set { _instance.Currency = value; }
        }
        
        public string ExternalLab
        {
            get
            {
                if (_instance == null)
                    return null;
                return _instance.ExternalLab.Name;
            }
        }

        public ExternalReport ExternalReportInstance
        {
            get { return _instance; }
            set 
            {
                _instance = value;
                OnPropertyChanged("Currency");
                OnPropertyChanged("Description");

                SelectedBatch = null;

                _batchList = new ObservableCollection<Batch>
                    (_instance.BatchMappings.Select(btm => btm.Batch));
                OnPropertyChanged("BatchList");

                _reportFiles = new ObservableCollection<ExternalReportFile>
                    (_instance.ExternalReportFiles);
                OnPropertyChanged("ReportFiles");
                OnPropertyChanged("ExternalLab");
                OnPropertyChanged("InternalNumber");
                OnPropertyChanged("SamplesSent");
                OnPropertyChanged("Price");
                OnPropertyChanged("Project");
                OnPropertyChanged("PurchaseOrder");
                OnPropertyChanged("ReportReceived");
                OnPropertyChanged("RequestDone");
                OnPropertyChanged("Samples");
            }
        }
        
        public int InternalNumber
        {
            get 
            { 
                if (_instance == null)
                    return 0;
                    
                return _instance.InternalNumber; 
            }
            set { _instance.InternalNumber = value; }
        }
        
        public DelegateCommand OpenBatchCommand
        {
            get { return _openBatch; }
        }

        public DelegateCommand OpenFileCommand
        {
            get { return _openFile; }
        }

        public DelegateCommand RemoveBatchCommand
        {
            get { return _removeBatch; }
        }

        public DelegateCommand RemoveFileCommand
        {
            get { return _removeFile; }
        }

        public ObservableCollection<ExternalReportFile> ReportFiles
        {
            get { return _reportFiles; }
        }

        public bool ReportReceived
        {
            get
            {
                if (_instance == null)
                    return false;

                else
                    return _instance.ReportReceived;
            }
            set
            {
                _instance.ReportReceived = value;
            }
        }

        public bool RequestDone
        {
            get
            {
                if (_instance == null)
                    return false;

                else
                    return _instance.RequestDone;
            }
            set
            {
                _instance.RequestDone = value;
            }
        }

        public bool SamplesSent
        {
            get
            {
                if (_instance == null)
                    return false;

                else
                    return _instance.MaterialSent;
            }

            set
            {
                _instance.MaterialSent = value;
            }
        }

        public Batch SelectedBatch
        {
            get { return _selectedBatch;  }
            set
            {
                _selectedBatch = value;
                OnPropertyChanged("SelectedBatch");
                _openBatch.RaiseCanExecuteChanged();
                _removeBatch.RaiseCanExecuteChanged();
            }
        }
        

        public double Price
        {
            get 
            { 
                if (_instance == null)
                    return 0;
                    
                return _instance.Price; 
            }

            set { _instance.Price = value; }
        }
        
        public Project Project
        {
            get 
            { 
                if (_instance == null)
                    return null;
                    
                return _instance.Project; 
            }
        }

        public PurchaseOrder PurchaseOrder
        {
            get 
            { 
                if (_instance == null)
                    return null;
                    
                return _instance.PO; 
            }
        }

        public string Samples
        {
            get 
            { 
                if (_instance == null)
                    return null;

                return _instance.Samples; 
            }
            
            set { _instance.Samples = value; }
        }

        public ExternalReportFile SelectedFile
        {
            get { return _selectedFile; }
            set
            {
                _selectedFile = value;
                _openFile.RaiseCanExecuteChanged();
                _removeFile.RaiseCanExecuteChanged();
            }
        }
    }
}

