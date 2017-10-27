using DBManager;
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
        private readonly Dictionary<string, int> _stockModifiers = new Dictionary<string, int>()
                                                                {
                                                                    {"A", 1 },
                                                                    {"B", -1 },
                                                                    {"F", -1 },
                                                                    {"M", -1 },
                                                                    {"S", -1 }
                                                                };
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
                    _eventAggregator.GetEvent<ProjectCostUpdateRequested>()
                                    .Publish();
                        
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

        public string InstrumentTypeManagementRegionName
        {
            get { return RegionNames.InstrumentTypeManagementRegion; }
        }

        public string InstrumentUtilizationAreasRegionName
        {
            get { return RegionNames.InstrumentUtilizationAreasRegion; }
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

        public string UnitOfMeasurementManagementRegionName
        {
            get { return RegionNames.UnitOfMeasurementManagementRegion; }
        }

        private void mtd()
        {
            //IEnumerable<Batch> btcList = entities.Batches.ToList();

            //foreach (Batch btc in btcList)
            //{
            //    Sample tmp = btc.Samples.Where(smp => smp.Code == "A").OrderByDescending(smp => smp.Date).FirstOrDefault();
            //    btc.LatestSampleID = tmp?.ID;
            //}

            //entities.SaveChanges();
        }
    }
}
