using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.ViewModels
{
    internal class AdminMainViewModel : BindableBase
    {
        DelegateCommand _newOrganizationRole, _newUserRole, _runMethod;
        private string _name;

        internal AdminMainViewModel(ServiceProvider services) : base()
        {
            _newOrganizationRole = new DelegateCommand(
                () =>
                {
                    services.AddOrganizationRole(_name);
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

        public DelegateCommand RunMethodCommand
        {
            get { return _runMethod; }
        }
    }
}
