using Controls.Views;
using DBManager;
using DBManager.Services;
using Infrastructure;
using Infrastructure.Events;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Materials.ViewModels
{
    public class BatchInfoViewModel : BindableBase
    {
        private Batch _instance;
        private DBPrincipal _principal;
        private DelegateCommand _newReport, _openExternalReport, _openReport;
        private EventAggregator _eventAggregator;
        private ExternalReport _selectedExternalReport;
        private List<SamplesWrapper> _samplesList;
        private Report _selectedReport;
        private IUnityContainer _container;

        public BatchInfoViewModel(DBPrincipal principal,
                                EventAggregator aggregator,
                                IUnityContainer container) : base()
        {
            _container = container;
            _eventAggregator = aggregator;
            _principal = principal;

            _eventAggregator.GetEvent<ReportListUpdateRequested>().Subscribe(
                () =>
                {
                    SelectedReport = null;
                    RaisePropertyChanged("ReportList");
                }); 
               
            _newReport = new DelegateCommand(
                () => 
                {
                    NewReportToken token = new NewReportToken(_instance);
                    _eventAggregator.GetEvent<ReportCreationRequested>().Publish(token);
                }
            );
            
            _openExternalReport = new DelegateCommand(
                () => 
                {
                    NavigationToken token = new NavigationToken(ReportViewNames.ExternalReportEditView,
                                                                _selectedExternalReport);
                    _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
                },
                () => _selectedExternalReport != null);

            _openReport = new DelegateCommand(
                () => 
                {
                    NavigationToken token = new NavigationToken(ReportViewNames.ReportEditView,
                                                                _selectedReport);
                    _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
                },
                () => _selectedReport != null);
        }

        public Batch BatchInstance
        {
            get { return _instance; }
            set
            {
                _instance = value;
                _instance.Load();


                _samplesList = _instance.Samples.Select(smp => new SamplesWrapper(smp)).ToList();

                SelectedExternalReport = null;
                SelectedReport = null;
                
                RaisePropertyChanged("ExternalReportList");
                RaisePropertyChanged("Samples");
                RaisePropertyChanged("Material");
                RaisePropertyChanged("Number");
                RaisePropertyChanged("Project");
                RaisePropertyChanged("ReportList");
            }
        }
        
        public bool CanCreateReport
        {
            get
            {
                return _principal.IsInRole(UserRoleNames.ReportEdit);
            }
        }
        
        public IEnumerable<ExternalReport> ExternalReportList 
        {
            get 
            { 
                if (_instance == null)
                    return null;
                    
                return _instance.ExternalReports; 
            }
        }

        public Material Material
        {
            get
            {
                if (_instance == null)
                    return null;
                else
                    return _instance.Material;
            }
        }
        
        public DelegateCommand NewReportCommand
        {
            get { return _newReport;}
        }

        public string Number
        {
            get
            {
                if (_instance == null)
                    return null;
                else
                    return _instance.Number;
            }
        }

        public DelegateCommand OpenExternalReportCommand
        {
            get { return _openExternalReport; }
        }

        public DelegateCommand OpenReportCommand
        {
            get { return _openReport; }
        }

        public Project Project
        {
            get
            {
                try
                {
                    return _instance.Material.Construction.Project;
                }
                catch
                {
                    return null;
                }
            }
        }

        public List<SamplesWrapper> Samples
        {
            get { return _samplesList; }
        }

        public IEnumerable<Report> ReportList
        {
            get { return _instance.Reports; }
        }
        
        public Report SelectedReport
        {
            get { return _selectedReport; }
            set 
            {
                _selectedReport = value; 
                _openReport.RaiseCanExecuteChanged();
            }
        }

        public ExternalReport SelectedExternalReport
        {
            get { return _selectedExternalReport; }
            set 
            {
                _selectedExternalReport = value;
                _openExternalReport.RaiseCanExecuteChanged();
            }
        }
    }
}
