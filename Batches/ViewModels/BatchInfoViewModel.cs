using DBManager;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Batches.ViewModels
{
    class BatchInfoViewModel : BindableBase
    {
        Batch _instance;

        internal BatchInfoViewModel(Batch instance) : base()
        {
            _instance = instance;
        }

        public DBManager.Material Material
        {
            get { return _instance.Material; }
        }

        public string Number
        {
            get { return _instance.Number; }
        }

        public ObservableCollection<Sample> Samples
        {
            get { return new ObservableCollection<Sample>(_instance.Samples); }
        }

        public ObservableCollection<DBManager.Report> Reports
        {
            get { return new ObservableCollection<Report>(_instance.Reports); }
        }
    }
}
