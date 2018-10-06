using Controls.Views;
using Infrastructure;
using Infrastructure.Events;
using LabDbContext;
using LabDbContext.EntityExtensions;
using LabDbContext.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;

namespace Admin.ViewModels
{
    public class UserMainViewModel : BindableBase
    {
        #region Fields

        private IAdminService _adminService;
        private IDataService _dataService;
        private IEventAggregator _eventAggregator;
        private User _selectedUser;
        private IEnumerable<User> _userList;

        #endregion Fields

        #region Constructors

        public UserMainViewModel(IEventAggregator eventAggregator,
                                IAdminService adminService,
                                IDataService dataService) : base()
        {
            _adminService = adminService;
            _eventAggregator = eventAggregator;
            _dataService = dataService;

            _userList = _dataService.GetUsers();

            CreateNewUserCommand = new DelegateCommand(
                () =>
                {
                    _adminService.CreateNewUser();

                    UserList = _dataService.GetUsers();
                });

            DeleteUserCommand = new DelegateCommand(
                () =>
                {
                    _selectedUser.Delete();

                    UserList = _dataService.GetUsers();
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