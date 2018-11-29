using DataAccessCore;
using DataAccessCore.Commands;
using Infrastructure.Commands;
using LInst;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Windows;

namespace Admin.ViewModels
{
    public class OrganizationEditViewModel : BindableBase
    {
        #region Fields

        private bool _editMode;
        private Organization _organizationInstance;
        private IDataService<LInstContext> _lInstData;

        #endregion Fields

        #region Constructors

        public OrganizationEditViewModel(IDataService<LInstContext> lInstData) : base()
        {
            _editMode = false;
            _lInstData = lInstData;

            SaveCommand = new DelegateCommand(
                () =>
                {
                    _lInstData.Execute(new UpdateEntityCommand<LInstContext>(_organizationInstance));
                    _lInstData.Execute(new BulkUpdateEntitiesCommand<LInstContext>(RoleList));

                    EditMode = false;
                },
                () => _editMode);

            StartEditCommand = new DelegateCommand(
                () =>
                {
                    EditMode = true;
                },
                () => !_editMode);
        }

        #endregion Constructors

        #region Properties

        public bool EditMode
        {
            get { return _editMode; }
            set
            {
                _editMode = value;
                RaisePropertyChanged("EditMode");

                SaveCommand.RaiseCanExecuteChanged();
                StartEditCommand.RaiseCanExecuteChanged();
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
                RoleList = _organizationInstance.RoleMappings;

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

        public IEnumerable<OrganizationRoleMapping> RoleList { get; private set; }

        public DelegateCommand SaveCommand { get; }

        public DelegateCommand StartEditCommand { get; }

        #endregion Properties
    }
}