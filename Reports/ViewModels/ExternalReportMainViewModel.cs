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
    public class ExternalReportMainViewModel : BindableBase
    {
        private DBEntities _entities;
        private DelegateCommand _newReport, _openReport;
        private EventAggregator _eventAggregator;
        private ExternalReport _selectedReport;
        private ObservableCollection<ExternalReport> _reportList;      
        private UnityContainer _container; 

        public ExternalReportMainViewModel(DBEntities entities, 
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
                        SelectedExternalReport = tempReport;
                        _openReport.Execute();
                    }
                });

            _openReport = new DelegateCommand(
                () =>
                {
                    NavigationToken token = new NavigationToken(ViewNames.ExternalReportEditView,
                                                                _selectedReport);

                    _eventAggregator.GetEvent<Infrastructure.Events.NavigationRequested>()
                        .Publish(token);
                },
                () => _selectedReport != null);
        } 
        
        public DelegateCommand NewExternalReportCommand
        {
            get { return _newReport; }
        }

        public DelegateCommand OpenExternalReportCommand
        {
            get { return _openReport; }
        }

        public ObservableCollection<ExternalReport> ExternalReportList
        {
            get { return _reportList; }
        }

        public ExternalReport SelectedExternalReport
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
