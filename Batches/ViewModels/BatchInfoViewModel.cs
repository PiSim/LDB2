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
        private Batch _instance;
        private List<SamplesWrapper> _samplesList;

        internal BatchInfoViewModel(Batch instance) : base()
        {
            _instance = instance;
            _samplesList = new List<SamplesWrapper>();
            foreach (Sample smp in _instance.Samples)
                _samplesList.Add(new SamplesWrapper(smp));
        }

        public Material Material
        {
            get { return _instance.Material; }
        }

        public string Number
        {
            get { return _instance.Number; }
        }

        public Project Project
        {
            get { return _instance.Material.Construction.Project; }
        }

        public List<SamplesWrapper> Samples
        {
            get { return _samplesList; }
        }

        public ObservableCollection<DBManager.Report> Reports
        {
            get { return new ObservableCollection<Report>(_instance.Reports); }
        }
    }
}
