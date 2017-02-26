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
        DelegateCommand _runMethod;

        internal AdminMainViewModel(ServiceProvider services) : base()
        {
            _runMethod = new DelegateCommand(
                () =>
                {
                    services.BuildOrganizationRoles();
                } );
        }

        public DelegateCommand RunMethodCommand
        {
            get { return _runMethod; }
        }
    }
}
