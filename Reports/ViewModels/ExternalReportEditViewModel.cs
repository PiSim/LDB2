using DBManager;
using DBManager.Services;
using Infrastructure;
using Infrastructure.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Services;
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
        private DBPrincipal _principal;
        private DelegateCommand _addBatch, _addFile, _addPO, _openBatch, _openFile, _removeBatch, _removeFile;
        private EventAggregator _eventAggregator;
        private ExternalReport _instance;
        private ExternalReportFile _selectedFile;
        private string _batchNumber;

        public ExternalReportEditViewModel(DBPrincipal principal,
                                            EventAggregator aggregator) : base()
        {
            _eventAggregator = aggregator;
            _principal = principal;

            _eventAggregator.GetEvent<CommitRequested>().Subscribe(() => _instance.Update());
            
            _addBatch = new DelegateCommand(
                () => 
                {
                    Batch tempBatch = CommonServices.StartBatchSelection();
                    if (tempBatch != null)
                    {
                        _instance.Batches.Add(tempBatch);
                    }
                },
                () => _instance != null);

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

                            _instance.AddFile(temp);
                        }
                    }
                });

            _addPO = new DelegateCommand(
                () =>
                {
                    ReportServiceProvider.AddPOToExternalReport(_instance);

                    RaisePropertyChanged("OrderCurrency");
                    RaisePropertyChanged("OrderDate");
                    RaisePropertyChanged("OrderNumber");
                    RaisePropertyChanged("OrderPrice");
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
                    _selectedFile.Delete();
                    SelectedBatch = null;
                },
                () => _selectedBatch != null);

            _removeFile = new DelegateCommand(
                () =>
                {
                    _selectedFile.Delete();
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
        
        public IEnumerable<Batch> BatchList
        {
            get { return _instance.Batches; }
        }

        public string BatchNumber
        {
            get { return _batchNumber; }
            set
            {
                _batchNumber = value;
                RaisePropertyChanged("BatchNumber");
            }
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

        public bool CanModify
        {
            get 
            {
                if (_principal.IsInRole(UserRoleNames.ExternalReportAdmin))
                    return true;
                
                else 
                    return false;
            }
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
                _instance.Load();
                
                SelectedBatch = null;
                SelectedFile = null;
                
                RaisePropertyChanged("BatchList");
                RaisePropertyChanged("Currency");
                RaisePropertyChanged("Description");
                RaisePropertyChanged("ReportFiles");
                RaisePropertyChanged("ExternalLab");
                RaisePropertyChanged("InternalNumber");
                RaisePropertyChanged("SamplesSent");
                RaisePropertyChanged("OrderCurrency");
                RaisePropertyChanged("OrderDate");
                RaisePropertyChanged("OrderNumber");
                RaisePropertyChanged("OrderPrice");
                RaisePropertyChanged("Project");
                RaisePropertyChanged("ReportReceived");
                RaisePropertyChanged("RequestDone");
                RaisePropertyChanged("Samples");
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

        public string OrderCurrency
        {
            get
            {
                try
                {
                    return _instance.PO.Currency.Code;
                }
                catch
                {
                    return null;
                }
            }
        }

        public string OrderDate
        {
            get
            {
                try
                {
                    return _instance.PO.OrderDate.ToShortDateString();
                }
                catch
                {
                    return null;
                }
            }
        }

        public string OrderNumber
        {
            get
            {
                try
                {
                    return _instance.PO.Number;
                }
                catch
                {
                    return null;
                }
            }
        }

        public string OrderPrice
        {
            get
            {
                try
                {
                    return _instance.PO.Total.ToString();
                }
                catch
                {
                    return null;
                }
            }
        }

        public DelegateCommand RemoveBatchCommand
        {
            get { return _removeBatch; }
        }

        public DelegateCommand RemoveFileCommand
        {
            get { return _removeFile; }
        }

        public IEnumerable<ExternalReportFile> ReportFiles
        {
            get { return _instance.ExternalReportFiles; }
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
                RaisePropertyChanged("SelectedBatch");
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

        public PurchaseOrder PO
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

