using DBManager;
using DBManager.EntityExtensions;
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
        private IEnumerable<User> _userList;
        private User _selectedUser;

        public UserMainViewModel(EventAggregator eventAggregator) : base()
        {
            _eventAggregator = eventAggregator;

            _userList = DataService.GetUsers();

            _createNewUser = new DelegateCommand(
                () =>
                {
                    _eventAggregator.GetEvent<UserCreationRequested>()
                                    .Publish();

                    UserList = DataService.GetUsers();
                });

            _deleteUser = new DelegateCommand(
                () =>
                {
                    _selectedUser.Delete();

                    UserList = DataService.GetUsers();
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
