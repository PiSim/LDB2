using DBManager;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standards.ViewModels
{
    internal class StandardMainViewModel : BindableBase
    {
        private DBEntities _entities;
        private ObservableCollection<Std> _stdList;

        internal StandardMainViewModel(DBEntities entities) : base()
        {
            _entities = entities;
            _stdList = new ObservableCollection<Std>(_entities.Stds);
        }

        public ObservableCollection<Std> StandardList
        {
            get { return _stdList; }
        }
    }
}
