﻿using DBManager;
using DBManager.Services;
using Infrastructure.Events;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Specifications.ViewModels
{
    public class SpecificationMainViewModel : BindableBase
    {
        private DelegateCommand _newSpecification, _openSpecification;
        private EventAggregator _eventAggregator;
        private Specification _selectedSpecification;
        private UnityContainer _container;

        public SpecificationMainViewModel(EventAggregator aggregator,
                                            UnityContainer container) 
            : base()
        {
            _container = container;
            _eventAggregator = aggregator;

            _newSpecification = new DelegateCommand(
                () => 
                {
                    Views.SpecificationCreationDialog creationDialog = 
                        _container.Resolve<Views.SpecificationCreationDialog>();
                    
                    if (creationDialog.ShowDialog() == true)
                    {
                        Specification temp = creationDialog.SpecificationInstance;
                        SelectedSpecification = temp;
                        _openSpecification.Execute();
                        RaisePropertyChanged("SpecificationList");
                    }
                                            
                });

            _openSpecification = new DelegateCommand(
                () => 
                {
                    NavigationToken token = new NavigationToken(ViewNames.SpecificationsEditView,
                                                                SelectedSpecification);
                    _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
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

        public IEnumerable<Specification> SpecificationList
        {
            get { return SpecificationService.GetSpecifications(); }
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
