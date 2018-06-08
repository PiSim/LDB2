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

namespace Reports.ViewModels
{
    public class ExternalReportCreationDialogViewModel : BindableBase
    {
        private DelegateCommand<Window> _cancel, _confirm;
        private ExternalReport _externalReportInstance;
        private IDataService _dataService;
        private IReportService _reportService;
        private Organization _selectedLab;
        private Project _selectedProject;
        private string _sampleDescription, _testDescription;
        
        public ExternalReportCreationDialogViewModel(IDataService dataService,
                                                    IReportService reportService) : base()
        {
            _dataService = dataService;
            _reportService = reportService;
            _sampleDescription = "";
            _testDescription = "";
            
            _cancel = new DelegateCommand<Window>(
                parent => 
                {
                    parent.DialogResult = false;
                });
                
           _confirm = new DelegateCommand<Window>(
                parent => 
                {
                    int year = DateTime.Now.Year - 2000;

                    _externalReportInstance = new ExternalReport
                    {
                        Description = _testDescription,
                        Year = year,
                        Number = _reportService.GetNextExternalReportNumber(year),
                        MaterialSent = false,
                        RequestDone = false,
                        Samples = _sampleDescription,
                        ReportReceived = false,
                        ExternalLabID = _selectedLab.ID,
                        ProjectID = _selectedProject.ID
                    };

                    _externalReportInstance.Create();

                    parent.DialogResult = true;
                });
        }

        public ExternalReport ExternalReportInstance
        {
            get { return _externalReportInstance; }
        }
        
        public DelegateCommand<Window> CancelCommand
        {
            get { return _cancel; }
        }
        
        public DelegateCommand<Window> ConfirmCommand => _confirm;
        
        public IEnumerable<Organization> LaboratoriesList => _dataService.GetOrganizations(OrganizationRoleNames.TestLab);
        
        public string SampleDescription
        {
            get { return _sampleDescription; }
            set 
            {
                _sampleDescription = value;
            }
        }
        
        public IEnumerable<Project> ProjectList => _dataService.GetProjects();
        
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
