using DBManager;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Specifications.ViewModels
{
    internal class MethodCreationViewModel : BindableBase
    {
        private DBEntities _entities;
        private DelegateCommand _cancel, _confirm;
        private Organization _selectedOem;
        private Property _selectedProperty;
        private string _name;
        private Views.MethodCreationDialog _parentDialog;

        internal MethodCreationViewModel(DBEntities entities,
                                        Views.MethodCreationDialog parentDialog) : base()
        {
            _entities = entities;
            _parentDialog = parentDialog;

            _cancel = new DelegateCommand(
                () =>
                {
                    _parentDialog.DialogResult = false;
                });

            _confirm = new DelegateCommand(
                () =>
                {
                    Method output = new Method();

                    Std tempStd = _entities.Stds.FirstOrDefault(std => std.Name == _name);
                    if (tempStd == null)
                    {
                        tempStd = new Std();
                        tempStd.Name = Name;
                        tempStd.Organization = _selectedOem;
                    }

                    output.Standard = tempStd;
                    output.CostUnits = 0;
                    output.Description = "";
                    output.Property = _selectedProperty;
                    output.UM = "";

                    _entities.Methods.Add(output);
                    _entities.SaveChanges();

                    _parentDialog.MethodInstance = output;
                    _parentDialog.DialogResult = true;
                }, 
                () => IsValidInput);
        }

        public DelegateCommand CancelCommand
        {
            get { return _cancel; }
        }

        public DelegateCommand ConfirmCommand
        {
            get { return _confirm; }
        }

        private bool IsValidInput
        {
            get { return _name != null && _selectedOem != null && _selectedProperty != null; }
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

        public List<Organization> OemList
        {
            get
            {
                return new List<Organization>
                    (_entities.Organizations.Where(org => org.RoleMapping
                                            .Any(orm => orm.Role.Name == "OEM" && orm.IsSelected)));
            }
        }

        public List<Property> PropertiesList
        {
            get { return new List<Property>(_entities.Properties); }
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
