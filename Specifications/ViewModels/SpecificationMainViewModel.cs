﻿using DBManager;
using DBManager.Services;
using Infrastructure;
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
        private DBPrincipal _principal;
        private DelegateCommand _newSpecification, _openSpecification;
        private EventAggregator _eventAggregator;
        private readonly IDataService _dataService;
        private readonly ISpecificationService _specificationService;
        private Specification _selectedSpecification;

        public SpecificationMainViewModel(DBPrincipal principal,
                                            EventAggregator aggregator,
                                            IDataService dataService,
                                            ISpecificationService specificationService) 
            : base()
        {
            _dataService = dataService;
            _eventAggregator = aggregator;
            _principal = principal;
            _specificationService = specificationService;

            _newSpecification = new DelegateCommand(
                () => 
                {
                    _specificationService.CreateSpecification();

                },
                () => _principal.IsInRole(UserRoleNames.SpecificationEdit));

            _openSpecification = new DelegateCommand(
                () => 
                {
                    NavigationToken token = new NavigationToken(SpecificationViewNames.SpecificationEdit,
                                                                SelectedSpecification);
                    _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
                },
                () => SelectedSpecification != null);


            #region EventSubscriptions

            _eventAggregator.GetEvent<SpecificationChanged>()
                            .Subscribe(token =>
                            {
                                RaisePropertyChanged("SpecificationList");
                            });

            #endregion

        }

        public DelegateCommand NewSpecificationCommand
        {
            get { return _newSpecification; }
        }

        public DelegateCommand OpenSpecificationCommand
        {
            get { return _openSpecification; }
        }

        public IEnumerable<Specification> SpecificationList => _dataService.GetSpecifications();

        public string SpecificationMainListRegionName
        {
            get { return RegionNames.SpecificationMainListRegion; }
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
