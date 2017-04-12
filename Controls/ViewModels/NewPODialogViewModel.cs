using DBManager;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controls.ViewModels
{
    public class NewPODialogViewModel : BindableBase 
    {
        private Currency _selectedCurrency;
        private DateTime _date;
        private DBEntities _entities;
        private DelegateCommand _cancel, _confirm;
        private float _total;
        private Organization _selectedOrganization;
        private string _number;
        private Views.NewPODialog _parentDialog;

        public NewPODialogViewModel(DBEntities entities,
                                    Views.NewPODialog parentDialog) : base()
        {
            _entities = entities;
            _parentDialog = parentDialog;
            _date = DateTime.Now;
            _selectedCurrency = _entities.Currencies.First(cur => cur.Code == "EUR");

            _cancel = new DelegateCommand(
                () =>
                {
                    _parentDialog.DialogResult = false;
                });

            _confirm = new DelegateCommand(
                () =>
                {
                    _parentDialog.Currency = _selectedCurrency;
                    _parentDialog.Date = _date;
                    _parentDialog.Number = _number;
                    _parentDialog.Supplier = _selectedOrganization;
                    _parentDialog.Total = _total;
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

        public List<Currency> CurrencyList
        {
            get { return new List<Currency>(_entities.Currencies); }
        }

        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }

        public bool IsValidInput
        {
            get { return true; }
        }

        public string Number
        {
            get { return _number; }
            set { _number = value; }
        }

        public List<Organization> OrganizationList
        {
            get
            {
                OrganizationRole supplier = _entities.OrganizationRoles
                    .First(oro => oro.Name == "TEST_LAB");

                return new List<Organization>((supplier.OrganizationMappings
                    .Where(orm => orm.IsSelected == true )
                    .Select(orm => orm.Organization)));
            }
        }

        public Currency SelectedCurrency
        {
            get { return _selectedCurrency; }
            set
            {
                _selectedCurrency = value;
            }
        }

        public Organization SelectedOrganization
        {
            get { return _selectedOrganization; }
            set
            {
                _selectedOrganization = value;
            }
        }

        public void SetOrganization(Organization target)
        {
            _selectedOrganization = OrganizationList.Find(org => org.ID == target.ID);
            OnPropertyChanged("SelectedOrganization");
        }

        public float Total
        {
            get { return _total; }
            set
            {
                _total = value;
            }
        }
    }
}
