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
        private bool _editMode;
        private DelegateCommand _newPerson,
                                _save;
        private EventAggregator _eventAggregator;
        private Person _selectedPerson;

        public PeopleMainViewModel(EventAggregator eventAggregator) : base()
        {
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<PeopleListUpdateRequested>()
                            .Subscribe(() => RaisePropertyChanged("PeopleList"));

            _newPerson = new DelegateCommand(
                () =>
                {
                    _eventAggregator.GetEvent<PersonCreationRequested>()
                                    .Publish();
                });

            _save = new DelegateCommand(
                () =>
                {
                    _selectedPerson.Update();
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
