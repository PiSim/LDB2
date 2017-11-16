using DBManager;
using DBManager.EntityExtensions;
using DBManager.Services;
using Infrastructure;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Admin.ViewModels
{
    public class OrganizationEditViewModel : BindableBase
    {
        private bool _editMode;
        private DBPrincipal _principal;
        private DelegateCommand _save,
                                _startEdit;
        private IEnumerable<OrganizationRoleMapping> _roleList;
        private Organization _organizationInstance;

        public OrganizationEditViewModel(DBPrincipal principal) : base()
        {
            _editMode = false;
            _principal = principal;

            _save = new DelegateCommand(
                () =>
                {
                    _organizationInstance.Update();
                    _roleList.Update();

                    EditMode = false;
                },
                () => _editMode);

            _startEdit = new DelegateCommand(
                () =>
                {
                    EditMode = true;
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

        public Visibility OrganizationEditViewVisibility
        {
            get
            {
                if (_organizationInstance == null)
                    return Visibility.Collapsed;

                else
                    return Visibility.Visible;
            }
        }

        public Organization OrganizationInstance
        {
            get { return _organizationInstance; }
            set
            {
                _organizationInstance = value;
                _roleList = _organizationInstance.GetRoles();

                RaisePropertyChanged("RoleList");
                RaisePropertyChanged("OrganizationEditViewVisibility");
            }
        }

        public string OrganizationName
        {
            get
            {
                if (_organizationInstance == null)
                    return null;

                return _organizationInstance.Name;
            }

            set
            {
                _organizationInstance.Name = value;
            }
        }

        public IEnumerable<OrganizationRoleMapping> RoleList
        {
            get { return _roleList; }
        }

        public DelegateCommand SaveCommand
        {
            get { return _save; }
        }

        public DelegateCommand StartEditCommand
        {
            get { return _startEdit; }
        }
    }
}
