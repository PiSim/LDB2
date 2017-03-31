using Controls.Views;
using DBManager;
using Infrastructure;
using Infrastructure.Events;
using Infrastructure.Tokens;
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
        private DBEntities _entities;
        private DelegateCommand _newReport, _openExternalReport, _openReport;
        private EventAggregator _eventAggregator;
        private ExternalReport _selectedExternalReport;
        private List<SamplesWrapper> _samplesList;
        private Report _selectedReport;
        private IUnityContainer _container;

        public BatchInfoViewModel(DBEntities entities,
                                EventAggregator aggregator,
                                IUnityContainer container) : base()
        {
            _container = container;
            _entities = entities;
            _eventAggregator = aggregator;

            _eventAggregator.GetEvent<ReportCreated>().Subscribe(
                rpt => OnPropertyChanged("ReportList")); 
               
            _newReport = new DelegateCommand(
                () => 
                {
                    NewReportToken token = new NewReportToken();
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

                _samplesList = new List<SamplesWrapper>();
                foreach (Sample smp in _instance.Samples)
                    _samplesList.Add(new SamplesWrapper(smp));

                SelectedExternalReport = null;
                SelectedReport = null;
                
                OnPropertyChanged("ExternalReportList");
                OnPropertyChanged("Samples");
                OnPropertyChanged("Material");
                OnPropertyChanged("Number");
                OnPropertyChanged("Project");
                OnPropertyChanged("ReportList");
            }
        }
        
        public List<ExternalReport> ExternalReportList 
        {
            get 
            { 
                if (_instance == null)
                    return null;
                    
                return new List<ExternalReport>(_entities.ExternalReports
                                                        .Where(xtr => xtr.BatchMappings
                                                        .Any(btm => btm.BatchID == _instance.ID))); 
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

        public ObservableCollection<DBManager.Report> ReportList
        {
            get { return new ObservableCollection<Report>(_instance.Reports); }
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
