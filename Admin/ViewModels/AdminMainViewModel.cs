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
        DelegateCommand _newOrganizationRole, _newUser, _newUserRole, _runMethod;
        private string _name;
        private UnityContainer _container;

        public AdminMainViewModel(ServiceProvider services,
                                    UnityContainer container) : base()
        {
            _container = container;

            _newOrganizationRole = new DelegateCommand(
                () =>
                {
                    services.AddOrganizationRole(_name);
                });

            _newUser = new DelegateCommand(
                () =>
                {
                    Views.NewUserDialog dialog = _container.Resolve<Views.NewUserDialog>();

                    if (dialog.ShowDialog() == true)
                    {

                    }
                });

            _newUserRole = new DelegateCommand(
                () =>
                {
                    services.AddUserRole(_name);
                });

            _runMethod = new DelegateCommand(
                () =>
                {
                    services.BuildOrganizationRoles();
                } );
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
            }
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

        public DelegateCommand RunMethodCommand
        {
            get { return _runMethod; }
        }
    }
}
