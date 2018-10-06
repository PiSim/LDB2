using DataAccess;
using Infrastructure;
using Infrastructure.Events;
using Infrastructure.Queries;
using LabDbContext;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Linq;

namespace Admin.ViewModels
{
    public class PeopleMainViewModel : BindableBase
    {
        #region Fields

        private IAdminService _adminService;
        private IEventAggregator _eventAggregator;
        private IDataService<LabDbEntities> _labDbData;
        private DelegateCommand _save;
        private Person _selectedPerson;

        #endregion Fields

        #region Constructors

        public PeopleMainViewModel(IEventAggregator eventAggregator,
                                    IAdminService adminService,
                                    IDataService<LabDbEntities> labDbData) : base()
        {
            _adminService = adminService;
            _labDbData = labDbData;
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<PersonChanged>()
                            .Subscribe(ect => RaisePropertyChanged("PeopleList"));

            CreateNewPersonCommand = new DelegateCommand(
                () =>
                {
                    _adminService.CreateNewPerson();
                });

            _save = new DelegateCommand(
                () =>
                {
                    _selectedPerson.Update();
                });
        }

        #endregion Constructors

        #region Properties

        public DelegateCommand CreateNewPersonCommand { get; }

        public IEnumerable<Person> PeopleList => _labDbData.RunQuery(new PeopleQuery()).ToList();

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

        #endregion Properties
    }
}