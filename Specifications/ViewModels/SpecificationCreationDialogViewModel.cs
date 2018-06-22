using DBManager;
using DBManager.EntityExtensions;
using DBManager.Services;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Specifications.ViewModels
{
    public class SpecificationCreationDialogViewModel : BindableBase, INotifyDataErrorInfo
    {
        private DelegateCommand<Window> _cancel, _confirm;
        private readonly Dictionary<string, ICollection<string>> _validationErrors = new Dictionary<string, ICollection<string>>();
        private IDataService _dataService;
        private Organization _oem;
        private Specification _specificationInstance;
        private string _currentIssue,
                        _description,
                        _name;

        public SpecificationCreationDialogViewModel(IDataService dataService) : base()
        {
            _currentIssue = "";
            _dataService = dataService;
            _description = "";

            _cancel = new DelegateCommand<Window>(
                parent =>
                {
                    parent.DialogResult = false;
                });

            _confirm = new DelegateCommand<Window>(
                parent =>
                {

                    _specificationInstance = new Specification()
                    {
                        Description = _description,
                        Name = Name
                    };

                    Std tempStd = _dataService.GetStandard(_name);

                    if (tempStd == null)
                        _specificationInstance.Standard = new Std()
                        {
                            CurrentIssue = _currentIssue,
                            Name = Name,
                            OrganizationID = _oem.ID
                        };

                    else
                        _specificationInstance.StandardID = tempStd.ID;

                    SpecificationVersion tempMain = new SpecificationVersion();
                    tempMain.Name = "Generica";
                    tempMain.IsMain = true;
                    
                    ControlPlan tempControlPlan = new ControlPlan();
                    tempControlPlan.Name = "Completo";
                    tempControlPlan.IsDefault = true;
                    
                    _specificationInstance.ControlPlans.Add(tempControlPlan);
                    _specificationInstance.SpecificationVersions.Add(tempMain);
                                        
                    parent.DialogResult = true;
                },
                parent => !HasErrors);


            Oem = null;
        }

        #region INotifyDataErrorInfo interface elements

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName)
                || !_validationErrors.ContainsKey(propertyName))
                return null;

            return _validationErrors[propertyName];
        }

        public bool HasErrors
        {
            get { return _validationErrors.Count > 0; }
        }

        private void RaiseErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            _confirm.RaiseCanExecuteChanged();
        }

        #endregion

        public DelegateCommand<Window> CancelCommand
        {
            get { return _cancel; }
        }

        public DelegateCommand<Window> ConfirmCommand
        {
            get { return _confirm; }
        }

        public string CurrentIssue
        {
            get { return _currentIssue; }
            set
            {
                _currentIssue = value;
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;

                if (!string.IsNullOrEmpty(_name)
                    && _dataService.GetSpecification(_name) == null)
                {
                    if (_validationErrors.ContainsKey("Name"))
                    {
                        _validationErrors.Remove("Name");
                        RaiseErrorsChanged("Name");
                    }
                }

                else
                {
                    _validationErrors["Name"] = new List<string>() { "Nome non valido" };
                    RaiseErrorsChanged("Name");
                }
            }
        }

        public Organization Oem
        {
            get { return _oem; }
            set
            {
                _oem = value;

                if (_oem != null)
                {
                    if (_validationErrors.ContainsKey("Oem"))
                    {
                        _validationErrors.Remove("Oem");
                        RaiseErrorsChanged("Oem");
                    }
                }

                else
                {
                    _validationErrors["Oem"] = new List<string>() { "Oem non valido" };
                    RaiseErrorsChanged("Oem");
                }
            }
        }

        public IEnumerable<Organization> OemList => _dataService.GetOrganizations(OrganizationRoleNames.StandardPublisher);

        public Specification SpecificationInstance
        {
            get
            {
                return _specificationInstance;
            }
        }
    }
}
