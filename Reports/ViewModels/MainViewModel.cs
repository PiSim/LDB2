using DBManager;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reports.ViewModels
{
    class MainViewModel : BindableBase
    {
        private DBEntities _entities;
        private List<Report> _recentList;

        public MainViewModel(DBEntities entities) : base()
        {
            _entities = entities;
            int ii = 0;
            _recentList = new List<Report>(entities.Reports);
        }

        public List<Report> RecentList
        {
            get { return _recentList; }
        }
    }
}
