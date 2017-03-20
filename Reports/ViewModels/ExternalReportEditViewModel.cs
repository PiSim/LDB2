using DBManager;
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
        private DBEntities _entities;
        private DelegateCommand _addBatch, _addFile, _openBatch, _openFile, _removeBatch, _removeFile;
        private EventAggregator _eventAggregator;
        private ExternalReport _instance;
        private ExternalReportFile _selectedFile;
        private IMaterialServiceProvider _materialServiceProvider;
        private ObservableCollection<Batch> _batchList;
        private ObservableCollection<ExternalReportFile> _reportFiles;

        public ExternalReportEditViewModel(DBEntities entities,
                                            EventAggregator aggregator,
                                            IMaterialServiceProvider materialServiceProvider) : base()
        {
            _eventAggregator = aggregator;
            _materialServiceProvider = materialServiceProvider;
            _entities = entities;
            _eventAggregator.GetEvent<CommitRequested>().Subscribe(() => _entities.SaveChanges());

            _batchList = new ObservableCollection<Batch>
                (_instance.BatchMappings.Select(btm => btm.Batch));
            
            _reportFiles = new ObservableCollection<ExternalReportFile>
                (_instance.ExternalReportFiles);

            _addBatch = new DelegateCommand(
                () => 
                {
                    
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
                            ReportFiles.Add(temp);   
                            _instance.ExternalReportFiles.Add(temp);
                        }
                    }
                });

            _openFile = new DelegateCommand(
                () =>
                {
                    System.Diagnostics.Process.Start(_selectedFile.Path);
                },
                () => _selectedFile != null);

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
        
        public ObservableCollection<Batch> BatchList
        {
            get { return _batchList; }
        }

        public string Description
        {
            get { return _instance.Description; }
            set { _instance.Description = value; }
        }

        public string Currency
        {
            get { return _instance.Currency; }
            set { _instance.Currency = value; }
        }
        
        public string ExternalLab
        {
            get { return _instance.ExternalLab.Name; }
        }
        
        public int InternalNumber
        {
            get { return _instance.InternalNumber; }
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

        public double Price
        {
            get { return _instance.Price; }
            set { _instance.Price = value; }
        }
        
        public Project Project
        {
            get { return _instance.Project; }
        }

        public string PurchaseOrder
        {
            get { return _instance.purchase_order; }
            set { _instance.purchase_order = value; }
        }

        public string Samples
        {
            get { return _instance.Samples; }
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

