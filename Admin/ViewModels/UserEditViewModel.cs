using DataAccess;
using Infrastructure.Queries;
using LabDbContext;
using LabDbContext.EntityExtensions;
using LabDbContext.Services;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Linq;

namespace Admin.ViewModels
{
    public class UserEditViewModel : BindableBase
    {
        #region Fields

        private bool _editMode;
        private IDataService<LabDbEntities> _labDbData;
        private IEnumerable<UserRoleMapping> _roleList;
        private User _userInstance;

        #endregion Fields

        #region Constructors

        public UserEditViewModel(IDataService<LabDbEntities> labDbData) : base()
        {
            _labDbData = labDbData;
            _editMode = false;
            PeopleList = _labDbData.RunQuery(new PeopleQuery()).ToList();

            SaveCommand = new DelegateCommand(
                () =>
                {
                    _userInstance.Update();
                    _roleList.Update();
                    EditMode = false;
                },
                () => _editMode == true);

            StartEditCommand = new DelegateCommand(
                () =>
                {
                    EditMode = true;
                },
                () => _editMode == false && _userInstance != null);
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

        public IEnumerable<Person> PeopleList { get; }

        public IEnumerable<UserRoleMapping> RoleList
        {
            get { return _roleList; }

            private set
            {
                _roleList = value;
                RaisePropertyChanged("RoleList");
            }
        }

        public DelegateCommand SaveCommand { get; }

        public DelegateCommand StartEditCommand { get; }

        public User UserInstance
        {
            get { return _userInstance; }
            set
            {
                _userInstance = value;
                RoleList = _userInstance.GetRoles();
                EditMode = false;
            }
        }

        #endregion Properties
    }
}