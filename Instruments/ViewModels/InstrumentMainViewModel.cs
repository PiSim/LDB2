using DBManager;
using Infrastructure;
using Infrastructure.Events;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
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
        private DelegateCommand _newInstrument, _openInstrument;
        private DBEntities _entities;
        private EventAggregator _eventAggregator;
        private Instrument _selectedInstrument;
        IUnityContainer _container;

        public InstrumentMainViewModel(UnityContainer container) : base()
        {
            _container = container;
            _entities = _container.Resolve<DBEntities>();
            _eventAggregator = _container.Resolve<EventAggregator>();

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

            _openInstrument = new DelegateCommand(
                () =>
                {
                    NavigationToken token = new NavigationToken(ViewNames.InstrumentEditView,
                                                                SelectedInstrument);
                    _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
                },
                () => SelectedInstrument != null);
        }

        public List<Instrument> InstrumentList
        {
            get { return new List<Instrument>(_entities.Instruments); }
        }

        public DelegateCommand NewInstrumentCommand
        {
            get { return _newInstrument; }
        }

        public DelegateCommand OpenInstrumentCommand
        {
            get { return _openInstrument; }
        }

        public Instrument SelectedInstrument
        {
            get { return _selectedInstrument; }
            set
            {
                _selectedInstrument = value;
                _openInstrument.RaiseCanExecuteChanged();
            }
        }

    }
}
