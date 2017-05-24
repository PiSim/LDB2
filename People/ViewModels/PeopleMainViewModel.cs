using DBManager;
using DBManager.Services;
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
        private DelegateCommand _newPerson;
        private EventAggregator _eventAggregator;
        private Person _selectedPerson;

        public PeopleMainViewModel(EventAggregator eventAggregator) : base()
        {
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<CommitRequested>()
                            .Subscribe(() => _selectedPerson.Update());

            _eventAggregator.GetEvent<PeopleListUpdateRequested>()
                            .Subscribe(() => RaisePropertyChanged("PeopleList"));

            _newPerson = new DelegateCommand(
                () =>
                {
                    _eventAggregator.GetEvent<PersonCreationRequested>()
                                    .Publish();
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
                _selectedPerson.Load();

                RaisePropertyChanged("SelectedPerson");
                RaisePropertyChanged("PersonRoleMappingList");
            }
        }

        public IEnumerable<Person> PeopleList
        {
            get
            {
                return PeopleService.GetPeople();
            }
        }

        public IEnumerable<PersonRoleMapping> PersonRoleMappingList
        {
            get
            {
                if (_selectedPerson == null)
                    return new List<PersonRoleMapping>();

                else
                    return _selectedPerson.RoleMappings;
            }
        }
        
    }
}
