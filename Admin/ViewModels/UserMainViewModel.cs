using Admin.Queries;
using Controls.Views;
using DataAccess;
using Infrastructure;
using Infrastructure.Commands;
using Infrastructure.Events;
using LabDbContext;
using LabDbContext.EntityExtensions;
using LabDbContext.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Linq;

namespace Admin.ViewModels
{
    public class UserMainViewModel : BindableBase
    {
        #region Fields

        private IAdminService _adminService;
        IDataService<LabDbEntities> _labDbData;
        private IEventAggregator _eventAggregator;
        private User _selectedUser;
        private IEnumerable<User> _userList;

        #endregion Fields

        #region Constructors

        public UserMainViewModel(IEventAggregator eventAggregator,
                                IAdminService adminService,
                                IDataService<LabDbEntities> labDbData) : base()
        {
            _adminService = adminService;
            _eventAggregator = eventAggregator;
            _labDbData = labDbData;

            _userList = _labDbData.RunQuery(new UsersQuery()).ToList();

            CreateNewUserCommand = new DelegateCommand(
                () =>
                {
                    _adminService.CreateNewUser();

                    UserList = _labDbData.RunQuery(new UsersQuery()).ToList();
                });

            DeleteUserCommand = new DelegateCommand(
                () =>
                {
                    _labDbData.Execute(new DeleteEntityCommand(_selectedUser));
                    UserList = _labDbData.RunQuery(new UsersQuery()).ToList();
                },
                () => _selectedUser != null);
        }

        #endregion Constructors

        #region Properties

        public string AdminUserEditRegionName => RegionNames.AdminUserEditRegion;

        public DelegateCommand CreateNewUserCommand { get; }

        public DelegateCommand DeleteUserCommand { get; }

        public User SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                DeleteUserCommand.RaiseCanExecuteChanged();

                NavigationToken token = new NavigationToken(AdminViewNames.UserEditView,
                                                            _selectedUser,
                                                            AdminUserEditRegionName);

                _eventAggregator.GetEvent<NavigationRequested>()
                                .Publish(token);
            }
        }

        public IEnumerable<User> UserList
        {
            get { return _userList; }

            private set
            {
                _userList = value;
                RaisePropertyChanged("UserList");
            }
        }

        #endregion Properties
    }
}