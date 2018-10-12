using DataAccess;
using Infrastructure.Commands;
using LabDbContext;
using LabDbContext.EntityExtensions;
using LabDbContext.Services;
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
        private IDataService<LabDbEntities> _labDbData;

        #endregion Fields

        #region Constructors

        public OrganizationEditViewModel(IDataService<LabDbEntities> labDbData) : base()
        {
            _editMode = false;
            _labDbData = labDbData;

            SaveCommand = new DelegateCommand(
                () =>
                {
                    _labDbData.Execute(new UpdateEntityCommand(_organizationInstance));
                    _labDbData.Execute(new BulkUpdateEntitiesCommand(RoleList));

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
                RoleList = _organizationInstance.GetRoles();

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