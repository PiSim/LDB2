using DBManager;
using Infrastructure;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizations.ViewModels
{
    public class OrganizationEditViewModel : BindableBase
    {
        private bool _editMode;
        private DBPrincipal _principal;
        private DelegateCommand _save,
                                _startEdit;
        private Organization _organizationInstance;

        public OrganizationEditViewModel(DBPrincipal principal) : base()
        {
            _editMode = false;
            _principal = principal;

            _save = new DelegateCommand(
                () =>
                {

                },
                () => _editMode);

            _startEdit = new DelegateCommand(
                () =>
                {

                },
                () => !_editMode);
        }

        public bool EditMode
        {
            get { return _editMode; }
            set
            {
                _editMode = value;
                RaisePropertyChanged("EditMode");

                _save.RaiseCanExecuteChanged();
                _startEdit.RaiseCanExecuteChanged();
            }
        }
    }
}
