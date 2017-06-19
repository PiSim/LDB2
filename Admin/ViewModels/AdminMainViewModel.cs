using DBManager;
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
        private DelegateCommand _newOrganizationRole, _newPersonRole, _newUser, _newUserRole, _runMethod;
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

            _newUser = new DelegateCommand(
                () =>
                {
                    _eventAggregator.GetEvent<UserCreationRequested>()
                                    .Publish();
                });

            _newUserRole = new DelegateCommand(
                () =>
                {
                    _eventAggregator.GetEvent<UserRoleCreationRequested>();
                });

            _runMethod = new DelegateCommand(
                () =>
                {} );
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

        public DelegateCommand NewOrganizationRoleCommand
        {
            get { return _newOrganizationRole; }
        }

        public DelegateCommand NewUserRoleCommand
        {
            get { return _newUserRole; }
        }

        public DelegateCommand NewUserCommand
        {
            get { return _newUser; }
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
