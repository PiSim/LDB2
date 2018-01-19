using DBManager;
using DBManager.EntityExtensions;
using DBManager.Services;
using Infrastructure;
using Infrastructure.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
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
        private bool _editMode;
        private Batch _selectedBatch;
        private DBPrincipal _principal;
        private DelegateCommand _addBatch, 
                                _addFile, 
                                _addPO, 
                                _openBatch, 
                                _openFile, 
                                _removeBatch, 
                                _removeFile,
                                _save,
                                _startEdit;
        private EventAggregator _eventAggregator;
        private ExternalReport _instance;
        private ExternalReportFile _selectedFile;
        private IMaterialService _materialService;
        private IReportService _reportService;
        private string _batchNumber;

        public ExternalReportEditViewModel(DBPrincipal principal,
                                            EventAggregator aggregator,
                                            IMaterialService materialService,
                                            IReportService reportService) : base()
        {
            _editMode = false;
            _eventAggregator = aggregator;
            _materialService = materialService;
            _principal = principal;
            _reportService = reportService;

            _addBatch = new DelegateCommand(
                () => 
                {
                    Batch tempBatch = _materialService.StartBatchSelection();
                    if (tempBatch != null)
                    {
                        _instance.AddBatch(tempBatch);
                        RaisePropertyChanged("BatchList");
                    }
                },
                () => _instance != null);

            _addFile = new DelegateCommand(
                () =>
                {
                    OpenFileDialog fileDialog = new OpenFileDialog
                    {
                        InitialDirectory = UserSettings.ExternalReportPath,
                        Multiselect = true
                    };

                    if (fileDialog.ShowDialog() == DialogResult.OK)
                    {
                        foreach (string pth in fileDialog.FileNames)
                        {
                            ExternalReportFile temp = new ExternalReportFile();
                            temp.Path = pth;
                            temp.Description = "";
                            temp.ExternalReportID = _instance.ID;

                            temp.Create();
                        }

                        RaisePropertyChanged("ReportFiles");
                    }
                });

            _addPO = new DelegateCommand(
                () =>
                {
                    _reportService.AddPOToExternalReport(_instance);

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
                    _selectedBatch.Delete();
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

            _save = new DelegateCommand(
                () =>
                {
                    _instance.Update();
                    EditMode = false;
                },
                () => _editMode);

            _startEdit = new DelegateCommand(
                () =>
                {
                    EditMode = true;
                },
                () => !_editMode && CanModify);
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
            get { return _instance.GetBatches(); }
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
                
        public bool EditMode
        {
            get { return _editMode; }
            set
            {
                _editMode = value;
                RaisePropertyChanged("EditMode");
                _save.RaiseCanExecuteChanged();
                _startEdit.RaiseCanExecuteChanged();
            }
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
                EditMode = false;

                _instance = value;
                _instance.Load();
                
                SelectedBatch = null;
                SelectedFile = null;

                _addBatch.RaiseCanExecuteChanged();
                _removeBatch.RaiseCanExecuteChanged();

                RaisePropertyChanged("BatchList");
                RaisePropertyChanged("Currency");
                RaisePropertyChanged("Description");
                RaisePropertyChanged("ReportFiles");
                RaisePropertyChanged("ExternalLab");
                RaisePropertyChanged("FormattedNumber");
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
        
        public string FormattedNumber
        {
            get 
            { 
                return _instance?.FormattedNumber; 
            }
        }
        
        public DelegateCommand OpenBatchCommand
        {
            get { return _openBatch; }
        }

        public DelegateCommand OpenFileCommand
        {
            get { return _openFile; }
        }

        public string OrderNumber
        {

            get
            {
                if (_instance == null || _instance.PO == null)
                    return null;

                return _instance.PO.Number;
            }
        }

        public string OrderPrice
        {
            get
            {
                return _instance?.PO?.Total.ToString();
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
            get { return _instance.GetExternalReportFiles(); }
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

        public DelegateCommand SaveCommand
        {
            get { return _save; }
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

        public DelegateCommand StartEditCommand
        {
            get { return _startEdit; }
        }
    }
}

