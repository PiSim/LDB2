using DBManager;
using Infrastructure;
using Infrastructure.Events;
using Microsoft.Practices.Unity;
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
        private UnityContainer _container;

        internal SpecificationMainViewModel(DBEntities entities, 
                                            EventAggregator aggregator,
                                            UnityContainer container) 
            : base()
        {
            _container = container;
            _entities = entities;
            _eventAggregator = aggregator;
            _specificationList = new ObservableCollection<Specification>(
                _entities.Specifications);

            _newSpecification = new DelegateCommand(
                () => 
                {
                    Views.SpecificationCreationDialog creationDialog = 
                        _container.Resolve<Views.SpecificationCreationDialog>();
                    
                    if (creationDialog.ShowDialog() == true)
                    {
                        Specification temp = creationDialog.SpecificationInstance;
                        _specificationList.Add(temp);
                        SelectedSpecification = temp;
                        _openSpecification.Execute();
                    }
                                            
                });

            _openSpecification = new DelegateCommand(
                () => 
                {
                    ObjectNavigationToken token = new ObjectNavigationToken(SelectedSpecification, ViewNames.SpecificationsEditView);
                    _eventAggregator.GetEvent<VisualizeObjectRequested>().Publish(token);
                },
                () => SelectedSpecification != null);
        }

        public DelegateCommand NewSpecificationCommand
        {
            get { return _newSpecification; }
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
            set
            {
                _selectedSpecification = value;
                _openSpecification.RaiseCanExecuteChanged();
            }
        }
    }
}
