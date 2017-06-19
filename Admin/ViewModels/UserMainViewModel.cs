using DBManager;
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
        }

        public string AdminUserEditRegionName
        {
            get { return RegionNames.AdminUserEditRegion; }
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
        }
    }
}
