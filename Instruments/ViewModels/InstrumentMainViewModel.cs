using DBManager;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instruments.ViewModels
{
    public class InstrumentMainViewModel : BindableBase
    {
        private DelegateCommand _newInstrument;
        private DBEntities _entities;
        IUnityContainer _container;

        public InstrumentMainViewModel(UnityContainer container) : base()
        {
            _container = container;
            _entities = _container.Resolve<DBEntities>();

            _newInstrument = new DelegateCommand(
                () =>
                {
                    Views.InstrumentCreationDialog creationDialog =
                        _container.Resolve<Views.InstrumentCreationDialog>();

                    if (creationDialog.ShowDialog() == true)
                    {
                        OnPropertyChanged("InstrumentList");
                    }

                });
        }

        public List<Instrument> InstrumentList
        {
            get { return new List<Instrument>(_entities.Instruments); }
        }

        public DelegateCommand NewInstrumentCommand
        {
            get { return _newInstrument; }
        }

    }
}
