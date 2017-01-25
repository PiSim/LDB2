using DBManager;
using Infrastructure;
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
    class MainViewModel : BindableBase
    {
        private DBEntities _entities;
        private DelegateCommand _newReport;
        private EventAggregator _eventAggregator;
        private ObservableCollection<Report> _reportList;

        public MainViewModel(DBEntities entities, EventAggregator eventAggregator) : base()
        {
            _entities = entities;
            _eventAggregator = eventAggregator;
            _reportList = new ObservableCollection<Report>(entities.Reports);

            _newReport = new DelegateCommand(
                () =>
                {
                    
                });
        }

        public ObservableCollection<Report> ReportList
        {
            get { return _reportList; }
        }
    }
}
