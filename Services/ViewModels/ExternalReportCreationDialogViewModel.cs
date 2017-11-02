using DBManager;
using DBManager.EntityExtensions;
using DBManager.Services;
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
using System.Windows;
using System.Windows.Controls;

namespace Services.ViewModels
{
    public class ExternalReportCreationDialogViewModel : BindableBase
    {
        private Batch _selectedBatch;
        private DelegateCommand _removeBatch;
        private DelegateCommand<TextBox> _addBatch;
        private DelegateCommand<Window> _cancel, _confirm;
        private ExternalReport _externalReportInstance;
        private ObservableCollection<Batch> _batchList;
        private Organization _selectedLab;
        private Project _selectedProject;
        private string _batchNumber, _sampleDescription, _testDescription;
        
        public ExternalReportCreationDialogViewModel() : base()
        {
            _batchList = new ObservableCollection<Batch>();
            _sampleDescription = "";
            _testDescription = "";

            _addBatch = new DelegateCommand<TextBox>(
                batchBox => 
                {
                    Batch temp = MaterialService.GetBatch(batchBox.Text);
                    if (temp == null)
                    {
                        temp = new Batch
                        {
                            Number = batchBox.Text
                        };
                        temp.Create();
                    }

                    _batchList.Add(temp);                 
                });
            
            _cancel = new DelegateCommand<Window>(
                parent => 
                {
                    parent.DialogResult = false;
                });
                
           _confirm = new DelegateCommand<Window>(
                parent => 
                {
                    _externalReportInstance = new ExternalReport
                    {
                        Description = _testDescription,
                        Year = DateTime.Now.Year - 2000,
                        Number = ReportService.GetNextExternalReportNumber(_externalReportInstance.Year),
                        ExternalNumber = "",
                        MaterialSent = false,
                        RequestDone = false,
                        Samples = _sampleDescription,
                        ReportReceived = false,
                        ExternalLabID = _selectedLab.ID,
                        ProjectID = _selectedProject.ID
                    };

                    _externalReportInstance.Create();
                    foreach (Batch btc in _batchList)
                            _externalReportInstance.AddBatch(btc);

                    parent.DialogResult = true;
                });
                
            _removeBatch = new DelegateCommand(
                () => 
                {
                    _batchList.Remove(_selectedBatch);
                    SelectedBatch = null;
                },
                () => _selectedBatch != null);
        }
        
        
        public DelegateCommand<TextBox> AddBatchCommand
        {
            get { return _addBatch; }
        }

        public ExternalReport ExternalReportInstance
        {
            get { return _externalReportInstance; }
        }

        public ObservableCollection<Batch> BatchList
        {
            get { return _batchList; }
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

        public DelegateCommand<Window> CancelCommand
        {
            get { return _cancel; }
        }
        
        public DelegateCommand<Window> ConfirmCommand
        {
            get { return _confirm; }
        }
        
        public DelegateCommand RemoveBatchCommand
        {
            get { return _removeBatch; }
        }

        public IEnumerable<Organization> LaboratoriesList 
        {
            get { return OrganizationService.GetOrganizations(OrganizationRoleNames.TestLab); }
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
        
        public IEnumerable<Project> ProjectList
        {
            get { return ProjectService.GetProjects(); }
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
