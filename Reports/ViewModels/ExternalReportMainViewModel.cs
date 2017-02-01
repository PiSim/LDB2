using DBManager;
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
        private ObservableCollection<ExternalReport> _reportList;
        

        internal ExternalReportMainViewModel(DBEntities entities)
        {
            _entities = entities;
            _reportList = new ObservableCollection<ExternalReport>(_entities.ExternalReports);
        } 

        public ObservableCollection<ExternalReport> ReportList
        {
            get { return _reportList; }
        }
    }
}
