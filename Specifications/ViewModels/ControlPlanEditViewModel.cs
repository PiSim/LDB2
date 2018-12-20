using DataAccess;
using Infrastructure;
using Infrastructure.Commands;
using LabDbContext;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Threading;

namespace Specifications.ViewModels
{
    public class ControlPlanEditViewModel : BindableBase
    {
        #region Fields

        private ControlPlan _controlPlanInstance;

        private bool _editMode,
                            _hasCredentials;

        private IDataService<LabDbEntities> _labDbData;

        #endregion Fields

        #region Constructors

        public ControlPlanEditViewModel(IDataService<LabDbEntities> labDbData)
        {
            _labDbData = labDbData;
            _hasCredentials = Thread.CurrentPrincipal.IsInRole(UserRoleNames.SpecificationEdit);

            SaveCommand = new DelegateCommand(
                () =>
                {
                    _controlPlanInstance.control_plan_items_b = ControlPlanItemsList as ICollection<ControlPlanItem>;

                    _labDbData.Execute(new UpdateEntityCommand(_controlPlanInstance));
                    EditMode = false;
                },
                () => _hasCredentials && EditMode);

            StartEditCommand = new DelegateCommand(
                () =>
                {
                    EditMode = true;
                },
                () => _hasCredentials &&
                        _controlPlanInstance != null &&
                        !_controlPlanInstance.IsDefault &&
                        !EditMode);
        }

        #endregion Constructors

        #region Properties

        public ControlPlan ControlPlanInstance
        {
            get { return _controlPlanInstance; }

            set
            {
                _controlPlanInstance = value;
                RaisePropertyChanged("ControlPlanInstance");

                EditMode = false;

                ControlPlanItemsList = (value == null) ? new List<ControlPlanItem>() :
                                                            _controlPlanInstance.GetControlPlanItems(true);

                RaisePropertyChanged("Name");
                RaisePropertyChanged("ControlPlanItemsList");
            }
        }

        public IEnumerable<ControlPlanItem> ControlPlanItemsList { get; private set; }

        public bool EditMode
        {
            get { return _editMode; }
            set
            {
                _editMode = value;
                RaisePropertyChanged("EditMode");
                SaveCommand.RaiseCanExecuteChanged();
                StartEditCommand.RaiseCanExecuteChanged();
            }
        }

        public string Name
        {
            get { return _controlPlanInstance?.Name; }
            set
            {
                _controlPlanInstance.Name = value;
            }
        }

        public DelegateCommand SaveCommand { get; }

        public DelegateCommand StartEditCommand { get; }

        #endregion Properties
    }
}