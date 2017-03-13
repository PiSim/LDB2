using Controls.Views;
using DBManager;
using Infrastructure;
using Infrastructure.Events;
using Microsoft.Practices.Unity;
using Reports;
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
        private DelegateCommand _newReport, _openReport;
        private EventAggregator _eventAggregator;
        private List<SamplesWrapper> _samplesList;
        private Report _selectedReport;
        private IUnityContainer _container;

        public BatchInfoViewModel(EventAggregator aggregator,
                                IUnityContainer container) : base()
        {
            _container = container;
            _eventAggregator = aggregator;
                
            _newReport = new DelegateCommand(
                () => 
                {
                    ReportCreationDialog reportDialog = _container.Resolve<ReportCreationDialog>();
                    if (reportDialog.ShowDialog() == true)
                    {
                        SelectedReport = reportDialog.ReportInstance;
                        _openReport.Execute();
                    }
                    
                }
            );
            
            _openReport = new DelegateCommand(
                () => 
                {
                    NavigationToken token = new NavigationToken(Reports.ViewNames.ReportEditView,
                                                                _selectedReport);
                    _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
                }
            );
                
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
                OnPropertyChanged("Samples");
                OnPropertyChanged("Material");
                OnPropertyChanged("Number");
                OnPropertyChanged("Project");
                OnPropertyChanged("ReportList");
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
            set { _selectedReport = value; }
        }
    }
}
