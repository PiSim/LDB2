using DBManager;
using DBManager.EntityExtensions;
using DBManager.Services;
using Infrastructure;
using Infrastructure.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.ViewModels
{
    public class UserMainViewModel : BindableBase
    {
        private DelegateCommand _createNewUser, _deleteUser;
        private EventAggregator _eventAggregator;
        private IAdminService _adminService;
        private IDataService _dataService;
        private IEnumerable<User> _userList;
        private User _selectedUser;

        public UserMainViewModel(EventAggregator eventAggregator,
                                IAdminService adminService,
                                IDataService dataService) : base()
        {
            _adminService = adminService;
            _eventAggregator = eventAggregator;
            _dataService = dataService;

            _userList = _dataService.GetUsers();

            _createNewUser = new DelegateCommand(
                () =>
                {
                    _adminService.CreateNewUser();

                    UserList = _dataService.GetUsers();
                });

            _deleteUser = new DelegateCommand(
                () =>
                {
                    _selectedUser.Delete();

                    UserList = _dataService.GetUsers();
                },
                () => _selectedUser != null);
        }

        public string AdminUserEditRegionName
        {
            get { return RegionNames.AdminUserEditRegion; }
        }

        public DelegateCommand CreateNewUserCommand
        {
            get { return _createNewUser; }
        }

        public DelegateCommand DeleteUserCommand
        {
            get { return _deleteUser; }
        }

        public User SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                _deleteUser.RaiseCanExecuteChanged();

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
    }
}
