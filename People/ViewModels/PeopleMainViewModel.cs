using DBManager;
using Infrastructure;
using Infrastructure.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace People.ViewModels
{
    public class PeopleMainViewModel : BindableBase
    {
        private DBEntities _entities;
        private DelegateCommand _newPerson;
        private EventAggregator _eventAggregator;
        private IAdminServiceProvider _adminServiceProvider;
        private Person _selectedPerson;

        public PeopleMainViewModel(DBEntities entities,
                                    EventAggregator aggregator,
                                    IAdminServiceProvider adminServiceProvider) : base()
        {
            _entities = entities;
            _eventAggregator = aggregator;
            _adminServiceProvider = adminServiceProvider;

            _eventAggregator.GetEvent<CommitRequested>()
                            .Subscribe(() => _entities.SaveChanges());

            _newPerson = new DelegateCommand(
                () =>
                {
                    _adminServiceProvider.AddPerson();
                    RaisePropertyChanged("PeopleList");
                });
        }

        public DelegateCommand CreateNewPersonCommand
        {
            get { return _newPerson; }
        }
        
        public Person SelectedPerson
        {
            get { return _selectedPerson; }
            set
            {
                _selectedPerson = value;
                RaisePropertyChanged("SelectedPerson");
                RaisePropertyChanged("PersonRoleMappingList");
            }
        }

        public List<Person> PeopleList
        {
            get
            {
                return new List<Person>(_entities.People.OrderBy(per => per.Name));
            }
        }

        public List<PersonRoleMapping> PersonRoleMappingList
        {
            get
            {
                if (_selectedPerson == null)
                    return new List<PersonRoleMapping>();

                else
                    return new List<PersonRoleMapping>
                        (_selectedPerson.RoleMappings);
            }
        }
        
    }
}
