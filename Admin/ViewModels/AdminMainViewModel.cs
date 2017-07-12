﻿using DBManager;
using DBManager.EntityExtensions;
using DBManager.Services;
using Infrastructure;
using Infrastructure.Events;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.ViewModels
{
    public class AdminMainViewModel : BindableBase
    {
        private DelegateCommand _newOrganizationRole, _newPersonRole, _newUserRole, _runMethod;
        private EventAggregator _eventAggregator;
        private string _name;

        public AdminMainViewModel(EventAggregator eventAggregator) : base()
        {
            _eventAggregator = eventAggregator;

            _newOrganizationRole = new DelegateCommand(
                () =>
                {
                    _eventAggregator.GetEvent<OrganizationRoleCreationRequested>()
                                    .Publish();
                });

            _newPersonRole = new DelegateCommand(
                () =>
                {
                    _eventAggregator.GetEvent<PersonRoleCreationRequested>()
                                    .Publish();
                });

            _newUserRole = new DelegateCommand(
                () =>
                {
                    _eventAggregator.GetEvent<UserRoleCreationRequested>()
                                    .Publish();
                });

            _runMethod = new DelegateCommand(
                () =>
                {
                    using (DBEntities entities = new DBEntities())
                    {
                        IEnumerable<Batch> negativeStockBatches = entities.Batches.Where(btc => btc.ArchiveStock < 0)
                                                                                   .ToList();

                        foreach (Batch btc in negativeStockBatches)
                        {
                            DateTime tempDate = btc.Samples.First().Date;

                            for (int ii = btc.ArchiveStock; ii < 0; ii++)
                            {
                                entities.Samples.Add(new Sample()
                                {
                                    Code = "A",
                                    Batch = btc,
                                    Date = tempDate,
                                    personID = 1
                                });

                                btc.ArchiveStock += 1;
                            }
                            
                        }

                        entities.SaveChanges();
                    }
                } );
        }

        public string AdminUserMainRegionName
        {
            get { return RegionNames.AdminUserMainRegion; }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
            }
        }

        public DelegateCommand AddPersonRoleCommand
        {
            get { return _newPersonRole; }
        }

        public string MeasurableQuantityManagementRegionName
        {
            get { return RegionNames.MeasurableQuantityManagementRegion; }
        }

        public DelegateCommand NewOrganizationRoleCommand
        {
            get { return _newOrganizationRole; }
        }

        public DelegateCommand NewUserRoleCommand
        {
            get { return _newUserRole; }
        }

        public string OrganizationRoleManagementRegionName
        {
            get { return RegionNames.OrganizationRoleManagementRegion; }
        }

        public IEnumerable<PersonRole> PersonRoleList
        {
            get { return PeopleService.GetPersonRoles(); }
        }

        public DelegateCommand RunMethodCommand
        {
            get { return _runMethod; }
        }
    }
}
