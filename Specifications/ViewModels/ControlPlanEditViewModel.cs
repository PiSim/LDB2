using DBManager;
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

namespace Specifications.ViewModels
{
    public class ControlPlanEditViewModel : BindableBase
    {

        private bool _editMode,
                    _hasCredentials;
        private ControlPlan _controlPlanInstance;
        private DBPrincipal _principal;
        private DelegateCommand _save,
                                _startEdit;
        private IEnumerable<ControlPlanItemB> _controlPlanItemsList;

        public ControlPlanEditViewModel(DBPrincipal principal)
        {
            _principal = principal;
            _hasCredentials = _principal.IsInRole(UserRoleNames.SpecificationEdit);

            _save = new DelegateCommand(
                () =>
                {
                    _controlPlanInstance.control_plan_items_b = _controlPlanItemsList as ICollection<ControlPlanItemB>;
                    _controlPlanInstance.Update();
                    EditMode = false;
                },
                () => _hasCredentials && EditMode);

            _startEdit = new DelegateCommand(
                () =>
                {
                    EditMode = true;
                },
                () => _hasCredentials &&
                        _controlPlanInstance != null &&
                        !_controlPlanInstance.IsDefault && 
                        !EditMode );
        }

        public ControlPlan ControlPlanInstance
        {
            get { return _controlPlanInstance; }

            set
            {
                _controlPlanInstance = value;
                RaisePropertyChanged("ControlPlanInstance");

                EditMode = false;

                _controlPlanItemsList = (value == null) ? new List<ControlPlanItemB>() :
                                                            _controlPlanInstance.GetControlPlanItems(true);

                RaisePropertyChanged("Name");
                RaisePropertyChanged("ControlPlanItemsList");
            }
        }
        
        public IEnumerable<ControlPlanItemB> ControlPlanItemsList
        {
            get { return _controlPlanItemsList; }
        }

        public bool EditMode
        {
            get { return _editMode; }
            set
            {
                _editMode = value;
                RaisePropertyChanged("EditMode");
                _save.RaiseCanExecuteChanged();
                _startEdit.RaiseCanExecuteChanged();
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
        
        public DelegateCommand SaveCommand
        {
            get { return _save; }
        }

        public DelegateCommand StartEditCommand
        {
            get { return _startEdit; }
        }
    }
}
