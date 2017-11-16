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
    internal class MethodCreationDialogViewModel : BindableBase, INotifyDataErrorInfo
    {
        private DelegateCommand<Window> _cancel, _confirm;
        private readonly Dictionary<string, ICollection<string>> _validationErrors = new Dictionary<string, ICollection<string>>();
        private IEnumerable<Organization> _oemList;
        private Method _methodInstance;
        private Organization _selectedOem;
        private Property _selectedProperty;
        private Std _standardInstance;
        private string _name;

        public MethodCreationDialogViewModel() : base()
        {
            _oemList = OrganizationService.GetOrganizations(OrganizationRoleNames.StandardPublisher);

            _cancel = new DelegateCommand<Window>(
                parent =>
                {
                    parent.DialogResult = false;
                });

            _confirm = new DelegateCommand<Window>(
                parent =>
                {
                    _methodInstance = new Method();

                    if (_standardInstance == null)
                    {
                        _standardInstance = new Std
                        {
                            Name = Name,
                            OrganizationID = _selectedOem.ID
                        };

                        _methodInstance.Standard = _standardInstance;
                    }

                    else
                    {
                        _standardInstance.Update();
                        _methodInstance.StandardID = _standardInstance.ID;
                    }

                    _methodInstance.Duration = 0;
                    _methodInstance.Description = "";
                    _methodInstance.PropertyID = _selectedProperty.ID;
                    _methodInstance.UM = "";

                    _methodInstance.Create();
                    
                    parent.DialogResult = true;
                },
                parent => !HasErrors);

            Name = "";
            SelectedOem = null;
            SelectedProperty = null;
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
        
        public Method MethodInstance
        {
            get { return _methodInstance; }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;

                _standardInstance = SpecificationService.GetStandard(_name);

                if (_standardInstance != null)
                    SelectedOem = OemList.FirstOrDefault(oem => oem.ID == _standardInstance.OrganizationID);


                if (!string.IsNullOrEmpty(_name))
                {
                    if (_validationErrors.ContainsKey("Name"))
                        _validationErrors.Remove("Name");
                }
                else
                    _validationErrors["Name"] = new List<string>() { "Nome non valido" };

                RaiseErrorsChanged("Name");
            }
        }

        public IEnumerable<Organization> OemList
        {
            get
            {
                return _oemList;
            }
        }

        public IEnumerable<Property> PropertiesList
        {
            get { return DataService.GetProperties(); }
        }

        public Organization SelectedOem
        {
            get { return _selectedOem; }
            set
            {
                _selectedOem = value;

                if (_standardInstance != null)
                    _standardInstance.OrganizationID = _selectedOem.ID;

                if (_selectedOem != null)
                {
                        if (_validationErrors.ContainsKey("SelectedOem"))
                        _validationErrors.Remove("SelectedOem");
                }
                else
                    _validationErrors["SelectedOem"] = new List<string>() { "Oem non valido" };

                RaisePropertyChanged("SelectedOem");
                RaiseErrorsChanged("SelectedOem");
                
            }
        }
        
        public Property SelectedProperty
        {
            get { return _selectedProperty; }
            set
            {
                _selectedProperty = value;

                if (_selectedProperty != null)
                {
                    if (_validationErrors.ContainsKey("SelectedProperty"))
                        _validationErrors.Remove("SelectedProperty");
                }
                else
                    _validationErrors["SelectedProperty"] = new List<string>() { "Proprietä non valida" };

                RaiseErrorsChanged("SelectedProperty");
            }
        }
    }
}
