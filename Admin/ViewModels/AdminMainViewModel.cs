using DBManager;
using Infrastructure;
using Microsoft.Practices.Unity;
using Prism.Commands;
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
        private DBEntities _entities;
        private DelegateCommand _newOrganizationRole, _newPersonRole, _newUser, _newUserRole, _runMethod;
        private IAdminServiceProvider _adminServiceProvider;
        private IUserServiceProvider _userServiceProvider;
        private string _name;

        public AdminMainViewModel(DBEntities entities,
                                    IAdminServiceProvider services,
                                    IUserServiceProvider userServiceProvider) : base()
        {
            _adminServiceProvider = services;
            _entities = entities;
            _userServiceProvider = userServiceProvider;

            _newOrganizationRole = new DelegateCommand(
                () =>
                {
                    services.AddOrganizationRole(_name);
                });

            _newPersonRole = new DelegateCommand(
                () =>
                {
                    services.AddPersonRole();
                    RaisePropertyChanged("PersonRoleList");
                });

            _newUser = new DelegateCommand(
                () =>
                {
                    
                });

            _newUserRole = new DelegateCommand(
                () =>
                {
                    services.AddUserRole(_name);
                });

            _runMethod = new DelegateCommand(
                () =>
                {} );
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

        public List<PersonRole> PersonRoleList
        {
            get { return new List<PersonRole>(_entities.PersonRoles); }
        }

        public DelegateCommand RunMethodCommand
        {
            get { return _runMethod; }
        }
    }
}
