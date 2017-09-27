using DBManager;
using DBManager.EntityExtensions;
using DBManager.Services;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Specifications.ViewModels
{
    internal class MethodCreationDialogViewModel : BindableBase
    {
        private DelegateCommand<Window> _cancel, _confirm;
        private Method _methodInstance;
        private Organization _selectedOem;
        private Property _selectedProperty;
        private string _name;

        public MethodCreationDialogViewModel() : base()
        {
            _cancel = new DelegateCommand<Window>(
                parent =>
                {
                    parent.DialogResult = false;
                });

            _confirm = new DelegateCommand<Window>(
                parent =>
                {
                    _methodInstance = new Method();

                    Std tempStd = SpecificationService.GetStandard(Name);
                    if (tempStd == null)
                    {
                        tempStd = new Std();
                        tempStd.Name = Name;
                        tempStd.OrganizationID = _selectedOem.ID;
                        tempStd.Create();
                    }

                    _methodInstance.StandardID = tempStd.ID;
                    _methodInstance.Duration = 0;
                    _methodInstance.Description = "";
                    _methodInstance.PropertyID = _selectedProperty.ID;
                    _methodInstance.UM = "";

                    _methodInstance.Create();
                    
                    parent.DialogResult = true;
                },
                parent => IsValidInput);
        }

        public DelegateCommand<Window> CancelCommand
        {
            get { return _cancel; }
        }

        public DelegateCommand<Window> ConfirmCommand
        {
            get { return _confirm; }
        }

        private bool IsValidInput
        {
            get { return _name != null && _selectedOem != null && _selectedProperty != null; }
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
                _confirm.RaiseCanExecuteChanged();
            }
        }

        public IEnumerable<Organization> OemList
        {
            get
            {
                return OrganizationService.GetOrganizations(OrganizationRoleNames.StandardPublisher);
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
                _confirm.RaiseCanExecuteChanged();
            }
        }
        
        public Property SelectedProperty
        {
            get { return _selectedProperty; }
            set
            {
                _selectedProperty = value;
                _confirm.RaiseCanExecuteChanged();
            }
        }
    }
}
