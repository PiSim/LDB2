using DBManager;
using Infrastructure;
using Infrastructure.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Specifications.ViewModels
{
    internal class SpecificationMainViewModel : BindableBase
    {
        private DBEntities _entities;
        private DelegateCommand _newSpecification, _openSpecification;
        private EventAggregator _eventAggregator;
        private ObservableCollection<Specification> _specificationList;
        private Specification _selectedSpecification;

        internal SpecificationMainViewModel(DBEntities entities, EventAggregator aggregator) 
            : base()
        {
            _entities = entities;
            _eventAggregator = aggregator;
            _newSpecification = new DelegateCommand(
                () => { });
            _openSpecification = new DelegateCommand(
                () => {
                    ObjectNavigationToken token = new ObjectNavigationToken(SelectedSpecification, ViewNames.SpecificationsEditView);
                    _eventAggregator.GetEvent<VisualizeObjectRequested>().Publish(token);
                });
            _specificationList = new ObservableCollection<Specification>(
                _entities.Specifications);
        }

        public DelegateCommand OpenSpecificationCommand
        {
            get { return _openSpecification; }
        }

        public ObservableCollection<Specification> SpecificationList
        {
            get { return _specificationList; }
        }

        public Specification SelectedSpecification
        {
            get { return _selectedSpecification; }
            set { _selectedSpecification = value; }
        }
    }
}
