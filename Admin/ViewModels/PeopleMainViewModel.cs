using DataAccessCore;
using DataAccessCore.Commands;
using Infrastructure;
using Infrastructure.Commands;
using Infrastructure.Events;
using Infrastructure.Queries;
using LInst;
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
        private IDataService<LInstContext> _lInstData;
        private DelegateCommand _save;
        private Person _selectedPerson;

        #endregion Fields

        #region Constructors

        public PeopleMainViewModel(IEventAggregator eventAggregator,
                                    IAdminService adminService,
                                    IDataService<LInstContext> lInstData) : base()
        {
            _adminService = adminService;
            _lInstData = lInstData;
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
                    _lInstData.Execute(new UpdateEntityCommand<LInstContext>(_selectedPerson));
                });
        }

        #endregion Constructors

        #region Properties

        public DelegateCommand CreateNewPersonCommand { get; }

        public IEnumerable<Person> PeopleList => _lInstData.RunQuery(new PeopleQuery()).ToList();

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
                _lInstData.Execute(new ReloadEntityCommand<LInstContext>(_selectedPerson));
                RaisePropertyChanged("SelectedPerson");
                RaisePropertyChanged("PersonRoleMappingList");
            }
        }

        #endregion Properties
    }
}