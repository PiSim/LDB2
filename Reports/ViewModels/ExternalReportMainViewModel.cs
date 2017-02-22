using DBManager;
using Infrastructure;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reports.ViewModels
{
    internal class ExternalReportMainViewModel : BindableBase
    {
        private DBEntities _entities;
        private DelegateCommand _newReport, _openReport;
        private EventAggregator _eventAggregator;
        private ExternalReport _selectedReport;
        private ObservableCollection<ExternalReport> _reportList;      
        private UnityContainer _container; 

        internal ExternalReportMainViewModel(DBEntities entities, 
                                            EventAggregator aggregator,
                                            UnityContainer container)
        {
            _container = container;
            _entities = entities;
            _eventAggregator = aggregator;
            _reportList = new ObservableCollection<ExternalReport>(_entities.ExternalReports);
            
            _newReport = new DelegateCommand(
                () => 
                {
                    Views.ExternalReportCreationDialog creationDialog = _container.Resolve<Views.ExternalReportCreationDialog>();
                    if (creationDialog.ShowDialog() == true)
                    {
                        ExternalReport tempReport = creationDialog.ExternalReportInstance;
                        _reportList.Add(tempReport);
                        SelectedReport = tempReport;
                        _openReport.Execute();
                    }
                });

            _openReport = new DelegateCommand(
                () =>
                {
                    ObjectNavigationToken token = 
                        new ObjectNavigationToken(_selectedReport, ViewNames.ExternalReportEditView);

                    _eventAggregator.GetEvent<Infrastructure.Events.VisualizeObjectRequested>()
                        .Publish(token);
                },
                () => _selectedReport != null);
        } 
        
        public DelegateCommand NewReportCommand
        {
            get { return _newReport; }
        }

        public DelegateCommand OpenReportCommand
        {
            get { return _openReport; }
        }

        public ObservableCollection<ExternalReport> ReportList
        {
            get { return _reportList; }
        }

        public ExternalReport SelectedReport
        {
            get { return _selectedReport; }
            set 
            { 
                _selectedReport = value; 
                _openReport.RaiseCanExecuteChanged();
            }
        }
    }
}
