using DBManager;
using Infrastructure;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reports.ViewModels
{
    internal class ExternalReportCreationViewModel : BindableBase
    {
        private Batch _selectedBatch;
        private DBEntities _entities;
        private DelegateCommand _addBatch, _cancel, _confirm, _removeBatch;
        private IMaterialServiceProvider _materialServiceProvider;
        private Int32 _number;
        private ObservableCollection<Batch> _batchList;
        private Organization _selectedLab;
        private Project _selectedProject;
        private string _sampleDescription, _testDescription;
        private Views.ExternalReportCreationDialog _parentDialog;
        
        internal ExternalReportCreationViewModel(DBEntities entities,
                                                IMaterialServiceProvider materialServiceProvider,
                                                Views.ExternalReportCreationDialog parentDialog) : base()
        {
            _batchList = new ObservableCollection<Batch>();
            _materialServiceProvider = materialServiceProvider;
            _entities = entities;
            _parentDialog = parentDialog;
            
            _addBatch = new DelegateCommand(
                () => 
                {
                    Batch temp = _materialServiceProvider.StartBatchSelection();
                    _batchList.Add(temp);                                      
                });
            
            _cancel = new DelegateCommand(
                () => 
                {
                    _parentDialog.DialogResult = false;
                });
                
           _confirm = new DelegateCommand(
                () => 
                {
                    ExternalReport output = new ExternalReport();
                    output.Description = _testDescription;
                    output.InternalNumber = _number;
                    output.ExternalNumber = "";
                    output.MaterialSent = false;
                    output.RequestDone = false;
                    output.PurchaseOrder = "";
                    output.Price = 0;
                    output.Samples = _sampleDescription;
                    output.Currency = "";
                    output.ReportReceived = false;
                    output.ExternalLab = _selectedLab;
                    output.Project = _selectedProject;
                    
                    foreach (Batch selectedBatch in _batchList)
                    {
                        ExternalReportBatchMapping tempMap = new ExternalReportBatchMapping();
                        tempMap.Batch = _entities.Batches.First(btc => btc.ID == selectedBatch.ID);
                        
                        output.BatchMappings.Add(tempMap);
                    }

                    _entities.ExternalReports.Add(output);
                    _entities.SaveChanges();

                    _parentDialog.ExternalReportInstance = output;
                    _parentDialog.DialogResult = true;
                });
                
            _removeBatch = new DelegateCommand(
                () => 
                {
                    _batchList.Remove(_selectedBatch);
                    SelectedBatch = null;
                },
                () => _selectedBatch != null);
        }
        
        
        public DelegateCommand AddBatchCommand
        {
            get { return _addBatch; }
        }

        public ObservableCollection<Batch> BatchList
        {
            get { return _batchList; }
        }

        public DelegateCommand CancelCommand
        {
            get { return _cancel; }
        }
        
        public DelegateCommand ConfirmCommand
        {
            get { return _confirm; }
        }
        
        public DelegateCommand RemoveBatchCommand
        {
            get { return _removeBatch; }
        }
        
        public Int32 Number 
        {
            get { return _number; }
            set { _number = value; }
        }
        
        public List<Organization> LaboratoriesList 
        {
            get { return new List<Organization>(_entities.Organizations.Where(org => org.Category == "Laboratorio")); }
        }
        
        public string SampleDescription
        {
            get { return _sampleDescription; }
            set 
            {
                _sampleDescription = value;
            }
        }
        
        public Batch SelectedBatch
        {
            get { return _selectedBatch; }
            set
            {
                _selectedBatch = value;
                RemoveBatchCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged("SelectedBatch");
            }
        }
        
        public List<Project> ProjectList
        {
            get { return new List<Project>(_entities.Projects.OrderBy(prj => prj.Name)); }
        }
        
        public Organization SelectedLab
        {
            get { return _selectedLab; }
            set { _selectedLab = value; }
        }
        
        public Project SelectedProject
        {
            get { return _selectedProject; }
            set
            {
                _selectedProject = value;
            }
        }
        
        public string TestDescription
        {
            get { return _testDescription; }
            set 
            {
                _testDescription = value;    
            }
        }
    }
}
