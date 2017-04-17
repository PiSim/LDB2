using DBManager;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizations.ViewModels
{
    public class OrganizationCreationViewModel : BindableBase
    {
        private DBEntities _entities;
        private DelegateCommand _cancel, _confirm;
        private string _organizationName;
        private Views.OrganizationCreationDialog _parentDialog;

        public OrganizationCreationViewModel(DBEntities entities,
                                            Views.OrganizationCreationDialog parentDialog) : base()
        {
            _entities = entities;
            _parentDialog = parentDialog;

            _cancel = new DelegateCommand(
                () =>
                {
                    _parentDialog.DialogResult = false;
                });

            _confirm = new DelegateCommand(
                () =>
                {
                    _parentDialog.DialogResult = true;
                });
        }

        public DelegateCommand CancelCommand
        {
            get { return _cancel; }
        }

        public DelegateCommand ConfirmCommand
        {
            get { return _confirm; }
        }
        
        public string OrganizationName
        {
            get { return _organizationName; }
            set
            {
                _organizationName = value;
            }
        }
    }
}
