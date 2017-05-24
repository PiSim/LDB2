using DBManager;
using DBManager.Services;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Controls.ViewModels
{
    public class NewPODialogViewModel : BindableBase 
    {
        private Currency _selectedCurrency;
        private DateTime _date;
        private DelegateCommand<Window> _cancel, _confirm;
        private float _total;
        private IEnumerable<Currency> _currencyList;
        private IEnumerable<Organization> _organizationList;
        private Organization _selectedOrganization;
        private string _number;
        private Views.NewPODialog _parentDialog;

        public NewPODialogViewModel() : base()
        {
            _date = DateTime.Now;
            _currencyList = DataService.GetCurrencies();
            _selectedCurrency = _currencyList.First(cur => cur.Code == "EUR");
            _organizationList = OrganizationService.GetOrganizations(OrganizationRoleNames.TestLab);

            _cancel = new DelegateCommand<Window>(
                parent =>
                {
                    parent.DialogResult = false;
                });

            _confirm = new DelegateCommand<Window>(
                parent =>
                {
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

        public IEnumerable<Currency> CurrencyList
        {
            get { return _currencyList; }
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

        public IEnumerable<Organization> OrganizationList
        {
            get
            {
                return _organizationList;
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
            _selectedOrganization = _organizationList.First(org => org.ID == target.ID);
            RaisePropertyChanged("SelectedOrganization");
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
